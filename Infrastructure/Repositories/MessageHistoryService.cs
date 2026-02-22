using Application.Common.Models;
using Application.Features.SMS.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MessageHistoryService : GenericRepository<MessageHistory>, IMessageHistoryService
    {
        private readonly AppDbContext _db;

        public MessageHistoryService(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ApiResult<PagedResult<MessageHistoryListItemDto>>> SearchAsync(
            MessageHistoryListFilterDto filter,
            CancellationToken ct = default)
        {
            filter ??= new MessageHistoryListFilterDto();
            if (filter.Page <= 0) filter.Page = 1;
            if (filter.PageSize <= 0) filter.PageSize = 20;
            if (filter.PageSize > 200) filter.PageSize = 200;

            var q = _db.Set<MessageHistory>().AsNoTracking().AsQueryable();

            // filters
            if (filter.Channel.HasValue)
                q = q.Where(x => x.Channel == filter.Channel.Value);

            if (filter.Status.HasValue)
                q = q.Where(x => x.Status == filter.Status.Value);

            if (!string.IsNullOrWhiteSpace(filter.Recipient))
            {
                var r = filter.Recipient.Trim();
                q = q.Where(x => x.Recipient.Contains(r));
            }

            if (filter.TemplateId.HasValue)
                q = q.Where(x => x.TemplateId == filter.TemplateId.Value);

            if (!string.IsNullOrWhiteSpace(filter.ProviderName))
            {
                var p = filter.ProviderName.Trim();
                q = q.Where(x => x.ProviderName == p);
            }

            // date range (بهترین فیلد برای لیست: SendRequestedAt؛ اگر null بود می‌تونی CreatedAt/BaseEntity رو جایگزین کنی)
            if (filter.From.HasValue)
                q = q.Where(x => x.SendRequestedAt >= filter.From.Value);

            if (filter.To.HasValue)
                q = q.Where(x => x.SendRequestedAt <= filter.To.Value);

            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var k = filter.Keyword.Trim();
                q = q.Where(x =>
                    x.Recipient.Contains(k) ||
                    x.Content.Contains(k) ||
                    (x.ProviderStatusText != null && x.ProviderStatusText.Contains(k)) ||
                    (x.DeliveryStatusText != null && x.DeliveryStatusText.Contains(k)) ||
                    x.ProviderName.Contains(k)
                );
            }

            // total
            var total = await q.CountAsync(ct);

            // page
            var skip = (filter.Page - 1) * filter.PageSize;

            var items = await q
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(filter.PageSize)
                .Select(x => new MessageHistoryListItemDto
                {
                    Id = x.Id,
                    Channel = x.Channel,
                    Recipient = x.Recipient,
                    Content = x.Content,
                    TemplateId = x.TemplateId,
                    Status = x.Status,
                    ProviderName = x.ProviderName,
                    ProviderMessageId = x.ProviderMessageId,
                    ProviderStatus = x.ProviderStatus,
                    ProviderStatusText = x.ProviderStatusText,
                    DeliveryStatus = x.DeliveryStatus,
                    DeliveryStatusText = x.DeliveryStatusText,
                    Cost = x.Cost,
                    SendRequestedAt = x.SendRequestedAt,
                    SentAt = x.SentAt,
                    DeliveredAt = x.DeliveredAt,
                    ActivityLogId = x.ActivityLogId
                })
                .ToListAsync(ct);

            var result = new PagedResult<MessageHistoryListItemDto>
            {
                Items = items,
                TotalCount = total,
                Page = filter.Page,
                PageSize = filter.PageSize
            };

            return ApiResult<PagedResult<MessageHistoryListItemDto>>.Success(result);
        }
    }
}
