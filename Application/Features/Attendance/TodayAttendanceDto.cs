using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Attendance
{
    public class TodayAttendanceDto
    {
        public DateTime Date { get; set; }
        public bool HasOpenLog { get; set; }
        public DateTime? LastCheckIn { get; set; }
        public DateTime? LastCheckOut { get; set; }
        public decimal TotalWorkHours { get; set; }
        public int TotalLateMinutes { get; set; }
        public int TotalOvertimeMinutes { get; set; }
        public AttendanceStatus Status { get; set; }
    };
}
