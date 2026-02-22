using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SMS.Dtos
{
    public class SmsIrVerifyRequest
    {
        public string Mobile { get; set; } = "";
        public int TemplateId { get; set; }
        public List<SmsIrParameter> Parameters { get; set; } = new();
    }
}
