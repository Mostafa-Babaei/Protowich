using Application.Common.Models;
using Application.Features.FastFood.Dtos;

namespace Application.Interfaces.FastFoodInterface
{
    public interface IMenuThemeService
    {
        Task<string> GetActiveThemeKeyAsync(CancellationToken ct = default);
        Task<IReadOnlyList<MenuThemeOptionDto>> GetThemeOptionsAsync(CancellationToken ct = default);
        Task<ApiResult<string>> SetActiveThemeAsync(string themeKey, CancellationToken ct = default);
    }
}

