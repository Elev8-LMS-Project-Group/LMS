using LMS.DataAccess.Repository.IRepository;
using LMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace LMSWeb.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProfileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            UserVM vm = new UserVM();
            vm.UserId = int.Parse(HttpContext.User.FindFirstValue("id"));
            vm.UserName = HttpContext.User.FindFirstValue("username");
            vm.Password = HttpContext.User.FindFirstValue("password");
            vm.Role = HttpContext.User.FindFirstValue("role");
            return View(vm);
        }
    }
}
