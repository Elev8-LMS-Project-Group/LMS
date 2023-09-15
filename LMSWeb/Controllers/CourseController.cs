using Microsoft.AspNetCore.Mvc;

namespace LMSWeb.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
