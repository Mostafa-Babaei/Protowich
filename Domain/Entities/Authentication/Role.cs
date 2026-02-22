using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entities.Authentication;

namespace Domain.Entities
{
    [Table("Role", Schema = "Authentication")]
    public class Role : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string? Description { get; set; }

        // نقش می‌تواند چند مجوز داشته باشد
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

        // 
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // 🔗 ارتباط با منوها
        public ICollection<MenuRole> MenuRoles { get; set; } = new List<MenuRole>();
    }
}
