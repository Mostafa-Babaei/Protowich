using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities.Authentication
{
    [Table("MenuRole", Schema = "Authentication")]
    public class MenuRole : BaseEntity<long>
    {
        public int RoleId { get; set; }
        public int MenuItemId { get; set; }

        // Navigation
        public Role Role { get; set; } = null!;
        public MenuItem MenuItem { get; set; } = null!;
    }
}
