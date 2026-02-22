using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reservation.Dtos
{
    public class ShiftSlotDto
    {
        public string Time { get; set; } = "";        // "09:00"
        public bool IsBooked { get; set; }
        public string? FullName { get; set; }
        public string? Mobile { get; set; }
        public int? AppointmentId { get; set; }
    }
}
