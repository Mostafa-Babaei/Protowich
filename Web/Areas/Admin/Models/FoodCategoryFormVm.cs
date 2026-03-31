
using Application.Features.FastFood.Dtos;
using Microsoft.AspNetCore.Http;
namespace Web.Areas.Admin.Models
{
    public class FoodCategoryFormVm
    {
        public int? Id { get; set; }
        public FoodCategoryUpsertDto Dto { get; set; } = new();
        public IFormFile? UploadFile { get; set; }
    }
}
