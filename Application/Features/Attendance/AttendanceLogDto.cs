using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Attendance
{
    public class AttendanceLogDto
    {
        public int Id { get; set; }
        public int AttendanceDayId { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int WorkMinutes { get; set; }
        public string? Notes { get; set; }
    }
}
