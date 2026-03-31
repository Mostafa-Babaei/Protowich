using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuPublicService _svc;
        private readonly IMenuThemeService _themeSvc;

        public MenuController(IMenuPublicService svc, IMenuThemeService themeSvc)
        {
            _svc = svc;
            _themeSvc = themeSvc;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var vm = new PublicMenuPageVm
            {
                ActiveThemeKey = await _themeSvc.GetActiveThemeKeyAsync(ct)
            };

            var res = await _svc.GetPublicMenuAsync(ct);
            if (!res.IsSuccess)
                vm.Error = res.Message;

            vm.Categories = res.Data ?? new List<PublicMenuCategoryDto>();
            return View(vm);
        }
    }
}
