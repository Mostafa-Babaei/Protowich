namespace Application.Features.Attendance
{
    public class CheckInResponseDto
    {
        public int AttendanceDayId { get; set; }
        public int AttendanceLogId { get; set; }
        public DateTime CheckIn { get; set; }
        public TodayAttendanceDto Today { get; set; }

    }
}
