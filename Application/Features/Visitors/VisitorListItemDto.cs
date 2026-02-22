using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Visitors
{
    public class VisitorListItemDto
    {
        public long Id { get; set; }
        public string FullName { get; set; } = "";
        public string? Mobile { get; set; }
        public string? Mobile2 { get; set; }
        public string? NationalId { get; set; }
        public string? DocNumber { get; set; }
        public int NumberOfLink { get; set; }
        public int NumberOfVisit { get; set; }
        public DateTime VisitDate { get; set; }
    }
}
