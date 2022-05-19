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
        public AccountController(SecurityServices _SecurityService)
        {
            this._SecurityService= _SecurityService;
        }
        
        public IActionResult Login()
        {
            return View(new Account());
        }
       
       [HttpPost]
        public async Task<IActionResult> Login(Account account)
        {
            var Isvalid =await _SecurityService.ValidateUserCredentialsAsync(account);
            if (Isvalid)
            {
                return RedirectToAction("Index", $"{SecurityServices.UserRole}");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return View();
            }
        }
      
        public IActionResult Logout()
        {
            SecurityServices.UserName = String.Empty;
            SecurityServices.IsLoogedIn = false;

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
            return Json(_SecurityService.CheckUserNameAvaliability(Email).Result);
        }
    }
}
