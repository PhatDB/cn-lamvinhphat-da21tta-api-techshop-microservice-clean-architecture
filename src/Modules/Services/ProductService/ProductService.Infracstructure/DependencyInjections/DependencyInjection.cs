using System.Reflection;
using BuildingBlocks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Abtractions;
using ProductService.Domain.Events;
using ProductService.Infracstructure.EventHandler;

namespace ProductService.Infracstructure.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRabbitMq(configuration, Assembly.GetExecutingAssembly());
            services.AddScoped<INotificationHandler<ProductCreatedDomainEvent>, ProductCreatedDomainEventHandler>();
            services.AddScoped<IProductService, RequestClient.ProductService>();
            return services;
        }
    }
}