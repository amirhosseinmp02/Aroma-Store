using System;
using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Services;
using Aroma_Shop.Data.Context;
using Aroma_Shop.Data.Repositories;
using Aroma_Shop.Domain.Interfaces;
using Aroma_Shop.Domain.Models;
using Microsoft.Extensions.DependencyInjection;


namespace Aroma_Shop.Ioc
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccountService, AccountService>();
        }
    }
}
