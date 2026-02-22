using Microsoft.AspNetCore.Http;

namespace Application.Features.Visitors
{
    public class VisitorLinkUpsertFormDto
    {
        public long? Id { get; set; }          // برای edit (اختیاری)
        public long VisitorId { get; set; }
        public int? LinkTypeId { get; set; }
        public string? Title { get; set; }

        // اگر فایل نبود => Url باید پر باشد
        public string? Url { get; set; }

        // اگر فایل بود => Url اهمیتی ندارد
        public IFormFile? File { get; set; }
    }

}
