using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reservation.Dtos
{
    public class ShiftSlotsResult
    {
        public int ShiftId { get; set; }
        public string VisitDatePersian { get; set; } = "";
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
        public int SlotDurationMin { get; set; }
        public bool IsActive { get; set; }
        public List<ShiftSlotDto> Slots { get; set; } = new();
    }
}
