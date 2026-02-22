using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Visitors
{
    public class ImportVisitorsResultDto
    {
        public bool Success { get; set; }
        public int Inserted { get; set; }
        public int Duplicates { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
