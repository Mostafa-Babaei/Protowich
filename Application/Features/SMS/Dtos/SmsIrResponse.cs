using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SMS.Dtos
{
    public class SmsIrResponse
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public SmsIrData? Data { get; set; }
    }
}
