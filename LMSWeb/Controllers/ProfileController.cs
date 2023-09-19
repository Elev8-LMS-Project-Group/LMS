using LMS.DataAccess.Repository.IRepository;
using LMS.Models;
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
            int id = int.Parse(HttpContext.User.FindFirstValue("id"));
            var user = _unitOfWork.User.Get(e => e.UserId == id);

            UserVM vm = new UserVM();
            vm.UserId = user.UserId;
            vm.UserName = user.UserName;
            vm.Email = user.Email;
            vm.Password = user.Password;
            vm.Role = user.Role.ToString();
            return View(vm);
        }

        public IActionResult MyCourses()
        {
            int id = int.Parse(HttpContext.User.FindFirstValue("id"));
            var courseIds = _unitOfWork.Enrollment.GetAllWithExp(e => e.UserId == id).Select(e => e.CourseId);
            List<Course> courses = new List<Course>();
            foreach (var courseId in courseIds)
            {
                courses.Add(_unitOfWork.Course.Get(e => e.CourseId == courseId));
            }

            return View(courses);
        }
    }
}
