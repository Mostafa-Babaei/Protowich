using Application.Common.Models;
using Application.Features.FastFood.Dtos;

namespace Application.Interfaces.FastFoodInterface
{
    public interface IMenuPublicService
    {
        Task<ApiResult<List<PublicMenuCategoryDto>>> GetPublicMenuAsync(CancellationToken ct = default);
    }

}
