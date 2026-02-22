using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Features.Visitors
{

    public class CreateVisitRequest
    {
        public long VisitorId { get; set; }
        public DateTime VisitDate { get; set; } // میلادی
        public VisitStatusEnum TreatmentStatus { get; set; } = VisitStatusEnum.InTreatment;
        public string? Description { get; set; }
        public bool IsPlanned { get; set; } = false;
    }

    public class CreateFollowupRequest
    {
        public long VisitorId { get; set; }
        public DateTime FollowupDate { get; set; } // میلادی
        public string Status { get; set; } = "scheduled";
    }

    public class DeleteVisitRequest
    {
        public long VisitId { get; set; }
    }

    public class CancelFollowupRequest
    {
        public long FollowupId { get; set; }
    }
}
