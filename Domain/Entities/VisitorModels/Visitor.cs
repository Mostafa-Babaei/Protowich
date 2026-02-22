using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities.VisitorModels
{
    [Table("Visitors", Schema = "Visitor")]
    public class Visitor : BaseEntity<long>
    {

        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Mobile { get; set; }

        [MaxLength(20)]
        public string? NationalId { get; set; }
        // NOT NULL
        public DateTime VisitDate { get; set; }
        // mediumtext
        public string? Description { get; set; }

        [MaxLength(20)]
        public TreatmentStatus? TreatmentStatus { get; set; }
        public DateTime? TreatmentStatusChangedAt { get; set; }
        [MaxLength(20)]
        public string? Mobile2 { get; set; }
        [MaxLength(50)]
        public string? DocNumber { get; set; }
        public virtual User? Creator { get; set; }
        public virtual ICollection<VisitorLink> Links { get; set; } = new List<VisitorLink>();
        //public virtual ICollection<VisitorLog> Logs { get; set; } = new List<VisitorLog>();
        public virtual ICollection<VisitorVisit> Visits { get; set; } = new List<VisitorVisit>();
    }
}
