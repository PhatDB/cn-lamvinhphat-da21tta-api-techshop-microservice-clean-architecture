using System.Reflection;
using BuildingBlocks;
using CartService.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRabbitMq(configuration, Assembly.GetExecutingAssembly());
            services.AddScoped<ICartService, RequestClient.CartService>();
            return services;
        }
    }
}