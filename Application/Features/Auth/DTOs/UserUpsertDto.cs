namespace Application.Features.Auth.DTOs
{
    // 📘 مدل ایجاد یا ویرایش کاربر
    public class UserUpsertDto
    {
        public Guid? Id { get; set; } 
        public string UserName { get; set; } 
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public bool IsActive { get; set; }
        public bool LoginWithSms { get; set; } = false;

        // نقش‌ها
        public List<int> RoleIds { get; set; } = new();
    }
}
