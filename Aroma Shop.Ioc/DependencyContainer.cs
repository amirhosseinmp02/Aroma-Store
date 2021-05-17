using System;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Security.Policy;
using Aroma_Shop.Application.Services;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Data.Repositories;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models;
using Aroma_Shop.Domain.Models.CustomIdentityModels;
using Aroma_Shop.Domain.Models.CustomIdentityModels.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace Aroma_Shop.Ioc
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
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
                    options.SignIn.RequireConfirmedEmail = false;
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
                    options.ClientId = "168190188156-bl2bbkcna0bgd8f8s2ih1mhps80dogbc.apps.googleusercontent.com";
                    options.ClientSecret = "5JhPg-OAJDEXQwm8v-fwbYrU";
                });

            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddHttpContextAccessor();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Founder", policy =>
                    policy.Requirements.Add(new PolicyRequirement("Founder")));
                options.AddPolicy("Manager", policy =>
                    policy.Requirements.Add(new PolicyRequirement("Manager")));
                options.AddPolicy("Writer", policy =>
                    policy.Requirements.Add(new PolicyRequirement("Writer")));
                options.AddPolicy("Customer", policy =>
                    policy.Requirements.Add(new PolicyRequirement("Customer")));
            });
            services.AddSingleton<IAuthorizationHandler, PolicyHandler>();
        }
    }
}
