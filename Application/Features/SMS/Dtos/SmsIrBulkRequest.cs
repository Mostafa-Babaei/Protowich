using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SMS.Dtos
{
    public class SmsIrBulkRequest
    {
        public string LineNumber { get; set; } = "";
        public string MessageText { get; set; } = "";
        public List<string> Mobiles { get; set; } = new();
        public long? SendDateTime { get; set; } // unix timestamp
    }

}
