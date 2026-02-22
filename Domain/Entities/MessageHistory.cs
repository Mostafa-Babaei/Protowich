using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    // message_history
    [Table("MessageHistory", Schema = "Notification")]
    public class MessageHistory : BaseEntity<long>
    {

        // enum('sms','email','push') => string
        [Required]

        public NotificationChannel Channel { get; set; } = NotificationChannel.SMS;

        [Required]
        [MaxLength(20)]
        public string Recipient { get; set; } = string.Empty;

        // text
        [Required]
        public string Content { get; set; } = string.Empty;

        public int? TemplateId { get; set; }

        // enum('pending','sent','failed','delivered','undelivered')
        [Required]

        public MessageStatusEnum Status { get; set; } = MessageStatusEnum.PENDING;

        [Required]
        [MaxLength(50)]
        public string ProviderName { get; set; } = "sms.ir";

        public long? ProviderMessageId { get; set; }

        public int? ProviderStatus { get; set; }

        [MaxLength(255)]
        public string? ProviderStatusText { get; set; }

        public int? DeliveryStatus { get; set; }

        [MaxLength(255)]
        public string? DeliveryStatusText { get; set; }

        public decimal? Cost { get; set; }

        public DateTime? SendRequestedAt { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public long? ActivityLogId { get; set; }


        // اگر بخوای می‌تونی این‌جا navigation به UserActivityLog اضافه کنی
        // public virtual UserActivityLog? ActivityLog { get; set; }
    }
}
