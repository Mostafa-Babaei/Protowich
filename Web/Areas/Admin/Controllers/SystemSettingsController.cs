using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models;

namespace Web.Areas.Admin.Controllers
{
    public class SystemSettingsController : AdminBaseController
    {
        private readonly ISystemSettingService _svc;
        private readonly IWebHostEnvironment _env;

        public SystemSettingsController(ISystemSettingService svc, IWebHostEnvironment env)
        {
            _svc = svc;
            _env = env;
        }

        public async Task<IActionResult> Index([FromQuery] string? category, CancellationToken ct)
        {
            var vm = new SystemSettingIndexVm
            {
                Category = string.IsNullOrWhiteSpace(category) ? "Landing" : category.Trim()
            };

            var res = await _svc.GetByCategoryAsync(vm.Category, ct);
            if (!res.IsSuccess) TempData["err"] = res.Message;
            vm.Items = res.Data ?? new List<SystemSettingItemDto>();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] int id, [FromForm] string category, [FromForm] SystemSettingUpdateDto dto, CancellationToken ct)
        {
            var res = await _svc.UpdateAsync(id, dto, ct);
            TempData[res.IsSuccess ? "ok" : "err"] = res.Message;
            return RedirectToAction(nameof(Index), new { category });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkUpdate(
            [FromForm] string category,
            [FromForm] List<SystemSettingBulkUpdateItemVm> items,
            CancellationToken ct)
        {
            if (items == null || items.Count == 0)
            {
                TempData["err"] = "موردی برای ذخیره وجود ندارد.";
                return RedirectToAction(nameof(Index), new { category });
            }

            var successCount = 0;
            var failCount = 0;

            foreach (var item in items)
            {
                if (item.Id <= 0)
                {
                    failCount++;
                    continue;
                }

                var finalValue = item.Value;
                if (item.UploadFile != null && item.UploadFile.Length > 0)
                {
                    var saveRes = await SaveUploadedSettingImageAsync(item.UploadFile, ct);
                    if (!saveRes.IsSuccess)
                    {
                        failCount++;
                        continue;
                    }

                    finalValue = saveRes.Path;
                }

                var res = await _svc.UpdateAsync(item.Id, new SystemSettingUpdateDto
                {
                    Value = finalValue,
                    IsActive = item.IsActive
                }, ct);

                if (res.IsSuccess) successCount++;
                else failCount++;
            }

            TempData[failCount == 0 ? "ok" : "err"] = failCount == 0
                ? $"{successCount} مورد با موفقیت ذخیره شد."
                : $"{successCount} مورد ذخیره شد و {failCount} مورد خطا داشت.";

            return RedirectToAction(nameof(Index), new { category });
        }

        private async Task<(bool IsSuccess, string? Path)> SaveUploadedSettingImageAsync(IFormFile file, CancellationToken ct)
        {
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            if (string.IsNullOrWhiteSpace(extension) || !allowed.Contains(extension))
                return (false, null);

            var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", "system-settings");
            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var physicalPath = Path.Combine(uploadsRoot, fileName);

            await using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream, ct);
            }

            return (true, $"/uploads/system-settings/{fileName}");
        }
    }
}
