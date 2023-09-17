using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LMS.Models;
using LMS.DataAccess;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace LMSWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] LoginModel loginUser)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(e => e.UserName == loginUser.UserName && e.Password == loginUser.Password);
                if (user == null)
                    return Redirect("Account"); // Invalid email or password.
                // Defining Cookie
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("username", loginUser.UserName));
                claims.Add(new Claim("password", loginUser.Password));
                claims.Add(new Claim("role", user.Role.ToString()));
                var claimsIdentity = new ClaimsIdentity(claims, "user");
                var principal = new ClaimsPrincipal(claimsIdentity);
                // Creating Cookie
                await HttpContext.SignInAsync("user", principal);

                return Redirect("Home");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Kayit()
        {
            return View();
        }

        
    }

    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}
