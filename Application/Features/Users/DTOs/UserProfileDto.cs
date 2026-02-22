using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.DTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
        public string? PhoneNumber{ get; set; }
        public Guid? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? Gender { get; set; }
        public string? GenderName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
