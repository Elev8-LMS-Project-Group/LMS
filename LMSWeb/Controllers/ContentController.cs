using LMS.Models.ViewModels;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using LMS.DataAccess.Repository.IRepository;

namespace LMSWeb.Controllers
{
    public class ContentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Upsert(int lessonId, int? id)
        {
            ContentVM contentVM = new()
            {
                Content = new Content()
            };
            if (id == null || id == 0)
            {
                contentVM.Content.LessonId = lessonId;
                contentVM.Content.Lesson = _unitOfWork.Lesson.Get(u => u.LessonId == contentVM.Content.LessonId);
                return View(contentVM);
            }
            else
            {
                //update
                contentVM.Content = _unitOfWork.Content.Get(u => u.ContentId == id);
                return View(contentVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ContentVM contentVM)
        {
            contentVM.Content.Lesson = _unitOfWork.Lesson.Get(u => u.LessonId == contentVM.Content.LessonId); //for now manually assigned at the view
            if (ModelState.IsValid) // User field is intentionally nullable for now can't solve the ModelState.IsValid - User field is required problem
            {

                if (contentVM.Content.ContentId == 0)
                {
                    _unitOfWork.Content.Add(contentVM.Content);
                }
                else
                {
                    //TO-DO implement Update in Content repo
                }

                _unitOfWork.Save();
                TempData["success"] = "Content created successfully";
                return RedirectToAction("Details", "Course", new { courseId = contentVM.Content.Lesson.CourseId });
            }
            else
            {
                return View(contentVM);
            }
        }
    }
}
