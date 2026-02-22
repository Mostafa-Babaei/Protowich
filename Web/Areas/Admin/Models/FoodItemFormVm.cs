
using Application.Features.FastFood.Dtos;
namespace Web.Areas.Admin.Models
{
    public class FoodItemFormVm
    {
        public int? Id { get; set; }
        public FoodItemUpsertDto Dto { get; set; } = new();

        public List<FoodCategoryListItemDto> Categories { get; set; } = new();

        // مدیریت تصاویر داخل صفحه Edit
        public List<FoodImageListItemDto> Images { get; set; } = new();
    }
}
