using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models;

namespace Web.Areas.Admin.Controllers
{
    public class SystemSettingsController : AdminBaseController
    {
        private readonly ISystemSettingService _svc;

        public SystemSettingsController(ISystemSettingService svc)
        {
            _svc = svc;
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

                var res = await _svc.UpdateAsync(item.Id, new SystemSettingUpdateDto
                {
                    Value = item.Value,
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
    }
}
