using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDb.Identity.Core.Identity;
using MongoDb.Identity.Core.Identity.Stores;
using MongoDb.Identity.Core.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb.Identity.Core
{
    public static class StartUpExtentions
    {
        public static IServiceCollection MongoIdentityService(this IServiceCollection services)
        {
            services.AddTransient<MongoTablesFactory>();
            var lockoutOptions = new LockoutOptions()
            {
                AllowedForNewUsers = true,
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10),
                MaxFailedAccessAttempts = 3
            };
            services.AddDefaultIdentity<ApplicationUser>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Lockout = lockoutOptions;
                opt.SignIn.RequireConfirmedAccount = false;
                opt.User.RequireUniqueEmail = true;
            })
            .AddRoles<ApplicationRole>()
            .AddDefaultTokenProviders()
            .AddRoleStore<MongoRoleStore<ApplicationRole, ObjectId>>()
            .AddUserStore<MongoUserStore<ApplicationUser, ObjectId>>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,
                AdditionalUserClaimsPrincipalFactory>();

            return services;
        }
    }
}
