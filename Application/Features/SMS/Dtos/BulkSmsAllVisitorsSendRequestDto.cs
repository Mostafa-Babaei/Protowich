using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SMS.Dtos
{
    public class BulkSmsAllVisitorsSendRequestDto
    {
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// توکن گرفته شده از Preview
        /// </summary>
        public string ConfirmToken { get; set; } = string.Empty;
    }
}
