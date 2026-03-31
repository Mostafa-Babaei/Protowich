using Application.Common.Models;
using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly AppDbContext _db;

        public SystemSettingService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ApiResult<List<SystemSettingItemDto>>> GetByCategoryAsync(string category, CancellationToken ct = default)
        {
            try
            {
                category = (category ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(category))
                    return ApiResult<List<SystemSettingItemDto>>.Error("دسته‌بندی تنظیمات نامعتبر است.");

                var items = await _db.Set<SystemSetting>()
                    .AsNoTracking()
                    .Where(x => x.Category == category)
                    .OrderBy(x => x.DisplayOrder)
                    .ThenBy(x => x.Id)
                    .Select(x => new SystemSettingItemDto
                    {
                        Id = x.Id,
                        Category = x.Category,
                        Key = x.Key,
                        Value = x.Value,
                        ValueType = x.ValueType,
                        Description = x.Description,
                        DisplayOrder = x.DisplayOrder,
                        IsActive = x.IsActive
                    })
                    .ToListAsync(ct);

                return ApiResult<List<SystemSettingItemDto>>.Success(items);
            }
            catch (Exception ex)
            {
                return ApiResult<List<SystemSettingItemDto>>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<string>> UpdateAsync(int id, SystemSettingUpdateDto dto, CancellationToken ct = default)
        {
            try
            {
                if (id <= 0)
                    return ApiResult<string>.Error("شناسه تنظیم نامعتبر است.");

                dto ??= new SystemSettingUpdateDto();

                var entity = await _db.Set<SystemSetting>().FirstOrDefaultAsync(x => x.Id == id, ct);
                if (entity == null)
                    return ApiResult<string>.Error("تنظیم موردنظر یافت نشد.");

                entity.Value = dto.Value?.Trim();
                entity.IsActive = dto.IsActive;
                entity.UpdatedAt = DateTime.Now;

                await _db.SaveChangesAsync(ct);
                return ApiResult<string>.Success("تنظیم با موفقیت بروزرسانی شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }
    }
}

