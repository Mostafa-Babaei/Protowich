using Application.Common;
using Application.Common.Models;
using Application.Features.FastFood.Dtos;
using Domain.Entities.FastFood;

namespace Application.Interfaces.FastFoodInterface
{
    public interface IFoodCategoryService : IGenericRepository<FoodCategory>
    {
        Task<ApiResult<PagedResult<FoodCategoryListItemDto>>> GetPagedCategoriesAsync(
            int page, int pageSize, string? keyword = null, CancellationToken ct = default);

        Task<ApiResult<int>> CreateCategoryAsync(FoodCategoryUpsertDto dto, CancellationToken ct = default);
        Task<ApiResult<string>> UpdateCategoryAsync(int id, FoodCategoryUpsertDto dto, CancellationToken ct = default);
        Task<ApiResult<string>> DeleteCategoryAsync(int id, CancellationToken ct = default);
        Task<ApiResult<string>> SetCategoryActiveAsync(int id, bool isActive, CancellationToken ct = default);
    }

}
