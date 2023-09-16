using LMS.DataAccess.Repository.IRepository;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace LMSWeb.Controllers
{
    public class LessonController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public LessonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}
