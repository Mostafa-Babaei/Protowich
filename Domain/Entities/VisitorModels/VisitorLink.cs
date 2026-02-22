using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities.VisitorModels
{
    // visitor_links
    [Table("VisitorLinks", Schema = "Visitor")]
    public class VisitorLink : BaseEntity<long>
    {
        public long VisitorId { get; set; }
        public string LinkCode { get; set; }
        public string ObjectKey { get; set; }
        public int? LinkTypeId { get; set; }
        [MaxLength(100)]
        public string? Title { get; set; }

        public bool SendSms { get; set; }
        public DateTime? SendSmsOn { get; set; }

        public string Url { get; set; } = string.Empty;
        public virtual Visitor Visitor { get; set; } = default!;
        public virtual LinkType? LinkType { get; set; }
    }

}
