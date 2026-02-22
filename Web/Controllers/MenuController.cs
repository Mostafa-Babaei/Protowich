using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuPublicService _svc;

        public MenuController(IMenuPublicService svc)
        {
            _svc = svc;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var res = await _svc.GetPublicMenuAsync(ct);
            if (!res.IsSuccess)
                ViewBag.Error = res.Message;

            return View(res.Data ?? new List<PublicMenuCategoryDto>());
        }
    }
}
