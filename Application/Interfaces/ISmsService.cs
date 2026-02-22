using Application.Features.SMS.Dtos;

namespace Application.Interfaces
{

    public interface ISmsService
    {
        Task<SmsIrResponse> SendVerifyAsync(string mobile, int templateId, Dictionary<string, string> parameters, Guid? actorUserId = null);
        Task<SmsIrResponse> SendTextAsync(string mobile, string content, Guid? actorUserId = null, DateTime? sendDateTime = null);
        Task<BulkSmsSendResult> SendBulkTextAsync(List<string> mobiles, string content, Guid? actorUserId = null, DateTime? sendDateTime = null, CancellationToken ct = default);

        Task<double> GetBalanceAsync();
        Task<SmsIrResponse> QueryStatusAsync(long providerMessageId);
    }
}
