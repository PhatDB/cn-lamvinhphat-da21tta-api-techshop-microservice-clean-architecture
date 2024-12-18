using BuildingBlocks.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBuildingBlocks(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
                config.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
            });
            return services;
        }
    }
}