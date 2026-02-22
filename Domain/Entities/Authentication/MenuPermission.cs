using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    [Table("MenuPermission", Schema = "Authentication")]
    public class MenuPermission : BaseEntity<int>
    {
        public int MenuId { get; set; }
        public int PermissionId { get; set; }

        [ForeignKey("MenuId")]
        public MenuItem Menu { get; set; } = null!;

        [ForeignKey("PermissionId")]
        public Permission Permission { get; set; } = null!;
    }
}
