using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    [Table("UserActivityLogs", Schema = "Authentication")]
    public class UserActivityLog : BaseEntity<long>
    {

        public Guid? UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Action { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? TargetType { get; set; }

        public string? TargetId { get; set; }

        [MaxLength(45)]
        public string? IpAddress { get; set; }

        [MaxLength(255)]
        public string? UserAgent { get; set; }

        public virtual User? User { get; set; }
    }
}
