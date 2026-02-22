using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FastFood.Dtos
{
    public class FoodItemListItemDto
    {
        public int Id { get; set; }
        public int FoodCategoryId { get; set; }
        public string CategoryTitle { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int DisplayOrder { get; set; }

        public string? MainImageUrl { get; set; }
    }
}
