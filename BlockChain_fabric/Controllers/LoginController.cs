using BlockChain_fabric.Auth;
using BlockChain_fabric.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlockChain_fabric.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticateService _authenticate;
        public LoginController(IAuthenticateService authenticate)
        {
            _authenticate = authenticate;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            await _authenticate.Inialize();
            ViewBag.err = "";
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(LoginVIewModel _user, string returnUrl)
        {

            var usr = await _authenticate.Auth(_user);
            if (usr.sign.Succeeded)
            {
                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Name, usr.ID));
                claims.Add(new Claim(ClaimTypes.Surname, usr.name));
                claims.Add(new Claim(ClaimTypes.Role, usr.role));
                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.
        AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var props = new AuthenticationProperties();
                props.IsPersistent = _user.RememberMe;

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.
        AuthenticationScheme,
                    principal, props).Wait();

                return RedirectToAction("", "Home");
            }
            else if (usr.sign.IsLockedOut)
            {
                ViewBag.err = "User Locked Out for Multiple Wrong Attempts";
                return View();
            }
            else
            {
                ViewBag.err = "User Name / Password Error";
                return View();
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            await _authenticate.Inialize();
            ViewBag.BloodGroup = new List<string>()
            {"A+","A-","B+","B-","O+","O-","AB+","AB-"

            };
            ViewBag.gender = new List<string>()
            {
                "Male","Female","Cross Gender"
            };
            ViewBag.err = "";
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel _user, string returnUrl)
        {
            ViewBag.gender = new List<string>()
            {
                "Male","Female","Cross Gender"
            };
            var usr = await _authenticate.Register(_user);
            if (usr == null)
            {
                ViewBag.err = "Email ID Already Exists";
                return View();
            }
            if (usr.sign.Succeeded)
            {
                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Name, usr.ID));
                claims.Add(new Claim(ClaimTypes.Surname, usr.name));
                claims.Add(new Claim(ClaimTypes.Role, usr.role));
                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.
        AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var props = new AuthenticationProperties();
                props.IsPersistent = false;

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.
        AuthenticationScheme,
                    principal, props).Wait();

                return RedirectToAction("", "Home");
            }
            else if (usr.sign.IsLockedOut)
            {
                ViewBag.err = "User Locked Out for Multiple Wrong Attempts";
                return View();
            }
            else
            {
                ViewBag.err = "User Name / Password Error";
                return View();
            }
        }

        [AllowAnonymous]
        public IActionResult signout()
        {
            HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("", "Login");
        }
    }
}
