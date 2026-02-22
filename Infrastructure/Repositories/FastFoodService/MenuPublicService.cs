using Application.Common.Models;
using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MenuPublicService : IMenuPublicService
    {
        private readonly AppDbContext _db;

        public MenuPublicService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ApiResult<List<PublicMenuCategoryDto>>> GetPublicMenuAsync(CancellationToken ct = default)
        {
            try
            {
                var cats = await _db.FoodCategories
                    .AsNoTracking()
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id)
                    .Select(c => new PublicMenuCategoryDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Description = c.Description,
                        DisplayOrder = c.DisplayOrder
                    })
                    .ToListAsync(ct);

                if (cats.Count == 0)
                    return ApiResult<List<PublicMenuCategoryDto>>.Success(new List<PublicMenuCategoryDto>());

                var catIds = cats.Select(x => x.Id).ToList();

                var items = await _db.FoodItems
                    .AsNoTracking()
                    .Where(i => catIds.Contains(i.FoodCategoryId) && i.IsAvailable)
                    .OrderBy(i => i.DisplayOrder).ThenBy(i => i.Id)
                    .Select(i => new
                    {
                        i.Id,
                        i.FoodCategoryId,
                        i.Title,
                        i.Description,
                        i.Price,
                        i.DisplayOrder,
                        Images = _db.FoodImages
                            .Where(img => img.FoodItemId == i.Id)
                            .OrderByDescending(img => img.IsMain)
                            .ThenBy(img => img.DisplayOrder)
                            .Select(img => new { img.ImageUrl, img.IsMain })
                            .ToList()
                    })
                    .ToListAsync(ct);

                var map = cats.ToDictionary(x => x.Id, x => x);

                foreach (var it in items)
                {
                    var dto = new PublicMenuFoodItemDto
                    {
                        Id = it.Id,
                        Title = it.Title,
                        Description = it.Description,
                        Price = it.Price,
                        DisplayOrder = it.DisplayOrder,
                        MainImageUrl = it.Images.FirstOrDefault()?.ImageUrl,
                        ImageUrls = it.Images.Select(x => x.ImageUrl).ToList()
                    };

                    if (map.TryGetValue(it.FoodCategoryId, out var cat))
                        cat.Items.Add(dto);
                }

                // اگر دسته‌ای آیتم نداشت، خالی می‌مونه (می‌تونی فیلترش هم کنی)
                return ApiResult<List<PublicMenuCategoryDto>>.Success(cats);
            }
            catch (Exception ex)
            {
                return ApiResult<List<PublicMenuCategoryDto>>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }
    }
}
