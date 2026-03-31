using Application.Common.Models;
using Application.Features.FastFood.Dtos;

namespace Application.Interfaces.FastFoodInterface
{
    public interface ISystemSettingService
    {
        Task<ApiResult<List<SystemSettingItemDto>>> GetByCategoryAsync(string category, CancellationToken ct = default);
        Task<ApiResult<string>> UpdateAsync(int id, SystemSettingUpdateDto dto, CancellationToken ct = default);
    }
}

