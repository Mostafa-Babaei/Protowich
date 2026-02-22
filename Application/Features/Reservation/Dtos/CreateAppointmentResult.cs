using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reservation.Dtos
{
    public class CreateAppointmentResult
    {
        public int AppointmentId { get; set; }
        public string Message { get; set; } = "";
    }
}
