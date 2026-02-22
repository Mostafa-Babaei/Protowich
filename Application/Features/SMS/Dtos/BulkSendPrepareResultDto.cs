using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SMS.Dtos
{
    public class BulkSendPrepareResultDto
    {
        public Guid OperationId { get; set; }
        public int RecipientsCount { get; set; }
        public string Preview { get; set; } = string.Empty; // مثلا 60 کاراکتر اول
        public DateTime ExpiresAtUtc { get; set; }
    }
}
