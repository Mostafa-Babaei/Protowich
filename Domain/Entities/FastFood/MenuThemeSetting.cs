using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities.FastFood
{
    [Table("MenuThemeSetting", Schema = "Restaurant")]
    public class MenuThemeSetting : BaseEntity<int>
    {
        [Required]
        [MaxLength(50)]
        public string ActiveThemeKey { get; set; } = string.Empty;
    }
}

