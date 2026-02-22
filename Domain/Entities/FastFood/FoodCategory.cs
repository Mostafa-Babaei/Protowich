using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities.FastFood
{
    [Table("FoodCategory", Schema = "Restaurant")]
    public class FoodCategory : BaseEntity<int>
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        public int DisplayOrder { get; set; }


        // Navigation
        public ICollection<MenuItem> Items { get; set; } = new List<MenuItem>();
    }
}
