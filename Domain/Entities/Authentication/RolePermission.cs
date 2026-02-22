using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    [Table("RolePermission", Schema = "Authentication")]
    public class RolePermission : BaseEntity<long>
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public Role Role { get; set; } = null!;
        public Permission Permission { get; set; } = null!;
    }
}
