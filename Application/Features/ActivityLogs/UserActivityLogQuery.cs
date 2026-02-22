using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features
{
    public class UserActivityLogQuery
    {
        public Guid? UserId { get; set; }
        public string? Action { get; set; }
        public string? TargetType { get; set; }
        public string? TargetId { get; set; }
        public string? IpAddress { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public string? Keyword { get; set; } // سرچ روی Action/Description/UserAgent/TargetId
        public string? Sort { get; set; } = "-createdOn"; // createdOn | -createdOn | action | -action

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
