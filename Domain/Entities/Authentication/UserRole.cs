using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    [Table("UserRole", Schema = "Authentication")]
    public class UserRole : BaseEntity<int>
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }

        public User User { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}
