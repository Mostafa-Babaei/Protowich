using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Features.Visitors
{
    public class UpdateVisitRequest
    {
        public long Id { get; set; }
        public DateTime VisitDate { get; set; }
        public bool IsPlanned { get; set; }
        public string? Description { get; set; }
        public VisitStatusEnum Status { get; set; }
    }
    public class ChangeVisitStatusRequest
    {
        public long Id { get; set; }
        public VisitStatusEnum Status { get; set; }
        public string Description{ get; set; }
    }
    public class VisitListItemDto
    {
        public long Id { get; set; }
        public DateTime VisitDate { get; set; }
        public bool IsPlanned { get; set; }
        public VisitStatusEnum Status { get; set; }
        public string? Description { get; set; }
    }
    public class VisitStatsDto
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public int Upcoming { get; set; }     // آینده (IsPlanned یا VisitDate>Today)
    }
    public class TimelineItemDto
    {
        public string Type { get; set; } = "visit"; // فعلاً فقط visit، اگر بعداً followup اضافه کردی
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsPlanned { get; set; }
        public VisitStatusEnum Status { get; set; }
        public string? Description { get; set; }
    }

}
