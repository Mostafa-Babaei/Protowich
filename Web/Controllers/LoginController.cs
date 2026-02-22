using Application.Features.Auth.DTOs;
using Application.Interfaces.AuthenticationInterface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Panel.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IUserRepository _auth;

        public LoginController(IUserRepository auth)
        {
            _auth = auth;
        }

        [HttpGet]
        public IActionResult Index(string? returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // اگر companyId برای ادمین مهم نیست، بهتره GenerateJwtToken هم companyId خالی رو هندل کنه
            // فعلاً همون Guid.Empty که گفتی:
            var result = await _auth.LoginAsync(vm.Username, vm.Password);

            if (result == null || !result.IsSuccess)
            {
                ModelState.AddModelError("", result?.Message ?? "Login failed.");
                return View(vm);
            }

            // result.Data => object (anonymous) { access_token, refresh_token }
            // با JsonElement / Dictionary می‌کشیم بیرون
            string? accessToken = null;
            string? refreshToken = null;

            if (result.Data is JsonElement je && je.ValueKind == JsonValueKind.Object)
            {
                if (je.TryGetProperty("access_token", out var at)) accessToken = at.GetString();
                if (je.TryGetProperty("refresh_token", out var rt)) refreshToken = rt.GetString();
            }
            else
            {
                // fallback: serialize/deserialize
                var json = JsonSerializer.Serialize(result.Data);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("access_token", out var at)) accessToken = at.GetString();
                if (root.TryGetProperty("refresh_token", out var rt)) refreshToken = rt.GetString();
            }

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                ModelState.AddModelError("", "Token generation failed.");
                return View(vm);
            }

            // ✅ اگر دوست داری از روی JWT claim ها رو بخونی:
            // var handler = new JwtSecurityTokenHandler();
            // var jwt = handler.ReadJwtToken(accessToken);
            // var userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // فعلاً چون می‌خوای فقط Owner وارد بشه، یک Claim Owner می‌گذاریم
            // (اگر userId/email رو هم می‌خوای، بهتره LoginInternalAsync علاوه بر token، user info هم برگردونه)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, vm.Username),
                new Claim("IsOwner", "true"),
                new Claim("access_token", accessToken) // اختیاری (بهتره تو Session ذخیره بشه نه Claim)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(12)
                });

            // ✅ ذخیره توکن‌ها (پیشنهادی: Session)
            // لازمه AddSession() و UseSession() رو در Program.cs فعال کرده باشی
            HttpContext.Session.SetString("access_token", accessToken);
            if (!string.IsNullOrWhiteSpace(refreshToken))
                HttpContext.Session.SetString("refresh_token", refreshToken);

            // یا اگر Session نداری، می‌تونی Cookie HttpOnly بذاری:
            // Response.Cookies.Append("access_token", accessToken, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Lax });

            if (!string.IsNullOrWhiteSpace(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                return Redirect(vm.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
