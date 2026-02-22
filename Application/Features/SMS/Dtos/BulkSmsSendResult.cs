using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SMS.Dtos
{
    public class BulkSmsSendResult
    {
        public int Total { get; set; }
        public int Sent { get; set; }
        public int Failed { get; set; }
    }
}
