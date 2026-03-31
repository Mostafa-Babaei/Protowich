using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Models;

namespace Web.Areas.Admin.Controllers
{
    public class ManageMenuController : AdminBaseController
    {
        private readonly IMenuThemeService _themeSvc;

        public ManageMenuController(IMenuThemeService themeSvc)
        {
            _themeSvc = themeSvc;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var vm = new MenuThemeSelectionVm
            {
                ActiveThemeKey = await _themeSvc.GetActiveThemeKeyAsync(ct),
                Options = (await _themeSvc.GetThemeOptionsAsync(ct)).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetTheme([FromForm] string? themeKey, CancellationToken ct)
        {
            var res = await _themeSvc.SetActiveThemeAsync(themeKey ?? string.Empty, ct);
            TempData[res.IsSuccess ? "ok" : "err"] = res.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
