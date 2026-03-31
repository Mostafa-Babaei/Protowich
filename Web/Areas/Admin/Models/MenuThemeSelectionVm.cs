using Application.Features.FastFood.Dtos;

namespace Web.Areas.Admin.Models
{
    public class MenuThemeSelectionVm
    {
        public string ActiveThemeKey { get; set; } = "classic";
        public List<MenuThemeOptionDto> Options { get; set; } = new();
    }
}

