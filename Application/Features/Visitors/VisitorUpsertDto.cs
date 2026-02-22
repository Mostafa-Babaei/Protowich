using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Visitors
{
    public class VisitorUpsertDto
    {
        public long? Id { get; set; } // null => create
        public string FullName { get; set; } = "";
        public string? Mobile { get; set; }
        public string? Mobile2 { get; set; }
        public string? NationalId { get; set; }
        public string? DocNumber { get; set; } // اگر میخوای اجباری باشه، کنترلش کن
    }
}
