using System.Reflection;
using BuildingBlocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Abstractions;

namespace OrderService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRabbitMq(configuration, Assembly.GetExecutingAssembly());
            services.AddScoped<IOrderService, RequestClient.OrderService>();
            return services;
        }
    }
}