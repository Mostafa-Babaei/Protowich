using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Features.Reservation.Dtos
{
    public class DoctorVisitShiftListItemDto
    {
        public int Id { get; set; }
        public DateOnly VisitDate { get; set; }
        public DateTime VisitDateTime => VisitDate.ToDateTime(TimeOnly.MinValue);
        public string visitDatePersian => PersianDateHelper.ToPersianDate(VisitDateTime);
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int SlotDurationMin { get; set; }
        public bool AllowMinuteZero { get; set; }
        public bool IsActive { get; set; }

        // برای UI
        public int AppointmentsCount { get; set; }
    }
}
