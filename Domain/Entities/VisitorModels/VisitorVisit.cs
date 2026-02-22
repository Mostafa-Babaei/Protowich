using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities.VisitorModels
{
    [Table("VisitorVisits", Schema = "Visitor")]
    public class VisitorVisit : BaseEntity<long>
    {

        public long VisitorId { get; set; }

        // visit_date datetime NOT NULL
        public DateTime VisitDate { get; set; }
        public string? Description { get; set; }

        // ===== وضعیت مراجعه =====
        public VisitStatusEnum Status { get; set; } = VisitStatusEnum.InTreatment;

        // برای کنترل “مراجعه آینده”
        public bool IsPlanned { get; set; } = false;     // false: مراجعه انجام شده/واقعی، true: رزرو/پلن آینده


        public virtual Visitor Visitor { get; set; } = default!;
        public virtual User? Creator { get; set; }
    }
}
