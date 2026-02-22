using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reservation.Dtos
{
    public class MoveAppointmentRequest
    {
        public int NewVisitShiftId { get; set; }
        public string NewVisitTime { get; set; } = ""; // "HH:mm"
        public string? Reason { get; set; } // اختیاری
    }
}
