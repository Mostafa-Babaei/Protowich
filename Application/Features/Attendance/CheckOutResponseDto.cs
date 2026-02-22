namespace Application.Features.Attendance
{
    public class CheckOutResponseDto
    {
        public int AttendanceDayId { get; set; }
        public int AttendanceLogId { get; set; }
        public DateTime CheckOut { get; set; }
        public int SessionWorkMinutes { get; set; }
        public TodayAttendanceDto Today { get; set; }

    }
}
