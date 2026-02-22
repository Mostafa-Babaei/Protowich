using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SMS.Dtos
{
    public class SmsIrOptions
    {
        public string BaseUrl { get; set; } = "";
        public string ApiKey { get; set; } = "";
        public string ProviderName { get; set; } = "sms.ir";
        public string LineNumber { get; set; } = "";
        public int TimeoutSeconds { get; set; } = 30;
    }
}
