using LMS.DataAccess.Repository.IRepository;
using LMS.Models;
using LMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LMSWeb.Controllers
{
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CourseController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Course> courseList = _unitOfWork.Course.GetAll("User");
            return View(courseList);
        }

        //TO-DO
        public IActionResult Details(int courseId)
        {
            Course course = _unitOfWork.Course.Get(u => u.CourseId == courseId, includeProperties: "User");
            return View(course);
        }

        public IActionResult Edit(int courseId)
        {
            Course course = _unitOfWork.Course.Get(u => u.CourseId == courseId, includeProperties: "User,Lessons,Lessons.Contents");
            return View(course);
        }
        public IActionResult Upsert(int? id) 
        {
            CourseVM courseVM = new()
            {
                Course = new Course()
            };
            if(id == null || id == 0)
            {
                //create
                return View(courseVM);
            }
            else
            {
                //update
                courseVM.Course = _unitOfWork.Course.Get(u => u.CourseId == id);
                return View(courseVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(CourseVM courseVM, IFormFile? file)
        {
            courseVM.Course.User = _unitOfWork.User.Get(u => u.UserId == courseVM.Course.UserId); //for now manually assigned at the view
            if (ModelState.IsValid) // User field is intentionally nullable for now can't solve the ModelState.IsValid - User field is required problem
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string coursePath = Path.Combine(wwwRootPath, @"images\course");

                    if(!string.IsNullOrEmpty(courseVM.Course.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, courseVM.Course.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(coursePath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    courseVM.Course.ImageUrl = @"\images\course\" + fileName;
                }
                if(courseVM.Course.CourseId == 0)
                {
                    _unitOfWork.Course.Add(courseVM.Course);
                }
                else
                {
                    //TO-DO implement Update in course repo
                }

                _unitOfWork.Save();
                TempData["success"] = "Course created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(courseVM);
            }
        }

        public IActionResult Remove()
        {
            return View();
        }
    }
}
