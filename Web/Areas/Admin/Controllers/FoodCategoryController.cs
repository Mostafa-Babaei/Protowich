using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.Text.Json;
using Web.Areas.Admin.Models;

namespace Web.Areas.Admin.Controllers
{
    public class FoodCategoriesController : AdminBaseController
    {
        private readonly IFoodCategoryService _svc;
        private readonly IFoodItemService _itemSvc;
        private readonly IWebHostEnvironment _env;

        public FoodCategoriesController(IFoodCategoryService svc, IFoodItemService itemSvc, IWebHostEnvironment env)
        {
            _svc = svc;
            _itemSvc = itemSvc;
            _env = env;
        }

        public async Task<IActionResult> Index([FromQuery] FoodCategoryIndexVm vm, CancellationToken ct)
        {
            vm.Page = vm.Page <= 0 ? 1 : vm.Page;
            vm.PageSize = vm.PageSize <= 0 ? 20 : vm.PageSize;

            var res = await _svc.GetPagedCategoriesAsync(vm.Page, vm.PageSize, vm.Keyword, ct);
            if (!res.IsSuccess) TempData["err"] = res.Message;

            vm.Data = res.Data;
            return View(vm);
        }

        public IActionResult Create() => View(new FoodCategoryFormVm { Dto = new FoodCategoryUpsertDto { IsActive = true } });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodCategoryFormVm vm, CancellationToken ct)
        {
            vm ??= new FoodCategoryFormVm();
            vm.Dto ??= new FoodCategoryUpsertDto();

            if (string.IsNullOrWhiteSpace(vm.Dto.Title))
            {
                ModelState.AddModelError("Dto.Title", "عنوان دسته‌بندی الزامی است.");
                return View(vm);
            }

            string? uploadedPath = null;
            if (vm.UploadFile != null && vm.UploadFile.Length > 0)
            {
                var saveRes = await SaveUploadedCategoryImageAsync(vm.UploadFile, ct);
                if (!saveRes.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, saveRes.Message);
                    return View(vm);
                }

                uploadedPath = saveRes.Path;
                vm.Dto.ImageUrl = uploadedPath;
            }

