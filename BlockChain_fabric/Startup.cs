using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using BL;
using BlockChain_fabric.Auth;
using MongoDb.Identity.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Converters;

namespace BlockChain_fabric
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDALServices();
            services.AddBLServices();
            services.AddScoped(typeof(IAuthenticateService), typeof(AuthenticateService));
            services.MongoIdentityService();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);//You can set Time   
            });

            services.AddAuthentication
                    (CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Login/";
                        options.SlidingExpiration = true;
                        options.Cookie.HttpOnly = true;
                        options.Cookie.SameSite = SameSiteMode.Strict;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                    });
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
