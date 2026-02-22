using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Visitors
{
    public class VisitorLinkDto
    {
        public long Id { get; set; }
        public long VisitorId { get; set; }
        public long? LinkTypeId { get; set; }
        public string? LinkTypeTitle { get; set; }
        public string? Title { get; set; }

        public bool SendSms { get; set; }
        public DateTime? SendSmsOn { get; set; }
        public string SendSmsOnPershion => PersianDateHelper.ToPersianDateTime(SendSmsOn);
        public string Url { get; set; } = "";
        public DateTime? CreatedAt { get; set; }
        public string CreatedAtPershion => PersianDateHelper.ToPersianDateTime(CreatedAt);
    }
}
