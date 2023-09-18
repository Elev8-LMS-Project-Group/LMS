using LMS.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Show_Users() {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
            .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var userList = dbContext.Users.ToList(); // Replace 'Users' with your actual table name
                return View(userList);
            }
        }
    }
}
