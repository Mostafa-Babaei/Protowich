using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    [Table("Settings", Schema = "Authentication")]
    public class Setting : BaseEntity<int>
    {
        [Required]
        [MaxLength(150)]
        public string SettingKey { get; set; } = string.Empty;

        // text
        public string? SettingValue { get; set; }
    }
}
