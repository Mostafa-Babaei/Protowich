namespace Application.Features.Attendance
{
    public class AttendanceDayReportDto
    {
        public DateTime Date { get; set; }
        public decimal TotalWorkHours { get; set; }
        public int TotalLateMinutes { get; set; }
        public int TotalOvertimeMinutes { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
