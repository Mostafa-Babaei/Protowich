using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reservation.Dtos
{

    public class CreateAppointmentRequest
    {
        public int VisitShiftId { get; set; }
        public string VisitTime { get; set; } = ""; // "HH:mm"
        public string FullName { get; set; } = "";
        public string Mobile { get; set; } = "";

        public string? NationalCode { get; set; }                 // ✅ optional
        public AppointmentType? AppointmentType { get; set; }     // ✅ optional
    }
}
