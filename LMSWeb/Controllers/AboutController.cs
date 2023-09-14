using Microsoft.AspNetCore.Mvc;

namespace LMSWeb.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
