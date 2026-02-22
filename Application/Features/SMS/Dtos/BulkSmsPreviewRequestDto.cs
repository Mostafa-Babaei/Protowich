using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SMS.Dtos
{
    public class BulkSmsPreviewRequestDto
    {
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// لیست موبایل‌های مراجعین (از فرانت)
        /// </summary>
        public List<string> Mobiles { get; set; } = new();
    }
}
