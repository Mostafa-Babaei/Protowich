using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    [Table("User", Schema = "Authentication")]
    public class User : BaseEntity<Guid>
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ResetCode { get; set; }
        public string? Avatar { get; set; }
        public string? Mobile { get; set; }
        public string? Phone { get; set; }
        public int? CompanyId { get; set; }
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiresAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool LoginWithSms { get; set; } = false;
        public bool? IsSystemAdmin { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
