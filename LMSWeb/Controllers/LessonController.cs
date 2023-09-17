using LMS.DataAccess.Repository.IRepository;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using LMS.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace LMSWeb.Controllers
{
    public class LessonController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LessonController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult GetLesson(int lessonId)
        {
            var lesson = _unitOfWork.Lesson.Get(u => u.LessonId == lessonId);
            if (lesson != null)
            {
                lesson.Contents = _unitOfWork.Content.GetAllWithExp(u => u.LessonId == lessonId).ToList();
                var jsonOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                // Serialize the lesson and return it as JSON
                var lessonJson = JsonSerializer.Serialize(lesson, jsonOptions);
                return Content(lessonJson, "application/json");
            }
            else
            {
                return NotFound(); // Handle the case where the lesson is not found
            }

        }

        public IActionResult Upsert(int courseId, int? id)
        {
            LessonVM lessonVM = new()
            {
                Lesson = new Lesson()
            };
            if (id == null || id == 0)
            {
                lessonVM.Lesson.CourseId = courseId;
                return View(lessonVM);
            }
            else
            {
                //update
                lessonVM.Lesson = _unitOfWork.Lesson.Get(u => u.LessonId == id);
                return View(lessonVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(LessonVM lessonVM)
        {
            lessonVM.Lesson.Course = _unitOfWork.Course.Get(u => u.CourseId == lessonVM.Lesson.CourseId); //for now manually assigned at the view
            if (ModelState.IsValid) // User field is intentionally nullable for now can't solve the ModelState.IsValid - User field is required problem
            {
                
                if (lessonVM.Lesson.LessonId == 0)
                {
                    _unitOfWork.Lesson.Add(lessonVM.Lesson);
                }
                else
                {
                    //TO-DO implement Update in Lesson repo
                }

                _unitOfWork.Save();
                TempData["success"] = "Lesson created successfully";
                return RedirectToAction("Details", "Course", new { courseId = lessonVM.Lesson.CourseId });
            }
            else
            {
                return View(lessonVM);
            }
        }
    }
}
