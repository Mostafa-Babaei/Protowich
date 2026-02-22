namespace Application.Common.Models
{
    public class CurrentUserDto
    {
        public Guid? UserId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? EmployeeId { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public bool ReceiveNotifications { get; set; }
    }
}
