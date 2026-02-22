using System.ComponentModel.DataAnnotations;

namespace Application.Features.Visitors
{

    public class VisitorLinkUpsertDto
    {
        public long? Id { get; set; } 
        public long VisitorId { get; set; }
        public int? LinkTypeId { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
    }
}
