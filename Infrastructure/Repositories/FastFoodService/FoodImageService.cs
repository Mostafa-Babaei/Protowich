using Application.Common.Models;
using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Domain.Entities.FastFood;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FoodImageService : GenericRepository<FoodImage>, IFoodImageService
    {
        private readonly AppDbContext _db;

        public FoodImageService(AppDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<ApiResult<List<FoodImageListItemDto>>> GetItemImagesAsync(int foodItemId, CancellationToken ct = default)
        {
            try
            {
                if (foodItemId <= 0) return ApiResult<List<FoodImageListItemDto>>.Error("FoodItemId نامعتبر است.");

                var exists = await _db.FoodItems.AsNoTracking().AnyAsync(x => x.Id == foodItemId, ct);
                if (!exists) return ApiResult<List<FoodImageListItemDto>>.Error("آیتم یافت نشد.");

                var imgs = await _db.FoodImages.AsNoTracking()
                    .Where(x => x.FoodItemId == foodItemId)
                    .OrderByDescending(x => x.IsMain)
                    .ThenBy(x => x.DisplayOrder)
                    .ThenBy(x => x.Id)
                    .Select(x => new FoodImageListItemDto
                    {
                        Id = x.Id,
                        FoodItemId = x.FoodItemId,
                        ImageUrl = x.ImageUrl,
                        IsMain = x.IsMain,
                        DisplayOrder = x.DisplayOrder
                    })
                    .ToListAsync(ct);

                return ApiResult<List<FoodImageListItemDto>>.Success(imgs);
            }
            catch (Exception ex)
            {
                return ApiResult<List<FoodImageListItemDto>>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<int>> AddImageAsync(FoodImageUpsertDto dto, CancellationToken ct = default)
        {
            try
            {
                if (dto.FoodItemId <= 0) return ApiResult<int>.Error("FoodItemId الزامی است.");
                if (string.IsNullOrWhiteSpace(dto.ImageUrl)) return ApiResult<int>.Error("آدرس تصویر الزامی است.");

                var item = await _db.FoodItems.FirstOrDefaultAsync(x => x.Id == dto.FoodItemId, ct);
                if (item == null) return ApiResult<int>.Error("آیتم یافت نشد.");

                // اگر کاربر IsMain زد، اول بقیه رو غیر اصلی کن
                if (dto.IsMain)
                {
                    var others = await _db.FoodImages.Where(x => x.FoodItemId == dto.FoodItemId && x.IsMain).ToListAsync(ct);
                    foreach (var o in others) o.IsMain = false;
                }

                var entity = new FoodImage
                {
                    FoodItemId = dto.FoodItemId,
                    ImageUrl = dto.ImageUrl.Trim(),
                    IsMain = dto.IsMain,
                    DisplayOrder = dto.DisplayOrder
                };

                _db.FoodImages.Add(entity);
                await _db.SaveChangesAsync(ct);

                // اگر اولین تصویر بود، خودکار اصلی کن
                var count = await _db.FoodImages.AsNoTracking().CountAsync(x => x.FoodItemId == dto.FoodItemId, ct);
                if (count == 1 && !entity.IsMain)
                {
                    entity.IsMain = true;
                    await _db.SaveChangesAsync(ct);
                }

                return ApiResult<int>.Success(entity.Id, "تصویر ثبت شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<int>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<string>> DeleteImageAsync(int id, CancellationToken ct = default)
        {
            try
            {
                if (id <= 0) return ApiResult<string>.Error("شناسه نامعتبر است.");

                var img = await _db.FoodImages.FirstOrDefaultAsync(x => x.Id == id, ct);
                if (img == null) return ApiResult<string>.Error("تصویر یافت نشد.");

                var foodItemId = img.FoodItemId;
                var wasMain = img.IsMain;

                _db.FoodImages.Remove(img);
                await _db.SaveChangesAsync(ct);

                // اگر تصویر اصلی حذف شد، یکی دیگر را اصلی کن
                if (wasMain)
                {
                    var next = await _db.FoodImages
                        .Where(x => x.FoodItemId == foodItemId)
                        .OrderBy(x => x.DisplayOrder).ThenBy(x => x.Id)
                        .FirstOrDefaultAsync(ct);

                    if (next != null)
                    {
                        next.IsMain = true;
                        await _db.SaveChangesAsync(ct);
                    }
                }

                return ApiResult<string>.Success("تصویر حذف شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<string>> SetMainImageAsync(int imageId, CancellationToken ct = default)
        {
            try
            {
                if (imageId <= 0) return ApiResult<string>.Error("شناسه نامعتبر است.");

                var img = await _db.FoodImages.FirstOrDefaultAsync(x => x.Id == imageId, ct);
                if (img == null) return ApiResult<string>.Error("تصویر یافت نشد.");

                var others = await _db.FoodImages
                    .Where(x => x.FoodItemId == img.FoodItemId && x.Id != img.Id && x.IsMain)
                    .ToListAsync(ct);

                foreach (var o in others) o.IsMain = false;

                img.IsMain = true;

                await _db.SaveChangesAsync(ct);
                return ApiResult<string>.Success("تصویر اصلی تنظیم شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }
    }
}
