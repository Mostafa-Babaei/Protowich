using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models;

namespace Web.Areas.Admin.Controllers
{
    public class FoodImagesController : AdminBaseController
    {
        private readonly IFoodImageService _svc;
        private readonly IWebHostEnvironment _env;

        public FoodImagesController(IFoodImageService svc, IWebHostEnvironment env)
        {
            _svc = svc;
            _env = env;
        }

        public async Task<IActionResult> Index(int? foodItemId, CancellationToken ct)
        {
            if (!foodItemId.HasValue || foodItemId.Value <= 0)
            {
                TempData["err"] = "ابتدا یک آیتم غذا انتخاب کنید.";
                return RedirectToAction("Index", "FoodItems", new { area = "Admin" });
            }

            var res = await _svc.GetItemImagesAsync(foodItemId.Value, ct);
            if (!res.IsSuccess) TempData["err"] = res.Message;

            ViewBag.FoodItemId = foodItemId.Value;
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
            if (vm.UploadFile == null || vm.UploadFile.Length <= 0)
            {
                TempData["err"] = "لطفا یک فایل تصویر انتخاب کنید.";
                return View(vm);
            }

            var saveRes = await SaveUploadedFileAsync(vm.UploadFile, ct);
            if (!saveRes.IsSuccess)
            {
                TempData["err"] = saveRes.Message;
                return View(vm);
            }

            vm.Dto.ImageUrl = saveRes.Path!;

            var res = await _svc.AddImageAsync(vm.Dto, ct);
            if (!res.IsSuccess)
            {
                if (!string.IsNullOrWhiteSpace(vm.Dto.ImageUrl))
                {
                    var physicalPath = System.IO.Path.Combine(_env.WebRootPath, vm.Dto.ImageUrl.TrimStart('/').Replace('/', System.IO.Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(physicalPath))
                        System.IO.File.Delete(physicalPath);
                }

                TempData["err"] = res.Message;
                return View(vm);
            }

            TempData["ok"] = "????? ??? ??.";
            return RedirectToAction(nameof(Index), new { foodItemId = vm.FoodItemId });
        }

        private async Task<(bool IsSuccess, string? Path, string Message)> SaveUploadedFileAsync(IFormFile file, CancellationToken ct)
        {
            var extension = System.IO.Path.GetExtension(file.FileName)?.ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            if (string.IsNullOrWhiteSpace(extension) || !allowed.Contains(extension))
                return (false, null, "فرمت فایل مجاز نیست.");

            var uploadsRoot = System.IO.Path.Combine(_env.WebRootPath, "uploads", "food-images");
            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var physicalPath = System.IO.Path.Combine(uploadsRoot, fileName);

            await using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream, ct);
            }

            var relativePath = $"/uploads/food-images/{fileName}";
            return (true, relativePath, string.Empty);
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
            if (id <= 0 || foodItemId <= 0) return NotFound();

            var entity = await _svc.GetByIdAsync(id);
            if (entity == null || entity.FoodItemId != foodItemId) return NotFound();

            ViewBag.FoodItemId = foodItemId;
            return View(new FoodImageListItemDto
            {
                Id = entity.Id,
                FoodItemId = entity.FoodItemId,
                ImageUrl = entity.ImageUrl,
                IsMain = entity.IsMain,
                DisplayOrder = entity.DisplayOrder
            });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int foodItemId, CancellationToken ct)
        {
            var entity = await _svc.GetByIdAsync(id);
            var imageUrl = entity?.ImageUrl;

            var res = await _svc.DeleteImageAsync(id, ct);
            TempData[res.IsSuccess ? "ok" : "err"] = res.Message;

            if (res.IsSuccess && !string.IsNullOrWhiteSpace(imageUrl) && imageUrl.StartsWith('/'))
            {
                var physicalPath = System.IO.Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/').Replace('/', System.IO.Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(physicalPath))
                    System.IO.File.Delete(physicalPath);
            }

            return RedirectToAction(nameof(Index), new { foodItemId });
        }
    }
}


