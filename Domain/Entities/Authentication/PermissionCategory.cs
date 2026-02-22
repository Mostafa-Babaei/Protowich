using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities.Authentication
{
    [Table("PermissionCategory", Schema = "Authentication")]
    public class PermissionCategory : BaseEntity<int>
    {
        public string Key { get; set; } = null!;       
        public string Title { get; set; } = null!;     
        public string Icon { get; set; } = null!;       

        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }

}
