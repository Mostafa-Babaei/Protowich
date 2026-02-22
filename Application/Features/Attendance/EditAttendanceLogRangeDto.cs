using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Attendance
{
    public class EditAttendanceLogRangeDto
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string? Reason { get; set; }
    }
}
