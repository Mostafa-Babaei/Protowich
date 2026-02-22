using Application.Common;
using Application.Common.Models;
using Application.Features.FastFood.Dtos;
using Domain.Entities.FastFood;

namespace Application.Interfaces.FastFoodInterface
{
    public interface IFoodImageService : IGenericRepository<FoodImage>
    {
        Task<ApiResult<List<FoodImageListItemDto>>> GetItemImagesAsync(int foodItemId, CancellationToken ct = default);

        Task<ApiResult<int>> AddImageAsync(FoodImageUpsertDto dto, CancellationToken ct = default);
        Task<ApiResult<string>> DeleteImageAsync(int id, CancellationToken ct = default);

        Task<ApiResult<string>> SetMainImageAsync(int imageId, CancellationToken ct = default);
    }

}
