using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class ManageMenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
