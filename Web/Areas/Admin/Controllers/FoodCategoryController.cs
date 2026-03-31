using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models;

namespace Web.Areas.Admin.Controllers
{
    public class FoodCategoriesController : AdminBaseController
    {
        private readonly IFoodCategoryService _svc;
        private readonly IFoodItemService _itemSvc;

        public FoodCategoriesController(IFoodCategoryService svc, IFoodItemService itemSvc)
        {
            _svc = svc;
            _itemSvc = itemSvc;
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

        public IActionResult Create() => View(new FoodCategoryUpsertDto());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodCategoryUpsertDto dto, CancellationToken ct)
        {
            dto ??= new FoodCategoryUpsertDto();
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                ModelState.AddModelError(nameof(dto.Title), "عنوان دسته‌بندی الزامی است.");
                return View(dto);
            }

            var res = await _svc.CreateCategoryAsync(dto, ct);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(res.DeveloperMessage)
                    ? (res.Message ?? "خطا در ثبت دسته‌بندی.")
                    : $"{res.Message} ({res.DeveloperMessage})");
                return View(dto);
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

            var res = await _svc.UpdateCategoryAsync(id, vm.Dto, ct);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(res.DeveloperMessage)
                    ? (res.Message ?? "خطا در ویرایش دسته‌بندی.")
                    : $"{res.Message} ({res.DeveloperMessage})");
                vm.Id = id;
                return View(vm);
            }

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
            var res = await _svc.DeleteCategoryAsync(id, ct);
            TempData[res.IsSuccess ? "ok" : "err"] = res.Message;
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
            if (ids.Any(id => !validIds.Contains(id)))
                return Json(new { isSuccess = false, message = "چیدمان ارسالی معتبر نیست." });

            var newOrder = 1;
            foreach (var id in ids)
            {
                var entity = await _itemSvc.GetByIdAsync(id);
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

                var updateRes = await _itemSvc.UpdateItemAsync(id, dto, ct);
                if (!updateRes.IsSuccess)
                    return Json(new { isSuccess = false, message = updateRes.Message });
            }

            return Json(new { isSuccess = true, message = "ترتیب نمایش آیتم‌ها بروزرسانی شد." });
        }
    }
}


