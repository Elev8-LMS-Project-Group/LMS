using LMS.DataAccess;
using LMS.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Show_Users()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
            .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var userList = dbContext.Users.ToList(); // Replace 'Users' with your actual table name
                return View(userList);
            }
        }


        [HttpPost]
        public IActionResult DeleteUser(int userId)
        {
            var user = _unitOfWork.User.Get(x => x.UserId == userId);
            if (user != null)
            {
                _unitOfWork.User.Remove(user);
                _unitOfWork.Save();
            }
            var userList = _unitOfWork.User.GetAll();
            return RedirectToAction("Show_Users", "Admin", new { userList = userList });

        }



        ////eklenen
        //public IActionResult DeleteUser(int id)
        //{
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //    .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
        //    .Options;
        //    using (var dbContext = new ApplicationDbContext(options))
        //    {
        //        var userToDelete = dbContext.Users.Find(id);

        //        if (userToDelete != null)
        //        {
        //            _unitOfWork.Enrollment.RemoveWithExp(e => e.UserId == userToDelete.UserId);
        //            _unitOfWork.Save();
        //            dbContext.Users.Remove(userToDelete);
        //            dbContext.SaveChanges();
        //        }
        //    }
        //    return RedirectToAction("Show_Users");
        //}





    }
}
