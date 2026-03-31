using Application.Common.Models;
using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Domain.Entities.FastFood;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MenuThemeService : IMenuThemeService
    {
        public const string ClassicThemeKey = "classic";
        public const string Modern1ThemeKey = "modern1";

        private readonly AppDbContext _db;

        public MenuThemeService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<string> GetActiveThemeKeyAsync(CancellationToken ct = default)
        {
            var setting = await _db.Set<MenuThemeSetting>()
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(ct);

            if (setting == null || string.IsNullOrWhiteSpace(setting.ActiveThemeKey))
                return ClassicThemeKey;

            var key = setting.ActiveThemeKey.Trim().ToLowerInvariant();
            return IsValidThemeKey(key) ? key : ClassicThemeKey;
        }

        public Task<IReadOnlyList<MenuThemeOptionDto>> GetThemeOptionsAsync(CancellationToken ct = default)
        {
            IReadOnlyList<MenuThemeOptionDto> options = new List<MenuThemeOptionDto>
            {
                new MenuThemeOptionDto
                {
                    Key = ClassicThemeKey,
                    Title = "کلاسیک",
                    Description = "نمای فعلی منو با کارت‌های ساده"
                },
                new MenuThemeOptionDto
                {
                    Key = Modern1ThemeKey,
                    Title = "قالب 1",
                    Description = "قالب جدید (menu1) با تب‌های چسبان و کارت افقی"
                }
            };

            return Task.FromResult(options);
        }

        public async Task<ApiResult<string>> SetActiveThemeAsync(string themeKey, CancellationToken ct = default)
        {
            try
            {
                var key = (themeKey ?? string.Empty).Trim().ToLowerInvariant();
                if (!IsValidThemeKey(key))
                    return ApiResult<string>.Error("قالب انتخاب‌شده معتبر نیست.");

                var setting = await _db.Set<MenuThemeSetting>().OrderByDescending(x => x.Id).FirstOrDefaultAsync(ct);
                if (setting == null)
                {
                    setting = new MenuThemeSetting
                    {
                        ActiveThemeKey = key,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    };
                    _db.Set<MenuThemeSetting>().Add(setting);
                }
                else
                {
                    setting.ActiveThemeKey = key;
                    setting.UpdatedAt = DateTime.Now;
                    if (!setting.IsActive)
                        setting.IsActive = true;
                }

                await _db.SaveChangesAsync(ct);
                return ApiResult<string>.Success("قالب منو با موفقیت تغییر کرد.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        private static bool IsValidThemeKey(string key)
            => key == ClassicThemeKey || key == Modern1ThemeKey;
    }
}
