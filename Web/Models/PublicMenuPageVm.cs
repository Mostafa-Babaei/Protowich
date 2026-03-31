using Application.Features.FastFood.Dtos;

namespace Web.Models
{
    public class PublicMenuPageVm
    {
        public string? Error { get; set; }
        public string ActiveThemeKey { get; set; } = "classic";
        public IReadOnlyList<PublicMenuCategoryDto> Categories { get; set; } = Array.Empty<PublicMenuCategoryDto>();
    }
}

