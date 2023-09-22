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
using LMS.Utility;

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
                //var user = _unitOfWork.User.Get(e => e.Email == loginUser.Email && e.Password == loginUser.Password);



                //if (user == null)
                //    return Redirect("Account"); // Invalid email or password.

                var user = _unitOfWork.User.Get(e => e.Email == loginUser.Email);
                if (EnDeCode.Decrypt(user.Password, StaticParams.SifrelemeParametresi) == loginUser.Password)
                { 
                    return Redirect("Account"); // Invalid email or password.
                }

                // Defining Cookies
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("id", user.UserId.ToString()));
                claims.Add(new Claim("name", user.Name));
                claims.Add(new Claim("email", user.Email));
                claims.Add(new Claim("password", user.Password));
                claims.Add(new Claim("role", user.Role.ToString()));
                var claimsIdentity = new ClaimsIdentity(claims, "user");
                var principal = new ClaimsPrincipal(claimsIdentity);
                // Creating Cookie
                await HttpContext.SignInAsync("user", principal);

                return Redirect("Home");
                //return View("Home",user);
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
                var user = _unitOfWork.User.Get(e => e.Name == loginUser.Name);
                if (user != null)
                {
                    return Redirect("Account"); // This user is already exists.
                }
                else
                {
                    if (ModelState.IsValid) // User field is intentionally nullable for now can't solve the ModelState.IsValid - User field is required problem
                    {
                        loginUser.Password = EnDeCode.Encrypt(loginUser.Password, StaticParams.SifrelemeParametresi);

                        _unitOfWork.User.Add(loginUser);
                        _unitOfWork.Save();
                        var newUser = _unitOfWork.User.Get(u => u.Name == loginUser.Name && u.Password == loginUser.Password);
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim("id", newUser.UserId.ToString()));
                        claims.Add(new Claim("name", newUser.Name));
                        claims.Add(new Claim("email", newUser.Email));
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
