using BlockChain_fabric.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDb.Identity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockChain_fabric.Auth
{
    public interface IAuthenticateService
    {
        Task<AuthenticatedModel> Auth(LoginVIewModel login);

        Task<AuthenticatedModel> Register(RegisterViewModel register);
        Task Inialize();
    }
    public class AuthenticateService : IAuthenticateService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AuthenticateService> _logger;
        public AuthenticateService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AuthenticateService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<AuthenticatedModel> Auth(LoginVIewModel login)
        {
            var res = await _signInManager.PasswordSignInAsync(login.uname, login.password, login.RememberMe, lockoutOnFailure: true);
            var Am = new AuthenticatedModel();
            Am.sign = res;
            if (res.Succeeded)
            {
                var usr = await _userManager.FindByNameAsync(login.uname.ToUpper());
                Am.name = usr.Name;
                Am.ID = usr.Id.ToString();
                Am.uname = usr.UserName;
                Am.role = usr.IsAdmin ? "Admin" : "User";
            }
            return Am;
        }

        public async Task Inialize()
        {
            try
            {
                var usr = await _userManager.FindByNameAsync("ADMIN");
                if (usr == null)
                {
                    await _userManager.CreateAsync(new ApplicationUser()
                    {
                        IsAdmin = true,
                        Email = "admin@gmail.com",
                        Name = "Admin",
                        UserName = "admin",

                    }, "123456");
                }
            }
            catch
            {
                await _userManager.CreateAsync(new ApplicationUser()
                {
                    IsAdmin = true,
                    Email = "admin@gmail.com",
                    Name = "Admin",
                    UserName = "admin",

                }, "123456");
            }

        }

        public async Task<AuthenticatedModel> Register(RegisterViewModel register)
        {
            var usr = _userManager.Users.Where(x => x.UserName == register.emailId).FirstOrDefault();
            if (usr == null)
            {
                await _userManager.CreateAsync(new ApplicationUser()
                {
                    IsAdmin = false,
                    Email = register.emailId,
                    UserName = register.emailId,
                    Name = register.name,
                    age=register.age,
                    gender=register.gender,
                    PhoneNumber = register.phoneNo
                }, register.password);

                var res = await _signInManager.PasswordSignInAsync(register.emailId, register.password, false, lockoutOnFailure: true);
                if (res.Succeeded)
                {
                    var u = _userManager.Users.Where(x => x.UserName == register.emailId).FirstOrDefault();
                    return new AuthenticatedModel()
                    {
                        ID = u.Id.ToString(),
                        name = u.Name,
                        role = "User",
                        sign = res,
                        uname = u.UserName
                    };
                }
                return new AuthenticatedModel()
                {
                    sign = res
                };
            }
            return null;

        }
    }
}
