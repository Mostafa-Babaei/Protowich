

using Application.Features.FastFood.Dtos;

namespace Web.Areas.Admin.Models
{
    public class FoodImageFormVm
    {
        public int FoodItemId { get; set; }
        public FoodImageUpsertDto Dto { get; set; } = new();
    }

}
