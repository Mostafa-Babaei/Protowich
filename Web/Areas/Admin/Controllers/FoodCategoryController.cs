using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models;

namespace Web.Areas.Admin.Controllers
{
    public class FoodCategoriesController : AdminBaseController
    {
        private readonly IFoodCategoryService _svc;

        public FoodCategoriesController(IFoodCategoryService svc)
        {
            _svc = svc;
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

        public IActionResult Create()
        {
            return View(new FoodCategoryFormVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodCategoryFormVm vm, CancellationToken ct)
        {
            var res = await _svc.CreateCategoryAsync(vm.Dto, ct);
            if (!res.IsSuccess)
            {
                TempData["err"] = res.Message;
                return View(vm);
            }

            TempData["ok"] = "????????? ??? ??.";
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
            var res = await _svc.UpdateCategoryAsync(id, vm.Dto, ct);
            if (!res.IsSuccess)
            {
                TempData["err"] = res.Message;
                vm.Id = id;
                return View(vm);
            }

            TempData["ok"] = "?????? ????? ??.";
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
    }
}


