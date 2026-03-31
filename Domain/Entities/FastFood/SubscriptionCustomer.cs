using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities.FastFood
{
    [Table("SubscriptionCustomer", Schema = "Restaurant")]
    public class SubscriptionCustomer : BaseEntity<int>
    {
        [Required]
        [MaxLength(80)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(80)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Mobile { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(40)]
        public string SubscriptionCode { get; set; } = string.Empty;
    }
}
