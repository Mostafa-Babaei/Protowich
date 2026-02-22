namespace Application.Features.Reservation.Dtos
{
    public class CreateVisitShiftsBulkRequest
    {
        // بازه تاریخ
        public string FromDate { get; set; }   // از فرم میاد
        public string  ToDate { get; set; }     // از فرم میاد

        // انتخاب روزها (Saturday..Friday)
        public List<DayOfWeek> DaysOfWeek { get; set; } = new();

        // ساعت‌ها - از فرم به صورت string یا TimeSpan
        public string StartTime { get; set; } = "09:00"; // "HH:mm"
        public string EndTime { get; set; } = "13:00";   // "HH:mm"

        public int SlotDurationMin { get; set; } = 15;
        public bool AllowMinuteZero { get; set; } = true;

        // اگر پزشک/کلینیک چندتا دکتر دارند بهتره DoctorId هم داشته باشه
        public int DoctorId { get; set; }
    }

}
