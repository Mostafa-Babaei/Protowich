using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class ManageMenuController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}


