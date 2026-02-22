using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities.FastFood
{
    [Table("FoodItem", Schema = "Restaurant")]
    public class FoodItem : BaseEntity<int>
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        public int DisplayOrder { get; set; }

        // FK
        public int FoodCategoryId { get; set; }
        public virtual FoodCategory FoodCategory { get; set; }

        // Navigation
        public virtual ICollection<FoodImage> Images { get; set; }
            = new List<FoodImage>();
    }

}
