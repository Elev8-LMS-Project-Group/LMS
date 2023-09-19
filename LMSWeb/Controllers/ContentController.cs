using LMS.Models.ViewModels;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using LMS.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;

namespace LMSWeb.Controllers
{
    public class ContentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ContentController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult Upsert(ContentVM contentVM, IFormFile? file)
        {
            contentVM.Content.Lesson = _unitOfWork.Lesson.Get(u => u.LessonId == contentVM.Content.LessonId); //for now manually assigned at the view
            if (ModelState.IsValid) // User field is intentionally nullable for now can't solve the ModelState.IsValid - User field is required problem
            {

                if (contentVM.Content.ContentId == 0)
                {
                    if (contentVM.Content.ContentType == ContentType.Image)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        if (file != null)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string coursePath = Path.Combine(wwwRootPath, @"images\content");

                            if (!string.IsNullOrEmpty(contentVM.Content.ImageUrl))
                            {
                                var oldImagePath = Path.Combine(wwwRootPath, contentVM.Content.ImageUrl.TrimStart('\\'));
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                            }

                            using (var fileStream = new FileStream(Path.Combine(coursePath, fileName), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            contentVM.Content.ImageUrl = @"\images\content\" + fileName;
                        }
                    }
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
