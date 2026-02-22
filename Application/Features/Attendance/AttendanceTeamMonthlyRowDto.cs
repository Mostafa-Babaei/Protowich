namespace Application.Features.Attendance
{
    public class AttendanceTeamMonthlyRowDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public int TotalWorkMinutes { get; set; }
        public int TotalLateMinutes { get; set; }
        public int TotalOvertimeMinutes { get; set; }
        public int PresentDays { get; set; }

    }
}
