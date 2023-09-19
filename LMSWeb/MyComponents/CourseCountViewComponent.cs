using LMS.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace LMSWeb.MyComponents
{
    public class CourseCountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CourseCountViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Replace 'YourEntity' with the actual entity representing your content items
            int courseCount = _context.Courses.Count();

            // Pass the content count to the view component's view
            return View(courseCount);
        }
    }
}
