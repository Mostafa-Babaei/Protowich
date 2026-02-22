using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reservation.Dtos
{
    public class CancelAppointmentRequest
    {
        public string? Reason { get; set; } // اختیاری
    }
}
