using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
namespace Domain.Entities
{
    [Table("AttendanceLog", Schema = "HR")]
    public class AttendanceLog : BaseEntity<int>
    {
        public int AttendanceDayId { get; set; }
        public AttendanceDay AttendanceDay { get; set; } = null!;

        public DateTime? CheckIn { get; set; }

        public DateTime? CheckOut { get; set; }

        //[Column(TypeName = "decimal(5,2)")]
        //public decimal? WorkHours { get; set; }

        public int WorkMinutes { get; set; } = 0;
        public int LateMinutes { get; set; } = 0;

        public int OvertimeMinutes { get; set; } = 0;

        public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;

        public string? Notes { get; set; }
        public string? UserIp { get; set; }

        [MaxLength(45)]
        public string? IpAddress { get; set; }

        public string? LocationData { get; set; }
    }
}
