using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Panel.Controllers
{
    //public class BaseController : Controller
    //{
    //    public override void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        //ViewBag.ApiBaseUrl = "https://api.paneldrnr.ir";
    //        ViewBag.ApiBaseUrl = "http://localhost:325";
    //        base.OnActionExecuting(context);
    //    }
    //}
    public class BaseController : Controller
    {
        private readonly IConfiguration _configuration;

        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.ApiBaseUrl = _configuration["ApiSettings:BaseUrl"];
            base.OnActionExecuting(context);
        }
    }
}
