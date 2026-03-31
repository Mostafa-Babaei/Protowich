using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IMenuPublicService _menuSvc;
        private readonly ISubscriptionCustomerService _subscriptionSvc;
        private readonly ISystemSettingService _settingSvc;

        public HomeController(
            IMenuPublicService menuSvc,
            ISubscriptionCustomerService subscriptionSvc,
            ISystemSettingService settingSvc)
        {
            _menuSvc = menuSvc;
            _subscriptionSvc = subscriptionSvc;
            _settingSvc = settingSvc;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var vm = new PublicHomeVm();

            var settingsRes = await _settingSvc.GetByCategoryAsync("Landing", ct);
            if (settingsRes.IsSuccess && settingsRes.Data != null)
            {
                vm.LandingSettings = settingsRes.Data
                    .Where(x => x.IsActive)
                    .ToDictionary(x => x.Key, x => x.Value ?? string.Empty);
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSubscription(
            [FromForm] string? fullName,
            [FromForm] string? mobile,
            [FromForm] string? address,
            [FromForm] string? phone,
            [FromForm] string? description,
            [FromForm] double? lat,
            [FromForm] double? lng,
            CancellationToken ct)
        {
            fullName = (fullName ?? string.Empty).Trim();
            mobile = (mobile ?? string.Empty).Trim();
            address = (address ?? string.Empty).Trim();
            phone = (phone ?? string.Empty).Trim();
            description = (description ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(fullName))
                return Json(new { isSuccess = false, message = "نام و نام خانوادگی الزامی است." });
            if (string.IsNullOrWhiteSpace(mobile))
                return Json(new { isSuccess = false, message = "موبایل الزامی است." });

            var parts = fullName
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => x.Length > 0)
                .ToList();

            var firstName = parts.Count > 0 ? parts[0] : fullName;
            var lastName = parts.Count > 1 ? string.Join(' ', parts.Skip(1)) : firstName;

            var finalDescription = description;
            if (lat.HasValue && lng.HasValue)
            {
                var loc = $"لوکیشن: {lat.Value:F6}, {lng.Value:F6}";
                finalDescription = string.IsNullOrWhiteSpace(finalDescription) ? loc : $"{finalDescription} | {loc}";
            }

            var dto = new SubscriptionCustomerUpsertDto
            {
                FirstName = firstName,
                LastName = lastName,
                Mobile = mobile,
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                Address = string.IsNullOrWhiteSpace(address) ? "ثبت نشده" : address,
                Description = string.IsNullOrWhiteSpace(finalDescription) ? null : finalDescription,
                IsActive = true
            };

            var res = await _subscriptionSvc.CreateAsync(dto, ct);
            return Json(new { isSuccess = res.IsSuccess, message = res.Message });
        }
    }
}
