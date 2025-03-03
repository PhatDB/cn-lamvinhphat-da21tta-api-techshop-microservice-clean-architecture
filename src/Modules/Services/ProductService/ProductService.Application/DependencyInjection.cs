using BuildingBlocks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Mappings;

namespace ProductService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
                config.RegisterServicesFromAssembly(typeof(DependencyInjection)
                    .Assembly));
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly,
                includeInternalTypes: true);
            services.AddFileServices();
            services.AddAutoMapper(typeof(ProductMappingProfile));
            return services;
        }
    }
}