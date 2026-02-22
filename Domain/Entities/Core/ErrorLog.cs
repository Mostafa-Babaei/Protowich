using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Logging
{
    [Table("ErrorLog", Schema = "Core")]
    public class ErrorLog : BaseEntity<long>
    {
        public string? Path { get; set; }
        public string? Method { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? Source { get; set; }
        public string? UserAgent { get; set; }
        public string? UserIp { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.Now;
    }
}
