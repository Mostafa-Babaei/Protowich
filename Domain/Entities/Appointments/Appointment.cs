using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities.Appointments
{
    [Table("Appointment", Schema = "Appointment")]
    public class Appointment : BaseEntity<int>
    {
        public int VisitShiftId { get; set; }
        public DateOnly VisitDate { get; set; }
        public TimeOnly VisitTime { get; set; }
        public int DurationMin { get; set; }
        [MaxLength(255)]
        public string FullName { get; set; } = string.Empty;
        [MaxLength(20)]
        public AppointmentType AppointmentType { get; set; } = AppointmentType.FirstVisit;
        public string Mobile { get; set; } = string.Empty;

        [MaxLength(10)]
        public string NationalCode { get; set; } = string.Empty;
        public CreatedByRole CreatedByRole { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public bool SmsSent { get; set; } = false;
        /* Navigation */
        public virtual DoctorVisitShift VisitShift { get; set; } = null!;
    }
}
