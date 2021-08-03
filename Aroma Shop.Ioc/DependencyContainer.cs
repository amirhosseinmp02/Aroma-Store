using System;
using System.Linq;
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
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IMediaService, MediaService>();

            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFileService, FileService>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

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

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IPageService, PageService>();

            services.AddScoped<IVisitorRepository, VisitorRepository>();
            services.AddScoped<IVisitorService, VisitorService>();
        }
    }
}
