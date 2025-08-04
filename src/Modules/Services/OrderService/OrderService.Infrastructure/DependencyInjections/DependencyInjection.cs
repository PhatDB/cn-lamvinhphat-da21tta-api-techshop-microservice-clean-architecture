using System.Reflection;
using BuildingBlocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Abstractions;
using OrderService.Infrastructure.EmailService;

namespace OrderService.Infrastructure.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddRabbitMq(configuration, Assembly.GetExecutingAssembly());
            services.AddScoped<IOrderService, RequestClient.OrderService>();
            services.AddSingleton<IPayOsService, PayOsService.PayOsService>();
            return services;
        }
    }
}