using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entities.Authentication;

namespace Domain.Entities
{
    [Table("Permission", Schema = "Authentication")]
    public class Permission : BaseEntity<int>
    {
        public string Code { get; set; } = null!;      
        public string Title { get; set; } = null!;       
        public int CategoryId { get; set; }
        public PermissionCategory Category { get; set; } = null!;
    }
}
