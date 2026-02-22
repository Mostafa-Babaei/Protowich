using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.DTOs
{
    public class RoleForEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public List<int> PermissionIds { get; set; } = new();

        public List<PermissionEditItemDto> Permissions { get; set; } = new();
    }
}
