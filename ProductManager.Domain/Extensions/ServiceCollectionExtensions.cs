using Microsoft.Extensions.DependencyInjection;
using ProductManager.Domain.Interfaces;
using ProductManager.Domain.Services;

namespace ProductManager.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
