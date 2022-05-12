using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace operation_OLX.Controllers
{
    public class AccountController : Controller
    {
        private readonly SecurityServices _SecurityService;

        public static string CurrentUserName { get; set; }

        public static bool IsLoggedIn { get; set; }


        public AccountController(SecurityServices _SecurityService)
        {
            this._SecurityService= _SecurityService;
        }
        
        public IActionResult Login()
        {
            return View();
        }
       
       [HttpPost]
        public async Task<IActionResult> Login(string UserName, string Password)
        {
            var Isvalid = _SecurityService.ValidateUserAsync(new Account() { UserName=UserName,Password=Password}).Result;
            if (Isvalid)
            {
                CurrentUserName = UserName;
                IsLoggedIn = true;
                return RedirectToAction("Index", "User");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return View();
            }


        }
      
        public async Task<IActionResult> Logout()
        {
            CurrentUserName = String.Empty;
            IsLoggedIn = false;

            return RedirectToAction("Login", "Account");
        }
         public IActionResult Register()
        {
            var User = new RegisterUser();
            return View(User);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser newUser)
        {
            if (ModelState.IsValid)
            {
                var IsRegisterd = await _SecurityService.RegisterUser(newUser);
                if (IsRegisterd)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return View(newUser);
                }
            }
            else
            {
                return View(newUser);
            }
        }
       
        
        public IActionResult IsValidUserName(string Email)
        {
            return Json(_SecurityService.ValidateUserName(Email).Result);
        }
    }
}
