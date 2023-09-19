using LMS.DataAccess.Repository.IRepository;
using LMS.Models;
using LMS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("Course/Index")]
        public IActionResult Index(int? instructorId)
        {
            IEnumerable<Course> courseList;
            if (instructorId != null)
            {
                courseList = _unitOfWork.Course.GetAllWithExp(c => c.UserId == instructorId, "User");
            }
            else
            {
                courseList = _unitOfWork.Course.GetAll("User");
            }
            
            return View(courseList);
        }
        
        public IActionResult EnrolledUsers(int courseId)
        {
            IEnumerable<Enrollment> enrList = _unitOfWork.Enrollment.GetAllWithExp(e => e.CourseId == courseId, "User");
            List<User> users = new List<User>();
            foreach (Enrollment enrollment in enrList)
            {
                users.Add(enrollment.User);
            }

            return View(users);
        }
        [HttpGet("Course/Search")]
        public IActionResult Index(string search)
        {
            // Retrieve the courses and apply filtering based on the search term
            IEnumerable<Course>? courses = null;

            if (!string.IsNullOrEmpty(search))
            {
                courses = _unitOfWork.Course.GetAllWithExp(course => course.Title.Contains(search), "User");
            }

            return View(courses);
        }
        
        public IActionResult Details(int courseId)
        {
            Course course = _unitOfWork.Course.Get(u => u.CourseId == courseId, includeProperties: "User");
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            var enrollment = _unitOfWork.Enrollment.Get(e => (e.UserId == userId) && e.CourseId == courseId);
            if (enrollment != null) ViewBag.IsEnrolled = true;


            return View(course);
        }

        public IActionResult Edit(int courseId)
        {
            Course course = _unitOfWork.Course.Get(u => u.CourseId == courseId, includeProperties: "User,Lessons,Lessons.Contents");
            return View(course);
        }
        //[Authorize(Roles = "Admin")]
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
            int userId = (Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id").Value));
            courseVM.Course.UserId = userId;
            courseVM.Course.User = _unitOfWork.User.Get(u => u.UserId == userId); //for now manually assigned at the view
            
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

        [HttpPost]
        public IActionResult Enroll(int userId, int courseId)
        {
            // Check if the user is already enrolled in the course
            var existingEnrollment = _unitOfWork.Enrollment.Get(e => e.UserId == userId && e.CourseId == courseId);

            if (existingEnrollment == null)
            {
                // User is not enrolled, create a new enrollment
                var enrollment = new Enrollment
                {
                    UserId = userId,
                    CourseId = courseId
                };

                _unitOfWork.Enrollment.Add(enrollment);
                _unitOfWork.Save();

                TempData["success"] = "Enrollment successful"; // Optionally set a success message
            }
            else
            {
                // User is already enrolled, handle accordingly (you can set a message or take other actions)
                TempData["info"] = "User is already enrolled in this course"; // Optionally set an info message
            }

            return RedirectToAction("Details", "Course", new { courseId = courseId });
        }

        public IActionResult Remove()
        {
            return View();
        }
    }
}
