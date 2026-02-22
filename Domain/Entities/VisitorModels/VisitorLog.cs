using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities.VisitorModels
{
    [Table("VisitorLogs", Schema = "Visitor")]
    public class VisitorLog : BaseEntity<long>
    {

        public int VisitorId { get; set; }

        // enum('create','edit','delete') -> string
        [Required]
        [MaxLength(20)]
        public string Action { get; set; } = string.Empty;

        public string? Description { get; set; }

        public virtual Visitor Visitor { get; set; } = default!;
        public virtual User? Creator { get; set; }
    }
}
