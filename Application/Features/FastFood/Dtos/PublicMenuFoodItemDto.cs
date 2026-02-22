using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FastFood.Dtos
{
    public class PublicMenuFoodItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DisplayOrder { get; set; }
        public string? MainImageUrl { get; set; }
        public List<string> ImageUrls { get; set; } = new();
    }
}
