using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SMS.Dtos
{
    public class SmsIrData
    {
        public List<long>? MessageIds { get; set; }
        public decimal? Cost { get; set; }
    }
}
