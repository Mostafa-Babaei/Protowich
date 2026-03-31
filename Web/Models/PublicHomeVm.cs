using Application.Features.FastFood.Dtos;

namespace Web.Models
{
    public class PublicHomeVm
    {
        public string? Error { get; set; }
        public int CategoriesCount { get; set; }
        public int ItemsCount { get; set; }
        public int ImagesCount { get; set; }

        public List<PublicMenuCategoryDto> Categories { get; set; } = new();
        public List<PublicMenuFoodItemDto> FeaturedItems { get; set; } = new();
        public Dictionary<string, string> LandingSettings { get; set; } = new();
    }
}
