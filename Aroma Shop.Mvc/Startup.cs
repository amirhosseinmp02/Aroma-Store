using Aroma_Shop.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Aroma_Shop.Domain.Models.CustomIdentityModels.Translations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aroma_Shop.Mvc
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
            services.AddControllersWithViews();
            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AromaAppDBConnection"));
            });
            services.AddIdentity<CustomIdentityUser, CustomIdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 0;
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<PersianIdentityErrorDescriber>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/LogOut";
            });
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "765998609051-j257eigqecb9oud7b6evtde9thuikul1.apps.googleusercontent.com";
                    options.ClientSecret = "MheK94NpxwDrmVQwBuFYfk9t";
                });
            RegisterServices(services);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                    context.Request.Path = "/ManageErrors/Error404";
                await next();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public static void RegisterServices(IServiceCollection service)
        {
            DependencyContainer.RegisterServices(service);
        }
    }
}
