using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reservation.Dtos
{
    public class ReservationActiveShiftDto
    {
        public int Id { get; set; }
        public DateOnly VisitDate { get; set; }
        public string VisitDatePersian { get; set; } = "";
        public string StartTime { get; set; } = ""; // "HH:mm"
        public string EndTime { get; set; } = "";   // "HH:mm"
        public int SlotDurationMin { get; set; }
        public int Capacity { get; set; }
        public int BookedCount { get; set; }
        public int FreeCount { get; set; }
    }
}
