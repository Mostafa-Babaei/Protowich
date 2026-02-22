using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
namespace Domain.Entities
{
    [Table("AttendanceDay", Schema = "HR")]
    public class AttendanceDay : BaseEntity<int>
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }

        public int TotalWorkMinutes { get; set; }
        public int TotalLateMinutes { get; set; }
        public int TotalOvertimeMinutes { get; set; }

        public AttendanceStatus Status { get; set; }

        public ICollection<AttendanceLog> Logs { get; set; } = new List<AttendanceLog>();
    }
}
