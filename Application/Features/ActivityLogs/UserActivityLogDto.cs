namespace Application.Features
{
    public class UserActivityLogDto
    {
        public long Id { get; set; }
        public Guid? UserId { get; set; }
        public string UserIdName { get; set; } = "";
        public string Action { get; set; } = "";
        public string? Description { get; set; }
        public string? TargetType { get; set; }
        public string? TargetId { get; set; }   
        public string? TargetIdName { get; set; }   
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedAtPersian => PersianDateHelper.ToPersianDate(CreatedAt);
    }
}
