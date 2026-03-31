using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models;

namespace Web.Areas.Admin.Controllers
{
    public class SubscriptionCustomersController : AdminBaseController
    {
        private readonly ISubscriptionCustomerService _svc;

        public SubscriptionCustomersController(ISubscriptionCustomerService svc)
        {
            _svc = svc;
        }

        public async Task<IActionResult> Index([FromQuery] SubscriptionCustomerIndexVm vm, CancellationToken ct)
        {
            vm.Page = vm.Page <= 0 ? 1 : vm.Page;
            vm.PageSize = vm.PageSize <= 0 ? 20 : vm.PageSize;

            var res = await _svc.GetPagedAsync(vm.Page, vm.PageSize, vm.Keyword, ct);
            if (!res.IsSuccess) TempData["err"] = res.Message;
            vm.Data = res.Data;
            return View(vm);
        }

        public IActionResult Create() => View(new SubscriptionCustomerUpsertDto { IsActive = true });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriptionCustomerUpsertDto dto, CancellationToken ct)
        {
            dto ??= new SubscriptionCustomerUpsertDto();
            var res = await _svc.CreateAsync(dto, ct);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, res.Message ?? "خطا در ثبت مشترک.");
                return View(dto);
            }

            TempData["ok"] = "مشترک با موفقیت ثبت شد.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            if (id <= 0) return NotFound();

            var entity = await _svc.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var vm = new SubscriptionCustomerFormVm
            {
                Id = id,
                Dto = new SubscriptionCustomerUpsertDto
                {
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Mobile = entity.Mobile,
                    Phone = entity.Phone,
                    Address = entity.Address,
                    Description = entity.Description,
                    SubscriptionCode = entity.SubscriptionCode,
                    IsActive = entity.IsActive
                }
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubscriptionCustomerFormVm vm, CancellationToken ct)
        {
            vm ??= new SubscriptionCustomerFormVm();
            vm.Dto ??= new SubscriptionCustomerUpsertDto();

            var res = await _svc.UpdateAsync(id, vm.Dto, ct);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, res.Message ?? "خطا در ویرایش مشترک.");
                vm.Id = id;
                return View(vm);
            }

            TempData["ok"] = "اطلاعات مشترک ویرایش شد.";
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

            var res = await _svc.SetActiveAsync(id, isActive.Value, ct);
            TempData[res.IsSuccess ? "ok" : "err"] = res.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
