using Domain.Enums;

namespace Application.Features.SMS.Dtos
{
    public class MessageHistoryListItemDto
    {
        public long Id { get; set; }

        public NotificationChannel Channel { get; set; }
        public string Recipient { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public int? TemplateId { get; set; }

        public MessageStatusEnum Status { get; set; }
        public string ProviderName { get; set; } = string.Empty;

        public long? ProviderMessageId { get; set; }
        public int? ProviderStatus { get; set; }
        public string? ProviderStatusText { get; set; }

        public int? DeliveryStatus { get; set; }
        public string? DeliveryStatusText { get; set; }

        public decimal? Cost { get; set; }

        public DateTime? SendRequestedAt { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public long? ActivityLogId { get; set; }
    }
}
