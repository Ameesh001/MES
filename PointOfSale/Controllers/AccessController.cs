using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PointOfSale.Business.Contracts;
using PointOfSale.Model;
using PointOfSale.Models;
using PointOfSale.Utilities.Logger;
using System.Security.Claims;

namespace PointOfSale.Controllers
{
    public class AccessController : Controller
    {
        //Codes
        private readonly IUserService _userService;
        Loggers log = new Loggers();



        public AccessController(IUserService userService)
        {
            _userService = userService;

        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            if (claimuser.Identity.IsAuthenticated)

                return RedirectToAction("Cheque", "Cheque");
            return View();
        }

        public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMUserLogin model)
        {
            try
            {

                User user_found = await _userService.GetByemail(model.Name);
                if (user_found == null)
                {
                    log.LogWriter("Login user_email not found");
                    ViewData["Message"] = "INVALID USER ID";
                    return View();
                }


                if (user_found.Password != model.PassWord)
                {
                    log.LogWriter("Login user_found not found UserDetail: " + model.Name + " ; kim" + Reverse(model.PassWord) + "carl");
                    ViewData["Message"] = "INVALID PASSWORD";
                    return View();
                }


                if ((bool)!user_found.IsActive)
                {
                    log.LogWriter("USER IS INACTIVE");
                    ViewData["Message"] = "USER IS INACTIVE";
                    return View();
                }

                log.LogWriter("Login user_found: " + user_found);

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user_found.Name),
                    new Claim(ClaimTypes.NameIdentifier, user_found.IdUsers.ToString()),
                    new Claim(ClaimTypes.Role,user_found.IdRol.ToString()),
                    new Claim("Email",user_found.Email),
                };


                HttpContext.Session.SetString("UserName", user_found.Name);
                HttpContext.Session.SetInt32("UserID", user_found.IdUsers);

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = model.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                log.LogWriter("Login Redirecting..");
                // return RedirectToAction("GateDashBoard", "Admin");

                return RedirectToAction("Cheque", "Cheque");
            }
            catch (Exception ex)
            {

                log.LogWriter("Login ex:" + ex);
                throw;
            }
            //return RedirectToAction("GatePass", "Setup");


        }
    }
}
