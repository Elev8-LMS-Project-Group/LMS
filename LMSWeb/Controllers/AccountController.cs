using Microsoft.AspNetCore.Mvc;

namespace LMSWeb.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Kayit()
        {
            return View("Kayit");
        }
    }
}
