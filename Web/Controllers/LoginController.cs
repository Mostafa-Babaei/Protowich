using Application.Features.Auth.DTOs;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Panel.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly AppDbContext _db;

        public LoginController(AppDbContext db)
        {
            _db = db;
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

            var username = (vm.Username ?? string.Empty).Trim();
            var password = vm.Password ?? string.Empty;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "نام کاربری و رمز عبور الزامی است.");
                return View(vm);
            }

            var inputHash = Convert.ToBase64String(
                SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));

            var user = await _db.Users.FirstOrDefaultAsync(
                x => !x.IsDeleted && x.IsActive && (x.UserName == username || x.Email == username),
                HttpContext.RequestAborted);

            if (user == null || user.Password != inputHash)
            {
                ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور اشتباه است.");
                return View(vm);
            }

            user.LastLogin = DateTime.Now;
            await _db.SaveChangesAsync(HttpContext.RequestAborted);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? username),
                new Claim("IsOwner", "true")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false
                });

            if (!string.IsNullOrWhiteSpace(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                return Redirect(vm.ReturnUrl);

            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
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
