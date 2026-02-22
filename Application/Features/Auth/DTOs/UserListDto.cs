using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Features.Auth.DTOs
{
    public class UserListDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? Username { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Mobile { get; set; }
        public string? Phone { get; set; }
        public bool LoginWithSms { get; set; } = false;
        public string Email { get; set; } = null!;
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}
