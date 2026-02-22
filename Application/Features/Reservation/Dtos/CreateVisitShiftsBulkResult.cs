namespace Application.Features.Reservation.Dtos
{
    public class CreateVisitShiftsBulkResult
    {
        public int CreatedCount { get; set; }
        public int SkippedCount { get; set; } // تکراری/همپوشان/نامعتبر
        public List<DateOnly> CreatedDates { get; set; } = new();
        public List<DateOnly> SkippedDates { get; set; } = new();
    }
}