            var res = await _svc.CreateCategoryAsync(vm.Dto, ct);
            if (!res.IsSuccess)
            {
                if (!string.IsNullOrWhiteSpace(uploadedPath))
                    DeletePhysicalFile(uploadedPath);

                ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(res.DeveloperMessage)
                    ? (res.Message ?? "خطا در ثبت دسته‌بندی.")
                    : $"{res.Message} ({res.DeveloperMessage})");
                return View(vm);
            }

            TempData["ok"] = "دسته‌بندی با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var entity = await _svc.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var vm = new FoodCategoryFormVm
            {
                Id = entity.Id,
                Dto = new FoodCategoryUpsertDto
                {
                    Title = entity.Title,
                    Description = entity.Description,
                    ImageUrl = entity.ImageUrl,
                    DisplayOrder = entity.DisplayOrder,
                    IsActive = entity.IsActive
                }
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FoodCategoryFormVm vm, CancellationToken ct)
        {
            vm ??= new FoodCategoryFormVm();
            vm.Dto ??= new FoodCategoryUpsertDto();

            if (string.IsNullOrWhiteSpace(vm.Dto.Title))
            {
                ModelState.AddModelError("Dto.Title", "عنوان دسته‌بندی الزامی است.");
                vm.Id = id;
                return View(vm);
            }

            var entity = await _svc.GetByIdAsync(id);
            if (entity == null)
            {
                TempData["err"] = "دسته‌بندی یافت نشد.";
                return RedirectToAction(nameof(Index));
            }

            vm.Dto.ImageUrl = entity.ImageUrl;
            string? newUploadedPath = null;

            if (vm.UploadFile != null && vm.UploadFile.Length > 0)
            {
                var saveRes = await SaveUploadedCategoryImageAsync(vm.UploadFile, ct);
                if (!saveRes.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, saveRes.Message);
                    vm.Id = id;
                    return View(vm);
                }

                newUploadedPath = saveRes.Path;
                vm.Dto.ImageUrl = newUploadedPath;
            }

            var oldPath = entity.ImageUrl;
            var res = await _svc.UpdateCategoryAsync(id, vm.Dto, ct);
            if (!res.IsSuccess)
            {
                if (!string.IsNullOrWhiteSpace(newUploadedPath))
                    DeletePhysicalFile(newUploadedPath);

                ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(res.DeveloperMessage)
                    ? (res.Message ?? "خطا در ویرایش دسته‌بندی.")
                    : $"{res.Message} ({res.DeveloperMessage})");
                vm.Id = id;
                return View(vm);
            }

            if (!string.IsNullOrWhiteSpace(newUploadedPath) && !string.IsNullOrWhiteSpace(oldPath))
                DeletePhysicalFile(oldPath);

            TempData["ok"] = "دسته‌بندی با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var entity = await _svc.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var entity = await _svc.GetByIdAsync(id);
            var oldPath = entity?.ImageUrl;

            var res = await _svc.DeleteCategoryAsync(id, ct);
            TempData[res.IsSuccess ? "ok" : "err"] = res.Message;

            if (res.IsSuccess && !string.IsNullOrWhiteSpace(oldPath))
                DeletePhysicalFile(oldPath);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActive([FromForm] int id, [FromForm] bool? isActive, CancellationToken ct)
        {
            if (id <= 0 || isActive is null)
            {
                TempData["err"] = "پارامترهای درخواست نامعتبر است.";
                return RedirectToAction(nameof(Index));
            }

            var res = await _svc.SetCategoryActiveAsync(id, isActive.Value, ct);
            TempData[res.IsSuccess ? "ok" : "err"] = res.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryItems(int id, CancellationToken ct)
        {
            if (id <= 0)
                return Json(new { isSuccess = false, message = "شناسه نامعتبر است.", items = Array.Empty<object>() });

            var res = await _itemSvc.GetPagedItemsAsync(1, 200, id, null, ct);
            if (!res.IsSuccess)
                return Json(new { isSuccess = false, message = res.Message, items = Array.Empty<object>() });

            var items = res.Data?.Items?.Select(x => new
            {
                id = x.Id,
                title = x.Title,
                price = x.Price,
                isAvailable = x.IsAvailable,
                displayOrder = x.DisplayOrder
            }) ?? Enumerable.Empty<object>();

            return Json(new { isSuccess = true, message = res.Message, items });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategoryItemPrice([FromForm] int id, [FromForm] decimal? price, CancellationToken ct)
        {
            if (id <= 0 || price is null || price.Value < 0)
                return Json(new { isSuccess = false, message = "پارامترهای درخواست نامعتبر است." });

            var entity = await _itemSvc.GetByIdAsync(id);
            if (entity == null)
                return Json(new { isSuccess = false, message = "آیتم یافت نشد." });

            var dto = new FoodItemUpsertDto
            {
                FoodCategoryId = entity.FoodCategoryId,
                Title = entity.Title,
                Description = entity.Description,
                Price = price.Value,
                IsAvailable = entity.IsAvailable,
                DisplayOrder = entity.DisplayOrder
            };

            var res = await _itemSvc.UpdateItemAsync(id, dto, ct);
            return Json(new { isSuccess = res.IsSuccess, message = res.Message, newPrice = price.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategoryItemsPrices([FromForm] int categoryId, [FromForm] string? items, CancellationToken ct)
        {
            if (categoryId <= 0 || string.IsNullOrWhiteSpace(items))
                return Json(new { isSuccess = false, message = "پارامترهای درخواست نامعتبر است." });

            var pairs = items
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .ToList();

            if (pairs.Count == 0)
                return Json(new { isSuccess = false, message = "لیست قیمت‌ها نامعتبر است." });

            var toUpdate = new List<(int Id, decimal Price)>();
            foreach (var pair in pairs)
            {
                var parts = pair.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                    return Json(new { isSuccess = false, message = "فرمت لیست قیمت‌ها نامعتبر است." });

                if (!int.TryParse(parts[0], out var itemId) || itemId <= 0)
                    return Json(new { isSuccess = false, message = "شناسه آیتم نامعتبر است." });

                var priceText = parts[1].Trim();
                var parsed = decimal.TryParse(priceText, NumberStyles.Number, CultureInfo.InvariantCulture, out var price)
                             || decimal.TryParse(priceText, NumberStyles.Number, CultureInfo.CurrentCulture, out price);
                if (!parsed || price < 0)
                    return Json(new { isSuccess = false, message = "مقدار قیمت نامعتبر است." });

                toUpdate.Add((itemId, price));
            }

            if (toUpdate.Count == 0)
                return Json(new { isSuccess = false, message = "قیمتی برای بروزرسانی ارسال نشده است." });

            var listRes = await _itemSvc.GetPagedItemsAsync(1, 500, categoryId, null, ct);
            if (!listRes.IsSuccess)
                return Json(new { isSuccess = false, message = listRes.Message });

            var validIds = (listRes.Data?.Items?.Select(x => x.Id).ToHashSet()) ?? new HashSet<int>();
            if (toUpdate.Any(x => !validIds.Contains(x.Id)))
                return Json(new { isSuccess = false, message = "برخی آیتم‌ها متعلق به این دسته‌بندی نیستند." });

            foreach (var row in toUpdate)
            {
                var entity = await _itemSvc.GetByIdAsync(row.Id);
                if (entity == null || entity.FoodCategoryId != categoryId)
                    return Json(new { isSuccess = false, message = "آیتم نامعتبر در لیست بروزرسانی وجود دارد." });

                var dto = new FoodItemUpsertDto
                {
                    FoodCategoryId = entity.FoodCategoryId,
                    Title = entity.Title,
                    Description = entity.Description,
                    Price = row.Price,
                    IsAvailable = entity.IsAvailable,
                    DisplayOrder = entity.DisplayOrder
                };

                var updateRes = await _itemSvc.UpdateItemAsync(row.Id, dto, ct);
                if (!updateRes.IsSuccess)
                    return Json(new { isSuccess = false, message = updateRes.Message });
            }

            return Json(new { isSuccess = true, message = "قیمت همه آیتم‌ها بروزرسانی شد." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleCategoryItemAvailability([FromForm] int id, [FromForm] bool? isAvailable, CancellationToken ct)
        {
            if (id <= 0 || isAvailable is null)
                return Json(new { isSuccess = false, message = "پارامترهای درخواست نامعتبر است." });

            var res = await _itemSvc.SetAvailabilityAsync(id, isAvailable.Value, ct);
            return Json(new { isSuccess = res.IsSuccess, message = res.Message, isAvailable = isAvailable.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategoryItemsOrder([FromForm] int categoryId, [FromForm] string? orderedIds, CancellationToken ct)
        {
            if (categoryId <= 0 || string.IsNullOrWhiteSpace(orderedIds))
                return Json(new { isSuccess = false, message = "پارامترهای درخواست نامعتبر است." });

            var ids = orderedIds
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.TryParse(x, out var id) ? id : 0)
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            if (ids.Count == 0)
                return Json(new { isSuccess = false, message = "لیست آیتم‌ها نامعتبر است." });

            var listRes = await _itemSvc.GetPagedItemsAsync(1, 500, categoryId, null, ct);
            if (!listRes.IsSuccess)
                return Json(new { isSuccess = false, message = listRes.Message });

            var validIds = (listRes.Data?.Items?.Select(x => x.Id).ToHashSet()) ?? new HashSet<int>();
            if (ids.Any(itemId => !validIds.Contains(itemId)))
                return Json(new { isSuccess = false, message = "چیدمان ارسالی معتبر نیست." });

            var newOrder = 1;
            foreach (var itemId in ids)
            {
                var entity = await _itemSvc.GetByIdAsync(itemId);
                if (entity == null || entity.FoodCategoryId != categoryId)
                    return Json(new { isSuccess = false, message = "آیتم نامعتبر در لیست چیدمان وجود دارد." });

                var dto = new FoodItemUpsertDto
                {
                    FoodCategoryId = entity.FoodCategoryId,
                    Title = entity.Title,
                    Description = entity.Description,
                    Price = entity.Price,
                    IsAvailable = entity.IsAvailable,
                    DisplayOrder = newOrder++
                };

                var updateRes = await _itemSvc.UpdateItemAsync(itemId, dto, ct);
                if (!updateRes.IsSuccess)
                    return Json(new { isSuccess = false, message = updateRes.Message });
            }

            return Json(new { isSuccess = true, message = "ترتیب نمایش آیتم‌ها بروزرسانی شد." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategoryItemsBulk([FromForm] int categoryId, [FromForm] string? itemsJson, CancellationToken ct)
        {
            if (categoryId <= 0 || string.IsNullOrWhiteSpace(itemsJson))
                return Json(new { isSuccess = false, message = "پارامترهای درخواست نامعتبر است." });

            var category = await _svc.GetByIdAsync(categoryId);
            if (category == null)
                return Json(new { isSuccess = false, message = "دسته‌بندی یافت نشد." });

            List<BulkFoodItemInput>? rows;
            try
            {
                rows = JsonSerializer.Deserialize<List<BulkFoodItemInput>>(itemsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                return Json(new { isSuccess = false, message = "فرمت لیست آیتم‌ها نامعتبر است." });
            }

            if (rows == null || rows.Count == 0)
                return Json(new { isSuccess = false, message = "آیتمی برای ثبت ارسال نشده است." });

            var validRows = rows
                .Where(x => x != null)
                .Select(x => new
                {
                    Title = (x!.Title ?? string.Empty).Trim(),
                    Price = x.Price
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.Title) && x.Price.HasValue && x.Price.Value >= 0)
                .ToList();

            if (validRows.Count == 0)
                return Json(new { isSuccess = false, message = "حداقل یک ردیف معتبر (نام + قیمت) وارد کنید." });

            var listRes = await _itemSvc.GetPagedItemsAsync(1, 2000, categoryId, null, ct);
            if (!listRes.IsSuccess)
                return Json(new { isSuccess = false, message = listRes.Message });

            var nextDisplayOrder = (listRes.Data?.Items?.Any() == true)
                ? listRes.Data.Items.Max(x => x.DisplayOrder) + 1
                : 1;

            var successCount = 0;
            var failCount = 0;
            string? firstError = null;

            foreach (var row in validRows)
            {
                var createRes = await _itemSvc.CreateItemAsync(new FoodItemUpsertDto
                {
                    FoodCategoryId = categoryId,
                    Title = row.Title,
                    Price = row.Price!.Value,
                    Description = null,
                    IsAvailable = true,
                    DisplayOrder = nextDisplayOrder++
                }, ct);

                if (createRes.IsSuccess) successCount++;
                else
                {
                    failCount++;
                    firstError ??= createRes.Message;
                }
            }

            if (successCount == 0)
                return Json(new { isSuccess = false, message = firstError ?? "خطا در ثبت گروهی آیتم‌ها." });

            var message = failCount == 0
                ? $"{successCount} آیتم با موفقیت ثبت شد."
                : $"{successCount} آیتم ثبت شد و {failCount} آیتم خطا داشت.";

            return Json(new { isSuccess = true, message });
        }

        private async Task<(bool IsSuccess, string? Path, string Message)> SaveUploadedCategoryImageAsync(IFormFile file, CancellationToken ct)
        {
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            if (string.IsNullOrWhiteSpace(extension) || !allowed.Contains(extension))
                return (false, null, "فرمت فایل مجاز نیست.");

            var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", "category-images");
            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var physicalPath = Path.Combine(uploadsRoot, fileName);

            await using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream, ct);
            }

            return (true, $"/uploads/category-images/{fileName}", string.Empty);
        }

        private void DeletePhysicalFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath) || !relativePath.StartsWith('/'))
                return;

            var physicalPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(physicalPath))
                System.IO.File.Delete(physicalPath);
        }

        private sealed class BulkFoodItemInput
        {
            public string? Title { get; set; }
            public decimal? Price { get; set; }
        }
    }
}
