using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities.FastFood
{
    [Table("FoodImage", Schema = "Restaurant")]
    public class FoodImage : BaseEntity<int>
    {
        [Required]
        [MaxLength(300)]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsMain { get; set; } = false;

        public int DisplayOrder { get; set; }

        // FK
        public int FoodItemId { get; set; }
        public virtual FoodItem FoodItem { get; set; }
    }

}
