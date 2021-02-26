using Aroma_Shop.Application.Interfaces;
using Aroma_Shop.Application.Services;
using Aroma_Shop.Data.Repositories;
using Aroma_Shop.Domain.Interfaces;
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
        }
    }
}
