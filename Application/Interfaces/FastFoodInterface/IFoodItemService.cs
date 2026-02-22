using Application.Common;
using Application.Common.Models;
using Application.Features.FastFood.Dtos;
using Domain.Entities.FastFood;

namespace Application.Interfaces.FastFoodInterface
{
    public interface IFoodItemService : IGenericRepository<FoodItem>
    {
        Task<ApiResult<PagedResult<FoodItemListItemDto>>> GetPagedItemsAsync(
            int page, int pageSize, int? categoryId = null, string? keyword = null, CancellationToken ct = default);

        Task<ApiResult<int>> CreateItemAsync(FoodItemUpsertDto dto, CancellationToken ct = default);
        Task<ApiResult<string>> UpdateItemAsync(int id, FoodItemUpsertDto dto, CancellationToken ct = default);
        Task<ApiResult<string>> DeleteItemAsync(int id, CancellationToken ct = default);

        Task<ApiResult<string>> SetAvailabilityAsync(int id, bool isAvailable, CancellationToken ct = default);
    }

}
