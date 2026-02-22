using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Features.SMS.Dtos
{
    public class MessageHistoryListFilterDto
    {
        // paging
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 200)]
        public int PageSize { get; set; } = 20;

        // filters
        public NotificationChannel? Channel { get; set; }
        public MessageStatusEnum? Status { get; set; }

        /// <summary>شماره/ایمیل گیرنده (contains)</summary>
        public string? Recipient { get; set; }

        /// <summary>جستجو در محتوا یا گیرنده یا ProviderStatusText</summary>
        public string? Keyword { get; set; }

        public int? TemplateId { get; set; }
        public string? ProviderName { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

}
