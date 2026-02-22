using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Attendance
{
    public class MonthlyAttendanceDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<MonthlyAttendanceDayDto> Days { get; set; } = new();
    }
    public class MonthlyAttendanceDayDto
    {
        public DateTime Date { get; set; }
        public string PersianDate => PersianDateHelper.ToPersianDate(Date);

        public int TotalWorkMinutes { get; set; }
        public string TotalWorkTime => FormatMinutes(TotalWorkMinutes);
        public int TotalLateMinutes { get; set; }
        public int TotalOvertimeMinutes { get; set; }
        public AttendanceStatus Status { get; set; }
        public List<MonthlyAttendanceLogDto> Logs { get; set; } = new();
        private static string FormatMinutes(int minutes)
        {
            if (minutes < 0) minutes = 0;
            var h = minutes / 60;
            var m = minutes % 60;
            return $"{h:00}:{m:00}";
        }
    }
    public class MonthlyAttendanceLogDto
    {
        public int Id { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? WorkMinutes { get; set; }
        public string? IpAddress { get; set; }
    }
}
