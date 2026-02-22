using Application.Common.Models;
using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Domain.Entities.FastFood;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FoodItemService : GenericRepository<FoodItem>, IFoodItemService
    {
        private readonly AppDbContext _db;

        public FoodItemService(AppDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<ApiResult<PagedResult<FoodItemListItemDto>>> GetPagedItemsAsync(
            int page, int pageSize, int? categoryId = null, string? keyword = null, CancellationToken ct = default)
        {
            try
            {
                keyword = keyword?.Trim();

                var q = _db.FoodItems
                    .AsNoTracking()
                    .Include(x => x.FoodCategory)
                    .Include(x => x.Images)
                    .AsQueryable();

                if (categoryId.HasValue && categoryId.Value > 0)
                    q = q.Where(x => x.FoodCategoryId == categoryId.Value);

                if (!string.IsNullOrWhiteSpace(keyword))
                    q = q.Where(x => x.Title.Contains(keyword) || (x.Description != null && x.Description.Contains(keyword)));

                var total = await q.CountAsync(ct);

                var items = await q
                    .OrderBy(x => x.DisplayOrder).ThenBy(x => x.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new FoodItemListItemDto
                    {
                        Id = x.Id,
                        FoodCategoryId = x.FoodCategoryId,
                        CategoryTitle = x.FoodCategory.Title,
                        Title = x.Title,
                        Price = x.Price,
                        IsAvailable = x.IsAvailable,
                        DisplayOrder = x.DisplayOrder,
                        MainImageUrl = x.Images
                            .OrderByDescending(i => i.IsMain)
                            .ThenBy(i => i.DisplayOrder)
                            .Select(i => i.ImageUrl)
                            .FirstOrDefault()
                    })
                    .ToListAsync(ct);

                var result = new PagedResult<FoodItemListItemDto>
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total,
                    Items = items
                };

                return ApiResult<PagedResult<FoodItemListItemDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResult<PagedResult<FoodItemListItemDto>>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<int>> CreateItemAsync(FoodItemUpsertDto dto, CancellationToken ct = default)
        {
            try
            {
                if (dto.FoodCategoryId <= 0) return ApiResult<int>.Error("دسته‌بندی الزامی است.");
                if (string.IsNullOrWhiteSpace(dto.Title)) return ApiResult<int>.Error("عنوان غذا الزامی است.");
                if (dto.Price < 0) return ApiResult<int>.Error("قیمت معتبر نیست.");

                var catExists = await _db.FoodCategories.AsNoTracking().AnyAsync(x => x.Id == dto.FoodCategoryId, ct);
                if (!catExists) return ApiResult<int>.Error("دسته‌بندی انتخاب شده یافت نشد.");

                var entity = new FoodItem
                {
                    FoodCategoryId = dto.FoodCategoryId,
                    Title = dto.Title.Trim(),
                    Description = dto.Description?.Trim(),
                    Price = dto.Price,
                    IsAvailable = dto.IsAvailable,
                    DisplayOrder = dto.DisplayOrder
                };

                _db.FoodItems.Add(entity);
                await _db.SaveChangesAsync(ct);

                return ApiResult<int>.Success(entity.Id, "ثبت آیتم منو انجام شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<int>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<string>> UpdateItemAsync(int id, FoodItemUpsertDto dto, CancellationToken ct = default)
        {
            try
            {
                if (id <= 0) return ApiResult<string>.Error("شناسه نامعتبر است.");
                if (dto.FoodCategoryId <= 0) return ApiResult<string>.Error("دسته‌بندی الزامی است.");
                if (string.IsNullOrWhiteSpace(dto.Title)) return ApiResult<string>.Error("عنوان غذا الزامی است.");
                if (dto.Price < 0) return ApiResult<string>.Error("قیمت معتبر نیست.");

                var entity = await _db.FoodItems.FirstOrDefaultAsync(x => x.Id == id, ct);
                if (entity == null) return ApiResult<string>.Error("آیتم یافت نشد.");

                var catExists = await _db.FoodCategories.AsNoTracking().AnyAsync(x => x.Id == dto.FoodCategoryId, ct);
                if (!catExists) return ApiResult<string>.Error("دسته‌بندی انتخاب شده یافت نشد.");

                entity.FoodCategoryId = dto.FoodCategoryId;
                entity.Title = dto.Title.Trim();
                entity.Description = dto.Description?.Trim();
                entity.Price = dto.Price;
                entity.IsAvailable = dto.IsAvailable;
                entity.DisplayOrder = dto.DisplayOrder;

                await _db.SaveChangesAsync(ct);
                return ApiResult<string>.Success("ویرایش آیتم انجام شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<string>> DeleteItemAsync(int id, CancellationToken ct = default)
        {
            try
            {
                if (id <= 0) return ApiResult<string>.Error("شناسه نامعتبر است.");

                var entity = await _db.FoodItems
                    .Include(x => x.Images)
                    .FirstOrDefaultAsync(x => x.Id == id, ct);

                if (entity == null) return ApiResult<string>.Error("آیتم یافت نشد.");

                // اگر SoftDelete داری می‌تونی اینجا جایگزین کنی
                _db.FoodImages.RemoveRange(entity.Images);
                _db.FoodItems.Remove(entity);

                await _db.SaveChangesAsync(ct);
                return ApiResult<string>.Success("حذف آیتم انجام شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<string>> SetAvailabilityAsync(int id, bool isAvailable, CancellationToken ct = default)
        {
            try
            {
                if (id <= 0) return ApiResult<string>.Error("شناسه نامعتبر است.");

                var entity = await _db.FoodItems.FirstOrDefaultAsync(x => x.Id == id, ct);
                if (entity == null) return ApiResult<string>.Error("آیتم یافت نشد.");

                entity.IsAvailable = isAvailable;
                await _db.SaveChangesAsync(ct);

                return ApiResult<string>.Success("وضعیت موجودی/نمایش بروزرسانی شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }
    }
}
