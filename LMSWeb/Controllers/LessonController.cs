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
                var curUserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id").Value);
                lesson.UserLessonProgresses = _unitOfWork.UserLessonProgress.GetAllWithExp(u => u.LessonId == lessonId && u.UserId == curUserId).ToList();
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
                    _unitOfWork.Save();
                    int newAddedLessonId = lessonVM.Lesson.LessonId;
                    var enrolledUsers = _unitOfWork.User.GetAllWithExp(u => u.Enrollments.Any(e => e.CourseId == lessonVM.Lesson.CourseId));
                    foreach (var user in enrolledUsers)
                    {
                        UserLessonProgress ulProgress = new UserLessonProgress()
                        {
                            UserId = user.UserId,
                            LessonId = newAddedLessonId,
                            IsCompleted = false
                        };
                        _unitOfWork.UserLessonProgress.Add(ulProgress);
                        _unitOfWork.Save();
                    }
                    //Bu kurstaki bu ders için her enrollment başına userlessonprogress oluşturup iscompletedi false çek.
                }
                else
                {
                    //TO-DO implement Update in Lesson repo
                }


                TempData["success"] = "Lesson created successfully";
                return RedirectToAction("Details", "Course", new { courseId = lessonVM.Lesson.CourseId });
            }
            else
            {
                return View(lessonVM);
            }
        }

        public IActionResult Delete(int lessonId)
        {
            LessonVM lessonVM = new()
            {
                Lesson = _unitOfWork.Lesson.Get(u => u.LessonId == lessonId)
            };
            return View(lessonVM);
        }

        [HttpPost]
        public IActionResult Delete(LessonVM lessonVM)
        {
            if (ModelState.IsValid) // User field is intentionally nullable for now can't solve the ModelState.IsValid - User field is required problem
            {
                var ulProgresses = _unitOfWork.UserLessonProgress.GetAllWithExp(ulp => ulp.LessonId == lessonVM.Lesson.LessonId);
                foreach (var ulp in ulProgresses)
                {
                    _unitOfWork.UserLessonProgress.Remove(ulp);
                }
                _unitOfWork.Lesson.Remove(lessonVM.Lesson);
                _unitOfWork.Save();

                TempData["success"] = "Lesson removed successfully";
                return RedirectToAction("Details", "Course", new { courseId = lessonVM.Lesson.CourseId });
            }
            else
            {
                return View(lessonVM);
            }
        }

        [HttpPost]
        public IActionResult CompleteLesson(int lessonId)
        {
            try
            {
                // Retrieve the lesson from the database
                var lesson = _unitOfWork.Lesson.Get(l => l.LessonId == lessonId);

                if (lesson == null)
                {
                    return NotFound(); // Lesson not found
                }

                // Check if the user has already completed the lesson
                var curUserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id").Value);
                var user = _unitOfWork.User.Get(u => u.UserId == curUserId); // Implement GetUserId() to get the current user's ID
                if (user == null)
                {
                    return Unauthorized(); // User not authenticated
                }

                var userLessonProgress = _unitOfWork.UserLessonProgress.Get(ulp =>
                    ulp.LessonId == lessonId && ulp.UserId == user.UserId);

                if (userLessonProgress != null && userLessonProgress.IsCompleted)
                {
                    return Conflict(); // Lesson already completed by the user
                }

                // Mark the lesson as completed for the user
                if (userLessonProgress == null)
                {
                    userLessonProgress = new UserLessonProgress
                    {
                        UserId = user.UserId,
                        LessonId = lessonId,
                        IsCompleted = true
                    };
                    _unitOfWork.UserLessonProgress.Add(userLessonProgress);
                }
                else
                {
                    userLessonProgress.IsCompleted = true;
                    _unitOfWork.UserLessonProgress.Update(userLessonProgress);
                }

                _unitOfWork.Save(); // Save changes to the database

                return Ok(); // Lesson marked as completed successfully
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500); // Internal server error
            }
        }





        //private readonly IUnitOfWork _unitOfWork;
        //private readonly IWebHostEnvironment _webHostEnvironment;
        //public LessonController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        //{
        //    _unitOfWork = unitOfWork;
        //    _webHostEnvironment = webHostEnvironment;
        //}
        //public IActionResult GetLesson(int lessonId)
        //{
        //    var lesson = _unitOfWork.Lesson.Get(u => u.LessonId == lessonId);
        //    if (lesson != null)
        //    {
        //        lesson.Contents = _unitOfWork.Content.GetAllWithExp(u => u.LessonId == lessonId).ToList();
        //        var jsonOptions = new JsonSerializerOptions
        //        {
        //            ReferenceHandler = ReferenceHandler.Preserve
        //        };

        //        // Serialize the lesson and return it as JSON
        //        var lessonJson = JsonSerializer.Serialize(lesson, jsonOptions);
        //        return Content(lessonJson, "application/json");
        //    }
        //    else
        //    {
        //        return NotFound(); // Handle the case where the lesson is not found
        //    }

        //}

        //public IActionResult Upsert(int courseId, int? id)
        //{
        //    LessonVM lessonVM = new()
        //    {
        //        Lesson = new Lesson()
        //    };
        //    if (id == null || id == 0)
        //    {
        //        lessonVM.Lesson.CourseId = courseId;
        //        return View(lessonVM);
        //    }
        //    else
        //    {
        //        //update
        //        lessonVM.Lesson = _unitOfWork.Lesson.Get(u => u.LessonId == id);
        //        return View(lessonVM);
        //    }
        //}

        //[HttpPost]
        //public IActionResult Upsert(LessonVM lessonVM)
        //{
        //    lessonVM.Lesson.Course = _unitOfWork.Course.Get(u => u.CourseId == lessonVM.Lesson.CourseId); //for now manually assigned at the view
        //    if (ModelState.IsValid) // User field is intentionally nullable for now can't solve the ModelState.IsValid - User field is required problem
        //    {

        //        if (lessonVM.Lesson.LessonId == 0)
        //        {
        //            _unitOfWork.Lesson.Add(lessonVM.Lesson);
        //        }
        //        else
        //        {
        //            //TO-DO implement Update in Lesson repo
        //        }

        //        _unitOfWork.Save();
        //        TempData["success"] = "Lesson created successfully";
        //        return RedirectToAction("Details", "Course", new { courseId = lessonVM.Lesson.CourseId });
        //    }
        //    else
        //    {
        //        return View(lessonVM);
        //    }
        //}
    }
}
