using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.FastFoodInterface;
using Web.Models;

namespace Web.Controllers
{

    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IMenuPublicService _menuSvc;

        public HomeController(IMenuPublicService menuSvc)
        {
            _menuSvc = menuSvc;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var vm = new PublicHomeVm();

            var res = await _menuSvc.GetPublicMenuAsync(ct);
            if (res.IsSuccess && res.Data != null)
            {
                vm.Categories = res.Data;
                vm.CategoriesCount = vm.Categories.Count;
                vm.ItemsCount = vm.Categories.Sum(c => c.Items?.Count ?? 0);
                vm.ImagesCount = vm.Categories
                    .SelectMany(c => c.Items ?? new())
                    .Sum(i => i.ImageUrls?.Count ?? 0);

                vm.FeaturedItems = vm.Categories
                    .OrderBy(c => c.DisplayOrder)
                    .SelectMany(c => (c.Items ?? new()).OrderBy(i => i.DisplayOrder))
                    .Take(6)
                    .ToList();
            }
            else
            {
                vm.Error = res.Message;
            }

            return View(vm);
        }

    }
}
