using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models;

namespace Web.Areas.Admin.Controllers
{
    public class FoodImagesController : AdminBaseController
    {
        private readonly IFoodImageService _svc;

        public FoodImagesController(IFoodImageService svc)
        {
            _svc = svc;
        }

        public async Task<IActionResult> Index(int foodItemId, CancellationToken ct)
        {
            var res = await _svc.GetItemImagesAsync(foodItemId, ct);
            if (!res.IsSuccess) TempData["err"] = res.Message;

            ViewBag.FoodItemId = foodItemId;
            return View(res.Data ?? new List<FoodImageListItemDto>());
        }

        public IActionResult Create(int foodItemId)
        {
            return View(new FoodImageFormVm
            {
                FoodItemId = foodItemId,
                Dto = new FoodImageUpsertDto { FoodItemId = foodItemId }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodImageFormVm vm, CancellationToken ct)
        {
            vm.Dto.FoodItemId = vm.FoodItemId;

            var res = await _svc.AddImageAsync(vm.Dto, ct);
            if (!res.IsSuccess)
            {
                TempData["err"] = res.Message;
                return View(vm);
            }

            TempData["ok"] = "????? ??? ??.";
            return RedirectToAction(nameof(Index), new { foodItemId = vm.FoodItemId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetMain(int imageId, int foodItemId, CancellationToken ct)
        {
            var res = await _svc.SetMainImageAsync(imageId, ct);
            TempData[res.IsSuccess ? "ok" : "err"] = res.Message;
            return RedirectToAction(nameof(Index), new { foodItemId });
        }

        public async Task<IActionResult> Delete(int id, int foodItemId, CancellationToken ct)
        {
            // ??? ???? ????? ????? ???
            ViewBag.FoodItemId = foodItemId;
            return View(new FoodImageListItemDto { Id = id, FoodItemId = foodItemId });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int foodItemId, CancellationToken ct)
        {
            var res = await _svc.DeleteImageAsync(id, ct);
            TempData[res.IsSuccess ? "ok" : "err"] = res.Message;
            return RedirectToAction(nameof(Index), new { foodItemId });
        }
    }
}


