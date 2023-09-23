using LMS.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace LMSWeb.MyComponents
{
    public class UserCountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public UserCountViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Replace 'YourEntity' with the actual entity representing your content items
            int userCount = _context.Users.Count();

            // Pass the content count to the view component's view
            return View(userCount);
        }
    }
}
