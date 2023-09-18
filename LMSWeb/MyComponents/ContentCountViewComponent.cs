using LMS.DataAccess;
using Microsoft.AspNetCore.Mvc;
using LMS.Models;

namespace LMSWeb.MyComponents
{
    public class ContentCountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ContentCountViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Replace 'YourEntity' with the actual entity representing your content items
            int contentCount = _context.Contents.Count();

            // Pass the content count to the view component's view
            return View(contentCount);
        }
    }
}
