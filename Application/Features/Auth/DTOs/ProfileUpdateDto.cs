using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.DTOs
{
    public class ProfileUpdateDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; } = null;
    }

}
