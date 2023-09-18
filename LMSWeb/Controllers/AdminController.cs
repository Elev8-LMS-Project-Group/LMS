using Microsoft.AspNetCore.Mvc;

namespace LMSWeb.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Show_Users() {
            return View();
        }
    }
}
