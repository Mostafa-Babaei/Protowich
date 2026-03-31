using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    [Table("SystemSetting", Schema = "Core")]
    public class SystemSetting : BaseEntity<int>
    {
        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Key { get; set; } = string.Empty;

        [MaxLength(3000)]
        public string? Value { get; set; }

        [MaxLength(30)]
        public string? ValueType { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        public int DisplayOrder { get; set; }
    }
}

