using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Panel.Controllers
{

    [AllowAnonymous]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}
