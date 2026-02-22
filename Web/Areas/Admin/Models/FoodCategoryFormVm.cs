
using Application.Features.FastFood.Dtos;
namespace Web.Areas.Admin.Models
{
    public class FoodCategoryFormVm
    {
        public int? Id { get; set; }
        public FoodCategoryUpsertDto Dto { get; set; } = new();
    }
}
