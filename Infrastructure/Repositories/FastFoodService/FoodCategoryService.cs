using Application.Common.Models;
using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Domain.Entities.FastFood;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FoodCategoryService : GenericRepository<FoodCategory>, IFoodCategoryService
    {
        private readonly AppDbContext _db;

        public FoodCategoryService(AppDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<ApiResult<PagedResult<FoodCategoryListItemDto>>> GetPagedCategoriesAsync(
            int page, int pageSize, string? keyword = null, CancellationToken ct = default)
        {
            try
            {
                keyword = keyword?.Trim();

                var q = _db.FoodCategories.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(keyword))
                    q = q.Where(x => x.Title.Contains(keyword) || (x.Description != null && x.Description.Contains(keyword)));

                var total = await q.CountAsync(ct);

                var items = await q
                    .OrderBy(x => x.DisplayOrder).ThenBy(x => x.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new FoodCategoryListItemDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        IsActive = x.IsActive,
                        DisplayOrder = x.DisplayOrder,
                        ItemsCount = _db.FoodItems.Count(fi => fi.FoodCategoryId == x.Id)
                    })
                    .ToListAsync(ct);

                var result = new PagedResult<FoodCategoryListItemDto>
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total,
                    Items = items
                };

                return ApiResult<PagedResult<FoodCategoryListItemDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResult<PagedResult<FoodCategoryListItemDto>>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<int>> CreateCategoryAsync(FoodCategoryUpsertDto dto, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                    return ApiResult<int>.Error("عنوان دسته‌بندی الزامی است.");

                var entity = new FoodCategory
                {
                    Title = dto.Title.Trim(),
                    Description = dto.Description?.Trim(),
                    ImageUrl = dto.ImageUrl?.Trim(),
                    DisplayOrder = dto.DisplayOrder,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _db.FoodCategories.Add(entity);
                await _db.SaveChangesAsync(ct);

                return ApiResult<int>.Success(entity.Id, "ثبت دسته‌بندی انجام شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<int>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<string>> UpdateCategoryAsync(int id, FoodCategoryUpsertDto dto, CancellationToken ct = default)
        {
            try
            {
                if (id <= 0) return ApiResult<string>.Error("شناسه نامعتبر است.");
                if (string.IsNullOrWhiteSpace(dto.Title)) return ApiResult<string>.Error("عنوان دسته‌بندی الزامی است.");

                var entity = await _db.FoodCategories.FirstOrDefaultAsync(x => x.Id == id, ct);
                if (entity == null) return ApiResult<string>.Error("دسته‌بندی یافت نشد.");

                entity.Title = dto.Title.Trim();
                entity.Description = dto.Description?.Trim();
                entity.ImageUrl = dto.ImageUrl?.Trim();
                entity.DisplayOrder = dto.DisplayOrder;
                entity.IsActive = dto.IsActive;
                entity.UpdatedAt = DateTime.Now;

                await _db.SaveChangesAsync(ct);
                return ApiResult<string>.Success("ویرایش دسته‌بندی انجام شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<string>> DeleteCategoryAsync(int id, CancellationToken ct = default)
        {
            try
            {
                if (id <= 0) return ApiResult<string>.Error("شناسه نامعتبر است.");

                var entity = await _db.FoodCategories.FirstOrDefaultAsync(x => x.Id == id, ct);
                if (entity == null) return ApiResult<string>.Error("دسته‌بندی یافت نشد.");

                var hasItems = await _db.FoodItems.AsNoTracking().AnyAsync(x => x.FoodCategoryId == id, ct);
                if (hasItems) return ApiResult<string>.Error("این دسته‌بندی دارای آیتم است و قابل حذف نیست.");

                _db.FoodCategories.Remove(entity);
                await _db.SaveChangesAsync(ct);

                return ApiResult<string>.Success("حذف دسته‌بندی انجام شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<string>> SetCategoryActiveAsync(int id, bool isActive, CancellationToken ct = default)
        {
            try
            {
                if (id <= 0) return ApiResult<string>.Error("شناسه نامعتبر است.");

                var entity = await _db.FoodCategories.FirstOrDefaultAsync(x => x.Id == id, ct);
                if (entity == null) return ApiResult<string>.Error("دسته‌بندی یافت نشد.");

                entity.IsActive = isActive;
                await _db.SaveChangesAsync(ct);

                return ApiResult<string>.Success(isActive ? "دسته‌بندی فعال شد." : "دسته‌بندی غیرفعال شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }
    }
}
