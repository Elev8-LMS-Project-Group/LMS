using Microsoft.AspNetCore.Mvc;

namespace LMSWeb.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
