using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LMS.Models;
using LMS.DataAccess;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using LMS.DataAccess.Repository.IRepository;
using LMS.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace LMSWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] User loginUser)
        {
            try
            {
                var user = _unitOfWork.User.Get(e => e.UserName == loginUser.UserName && e.Password == loginUser.Password);
                if (user == null)
                    return Redirect("Account"); // Invalid username or password.
                // Defining Cookies
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("id", user.UserId.ToString()));
                claims.Add(new Claim("username", user.UserName));
                claims.Add(new Claim("password", user.Password));
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

        [HttpPost]
        public async Task<IActionResult> Kayit([FromForm] User loginUser)
        {
            try
            {
                var user = _unitOfWork.User.Get(e => e.UserName == loginUser.UserName);
                if (user != null)
                {
                    return View("Kayit"); // This user is already exists.
                }
                else
                {
                    if (ModelState.IsValid) // User field is intentionally nullable for now can't solve the ModelState.IsValid - User field is required problem
                    {
                        _unitOfWork.User.Add(loginUser);
                        _unitOfWork.Save();
                        var newUser = _unitOfWork.User.Get(u => u.UserName == loginUser.UserName && u.Password == loginUser.Password);
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim("id", newUser.UserId.ToString()));
                        claims.Add(new Claim("username", newUser.UserName));
                        claims.Add(new Claim("password", newUser.Password));
                        claims.Add(new Claim("role", newUser.Role.ToString()));
                        var claimsIdentity = new ClaimsIdentity(claims, "user");
                        var principal = new ClaimsPrincipal(claimsIdentity);
                        // Creating Cookie
                        await HttpContext.SignInAsync("user", principal);
                        TempData["success"] = "User created successfully";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return View("Kayit");
                    }
                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Logout()
        {
            if(User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }  
    }
}
