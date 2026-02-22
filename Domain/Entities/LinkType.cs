using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entities.VisitorModels;

namespace Domain.Entities
{
    [Table("LinkType", Schema = "Core")]
    public class LinkType : BaseEntity<int>
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

    }
}
