using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entities.Authentication;

namespace Domain.Entities
{
    [Table("MenuItem", Schema = "Authentication")]
    public class MenuItem : BaseEntity<int>
    {
        public string Title { get; set; } = null!;
        public string? Icon { get; set; }
        public string? Route { get; set; }  // مسیر صفحه، مثل "/missions"
        public int? ParentId { get; set; }
        public string? Section { get; set; }
        public int DisplayOrder { get; set; }

        [ForeignKey("ParentId")]
        public MenuItem? Parent { get; set; }

        public ICollection<MenuItem> Children { get; set; } = new List<MenuItem>();

        // 🔗 ارتباط جدید
        public ICollection<MenuPermission> MenuPermissions { get; set; } = new List<MenuPermission>();

        // 🔗 ارتباط با نقش‌ها
        public ICollection<MenuRole> MenuRoles { get; set; } = new List<MenuRole>();
    }
}
