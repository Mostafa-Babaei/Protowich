using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FastFood.Dtos
{
    public class FoodItemUpsertDto
    {
        public int FoodCategoryId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;
        public int DisplayOrder { get; set; }
    }
}
