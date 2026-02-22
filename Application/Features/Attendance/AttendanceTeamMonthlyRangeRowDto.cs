using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Attendance
{
    public class AttendanceTeamMonthlyRangeRowDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = "";

        public int Year { get; set; }   // میلادی
        public int Month { get; set; }  // 1..12

        public int TotalWorkMinutes { get; set; }
        public int PresentDays { get; set; }
    }

}
