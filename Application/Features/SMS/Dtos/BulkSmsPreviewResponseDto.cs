using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SMS.Dtos
{
    public class BulkSmsPreviewResponseDto
    {
        public int Total { get; set; }
        public string ConfirmToken { get; set; } = string.Empty;
    }
}
