using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities.Appointments
{
    [Table("DoctorVisitShift", Schema = "Appointment")]
    public class DoctorVisitShift : BaseEntity<int>
    {
        public DateOnly VisitDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int SlotDurationMin { get; set; } = 15;
        public bool AllowMinuteZero { get; set; } = true;
        /* Navigation */
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
