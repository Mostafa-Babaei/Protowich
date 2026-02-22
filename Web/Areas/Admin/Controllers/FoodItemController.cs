using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FoodItemsController : Controller
    {
        private readonly IFoodItemService _svc;
        private readonly IFoodCategoryService _catSvc;
        private readonly IFoodImageService _imgSvc;

        public FoodItemsController(IFoodItemService svc, IFoodCategoryService catSvc, IFoodImageService imgSvc)
        {
            _svc = svc;
            _catSvc = catSvc;
            _imgSvc = imgSvc;
        }

        public async Task<IActionResult> Index([FromQuery] FoodItemIndexVm vm, CancellationToken ct)
        {
            vm.Page = vm.Page <= 0 ? 1 : vm.Page;
            vm.PageSize = vm.PageSize <= 0 ? 20 : vm.PageSize;

            // dropdown categories
            var cats = await _catSvc.GetPagedCategoriesAsync(1, 500, null, ct);
            vm.Categories = cats.Data?.Items?.ToList() ?? new();

            var res = await _svc.GetPagedItemsAsync(vm.Page, vm.PageSize, vm.CategoryId, vm.Keyword, ct);
            if (!res.IsSuccess) TempData["err"] = res.Message;

            vm.Data = res.Data;
            return View(vm);
        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var vm = new FoodItemFormVm();
            var cats = await _catSvc.GetPagedCategoriesAsync(1, 500, null, ct);
            vm.Categories = cats.Data?.Items?.ToList() ?? new();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodItemFormVm vm, CancellationToken ct)
        {
            var res = await _svc.CreateItemAsync(vm.Dto, ct);
            if (!res.IsSuccess)
            {
                TempData["err"] = res.Message;
                var cats = await _catSvc.GetPagedCategoriesAsync(1, 500, null, ct);
                vm.Categories = cats.Data?.Items?.ToList() ?? new();
                return View(vm);
            }

            TempData["ok"] = "آیتم ثبت شد.";
            return RedirectToAction(nameof(Edit), new { id = res.Data }); // مستقیم برو به ویرایش برای تصاویر
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var entity = await _svc.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var vm = new FoodItemFormVm
            {
                Id = entity.Id,
                Dto = new FoodItemUpsertDto
                {
                    FoodCategoryId = entity.FoodCategoryId,
                    Title = entity.Title,
                    Description = entity.Description,
                    Price = entity.Price,
                    IsAvailable = entity.IsAvailable,
                    DisplayOrder = entity.DisplayOrder
                }
            };

            var cats = await _catSvc.GetPagedCategoriesAsync(1, 500, null, ct);
            vm.Categories = cats.Data?.Items?.ToList() ?? new();

            var imgs = await _imgSvc.GetItemImagesAsync(id, ct);
            vm.Images = imgs.Data ?? new();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FoodItemFormVm vm, CancellationToken ct)
        {
            var res = await _svc.UpdateItemAsync(id, vm.Dto, ct);
            if (!res.IsSuccess)
            {
                TempData["err"] = res.Message;
                vm.Id = id;

                var cats = await _catSvc.GetPagedCategoriesAsync(1, 500, null, ct);
                vm.Categories = cats.Data?.Items?.ToList() ?? new();

                var imgs = await _imgSvc.GetItemImagesAsync(id, ct);
                vm.Images = imgs.Data ?? new();

                return View(vm);
            }

            TempData["ok"] = "ویرایش انجام شد.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetAvailability(int id, bool isAvailable, CancellationToken ct)
        {
            var res = await _svc.SetAvailabilityAsync(id, isAvailable, ct);
            TempData[res.IsSuccess ? "ok" : "err"] = res.Message;
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
            var res = await _svc.DeleteItemAsync(id, ct);
            TempData[res.IsSuccess ? "ok" : "err"] = res.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
