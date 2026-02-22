using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public abstract class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }

}
