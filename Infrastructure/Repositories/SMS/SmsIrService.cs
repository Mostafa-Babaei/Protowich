using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Application.Features.SMS.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories.SMS
{
    public class SmsIrService : ISmsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SmsIrOptions _opt;
        private readonly AppDbContext _db;

        private static readonly JsonSerializerOptions JsonOpt = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public SmsIrService(IHttpClientFactory httpClientFactory, IOptions<SmsIrOptions> options, AppDbContext db)
        {
            _httpClientFactory = httpClientFactory;
            _opt = options.Value;
            _db = db;
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient(nameof(SmsIrService));
            client.BaseAddress = new Uri(_opt.BaseUrl.TrimEnd('/') + "/");
            client.Timeout = TimeSpan.FromSeconds(_opt.TimeoutSeconds);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Remove("X-API-KEY");
            client.DefaultRequestHeaders.Add("X-API-KEY", _opt.ApiKey);

            return client;
        }

        public async Task<SmsIrResponse> SendVerifyAsync(string mobile, int templateId, Dictionary<string, string> parameters, Guid? actorUserId = null)
        {
            // 1) insert message_history: pending
            var message = await InsertMessageAsync(mobile, $"VERIFY TEMPLATE #{templateId}", templateId, actorUserId);

            try
            {
                var payload = new SmsIrVerifyRequest
                {
                    Mobile = mobile,
                    TemplateId = templateId,
                    Parameters = parameters.Select(p => new SmsIrParameter
                    {
                        Name = p.Key.ToUpperInvariant(),
                        Value = p.Value
                    }).ToList()
                };

                var response = await PostAsync("send/verify", payload);

                await HandleResponseAsync(message.Id, response, scheduledAt: null);
                return response;
            }
            catch (Exception ex)
            {
                await MarkFailedAsync(message.Id, ex.Message);
                throw;
            }
        }

        public async Task<SmsIrResponse> SendTextAsync(string mobile, string content, Guid? actorUserId = null, DateTime? sendDateTime = null)
        {
            var message = await InsertMessageAsync(mobile, content, templateId: null, actorUserId);

            long? unixSendTime = null;
            if (sendDateTime.HasValue)
            {
                var utc = DateTime.SpecifyKind(sendDateTime.Value, DateTimeKind.Local).ToUniversalTime();
                unixSendTime = new DateTimeOffset(utc).ToUnixTimeSeconds();
            }

            try
            {
                var payload = new SmsIrBulkRequest
                {
                    LineNumber = _opt.LineNumber,
                    MessageText = content,
                    Mobiles = new List<string> { mobile },
                    SendDateTime = unixSendTime
                };

                var response = await PostAsync("send/bulk", payload);

                // اگر scheduled بود می‌تونی SentAt رو همون زمان ست کنی یا null نگه داری
                await HandleResponseAsync(message.Id, response, scheduledAt: sendDateTime);
                return response;
            }
            catch (Exception ex)
            {
                await MarkFailedAsync(message.Id, ex.Message);
                throw;
            }
        }

        public async Task<double> GetBalanceAsync()
        {
            using var client = CreateClient();
            using var resp = await client.GetAsync("credit");

            var text = await resp.Content.ReadAsStringAsync();
            resp.EnsureSuccessStatusCode();

            var json = JsonSerializer.Deserialize<SmsIrResponse>(text, JsonOpt)
                       ?? throw new Exception("Invalid JSON response from SMS.IR");

            if (json.Status != 1)
                throw new Exception("خطا در دریافت اعتبار پیامک: " + (json.Message ?? "Unknown error"));

            // در PHP: return (float)$json['data'];
            // اما DTO ما Data را object گرفت. اگر واقعاً data عدد خالص است، DTO جدا بهتره:
            // اینجا برای سازگاری، از JsonDocument می‌خونیم:
            using var doc = JsonDocument.Parse(text);
            if (!doc.RootElement.TryGetProperty("data", out var dataEl))
                throw new Exception("Invalid credit response");

            if (dataEl.ValueKind == JsonValueKind.Number && dataEl.TryGetDouble(out var v))
                return v;

            // گاهی ممکنه رشته باشد
            if (dataEl.ValueKind == JsonValueKind.String && double.TryParse(dataEl.GetString(), out var vs))
                return vs;

            throw new Exception("Invalid credit value");
        }

        public async Task<SmsIrResponse> QueryStatusAsync(long providerMessageId)
        {
            var payload = new SmsIrStatusRequest { MessageId = providerMessageId };
            return await PostAsync("send/status", payload);
        }

        public async Task<BulkSmsSendResult> SendBulkTextAsync(
            List<string> mobiles,
            string content,
            Guid? actorUserId = null,
            DateTime? sendDateTime = null,
            CancellationToken ct = default)
        {
            mobiles = mobiles?
                .Select(x => (x ?? "").Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList() ?? new();

            if (mobiles.Count == 0)
                return new BulkSmsSendResult { Total = 0, Sent = 0, Failed = 0 };

            // محدودیت‌های سرویس‌دهنده‌ها معمولاً برای bulk وجود دارد؛ batch می‌کنیم
            const int batchSize = 500; // اگر لازم شد کم/زیاد کن

            int total = mobiles.Count;
            int sent = 0;
            int failed = 0;

            for (int i = 0; i < mobiles.Count; i += batchSize)
            {
                ct.ThrowIfCancellationRequested();

                var batch = mobiles.Skip(i).Take(batchSize).ToList();

                // 1) insert message_history for each recipient as PENDING
                var rows = batch.Select(m => new MessageHistory
                {
                    Channel = NotificationChannel.SMS,
                    Recipient = m,
                    Content = content,
                    TemplateId = null,
                    Status = MessageStatusEnum.PENDING,
                    ProviderName = _opt.ProviderName,
                    SendRequestedAt = DateTime.UtcNow
                }).ToList();

                _db.Set<MessageHistory>().AddRange(rows);
                await _db.SaveChangesAsync(ct);

                // 2) call SMS.IR bulk
                long? unixSendTime = null;
                if (sendDateTime.HasValue)
                {
                    var utc = DateTime.SpecifyKind(sendDateTime.Value, DateTimeKind.Local).ToUniversalTime();
                    unixSendTime = new DateTimeOffset(utc).ToUnixTimeSeconds();
                }

                try
                {
                    var payload = new SmsIrBulkRequest
                    {
                        LineNumber = _opt.LineNumber,
                        MessageText = content,
                        Mobiles = batch,
                        SendDateTime = unixSendTime
                    };

                    var response = await PostAsync("send/bulk", payload);

                    var isOk = response.Status == 1;
                    var statusText = SmsStatusText(response.Status);

                    // اگر provider ids به تعداد موبایل‌ها برگشت می‌تونیم map کنیم؛
                    // اگر نه، فقط status کلی را ست می‌کنیم.
                    var providerIds = response.Data?.MessageIds;

                    for (int r = 0; r < rows.Count; r++)
                    {
                        rows[r].Status = isOk ? MessageStatusEnum.SENT : MessageStatusEnum.FAILED;
                        rows[r].ProviderStatus = response.Status;
                        rows[r].ProviderStatusText = statusText;

                        if (providerIds != null && r < providerIds.Count)
                            rows[r].ProviderMessageId = providerIds[r];

                        rows[r].SentAt = isOk ? DateTime.UtcNow : null;
                    }

                    // هزینه اگر کل batch باشد، تقسیمش می‌کنیم (اختیاری)
                    if (response.Data?.Cost != null)
                    {
                        var per = (decimal)response.Data.Cost / Math.Max(1, rows.Count);
                        foreach (var row in rows) row.Cost = per;
                    }

                    await _db.SaveChangesAsync(ct);

                    if (isOk) sent += rows.Count;
                    else failed += rows.Count;
                }
                catch (Exception ex)
                {
                    // Mark all failed for this batch
                    foreach (var row in rows)
                    {
                        row.Status = MessageStatusEnum.FAILED;
                        row.ProviderStatusText = ex.Message;
                    }
                    await _db.SaveChangesAsync(ct);

                    failed += rows.Count;
                }
            }

            return new BulkSmsSendResult
            {
                Total = total,
                Sent = sent,
                Failed = failed
            };
        }


        /* =========================
           DB helpers
        ========================= */
        private async Task<MessageHistory> InsertMessageAsync(string mobile, string content, int? templateId, Guid? actorUserId)
        {
            var m = new MessageHistory
            {
                Channel = NotificationChannel.SMS,
                Recipient = mobile,
                Content = content,
                TemplateId = templateId,
                Status = MessageStatusEnum.PENDING,
                ProviderName = _opt.ProviderName,
                SendRequestedAt = DateTime.UtcNow
            };

            _db.Set<MessageHistory>().Add(m);
            await _db.SaveChangesAsync();
            return m;
        }

        private async Task HandleResponseAsync(long messageId, SmsIrResponse response, DateTime? scheduledAt)
        {
            var status = response.Status;
            var statusText = SmsStatusText(status);

            var providerMessageId = response.Data?.MessageIds?.FirstOrDefault();
            var cost = response.Data?.Cost;

            var finalStatus = (status == 1) ? MessageStatusEnum.SENT : MessageStatusEnum.FAILED;

            var m = await _db.Set<MessageHistory>().FirstAsync(x => x.Id == messageId);
            m.Status = finalStatus;
            m.ProviderStatus = status;
            m.ProviderStatusText = statusText;
            m.ProviderMessageId = providerMessageId == 0 ? null : providerMessageId;
            m.Cost = cost;
            m.SentAt = scheduledAt?.ToUniversalTime(); // یا null
            await _db.SaveChangesAsync();
        }

        private async Task MarkFailedAsync(long messageId, string error)
        {
            var m = await _db.Set<MessageHistory>().FirstAsync(x => x.Id == messageId);
            m.Status = MessageStatusEnum.FAILED;
            m.ProviderStatusText = error;
            await _db.SaveChangesAsync();
        }

        /* =========================
           HTTP helper
        ========================= */
        private async Task<SmsIrResponse> PostAsync(string endpoint, object payload)
        {
            using var client = CreateClient();

            var json = JsonSerializer.Serialize(payload, JsonOpt);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var resp = await client.PostAsync(endpoint.TrimStart('/'), content);

            var text = await resp.Content.ReadAsStringAsync();

            // اگر sms.ir خطای 4xx/5xx داد، متن رو هم بده
            if (!resp.IsSuccessStatusCode)
                throw new Exception($"SMS.IR HTTP {(int)resp.StatusCode}: {text}");

            var dto = JsonSerializer.Deserialize<SmsIrResponse>(text, JsonOpt);
            if (dto == null)
                throw new Exception("Invalid JSON response from SMS.IR");

            return dto;
        }

        /* =========================
           Status mapper (مثل SmsStatusMapper.php)
        ========================= */
        private static string SmsStatusText(int status) => status switch
        {
            1 => "sent",
            0 => "failed",
            _ => "unknown"
        };
    }
}
