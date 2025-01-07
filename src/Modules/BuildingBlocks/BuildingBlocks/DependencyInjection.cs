using System.Reflection;
using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Extensions;
using BuildingBlocks.Extensions.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
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
                config.AddOpenBehavior(typeof(TransactionalPipelineBehavior<,>));
            });
            return services;
        }

        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration, Assembly? consumersAssembly = null)
        {
            RabbitMqOptions rabbitMqOptions = new();
            configuration.GetSection("MessageBroker").Bind(rabbitMqOptions);

            services.AddMassTransit(busConfigurator =>
            {
                if (consumersAssembly is not null) busConfigurator.AddConsumers(consumersAssembly);

                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(rabbitMqOptions.Host), host =>
                    {
                        host.Username(rabbitMqOptions.Username);
                        host.Password(rabbitMqOptions.Password);
                        host.RequestedConnectionTimeout(TimeSpan.FromSeconds(10));
                    });

                    configurator.ConfigureEndpoints(context);
                    configurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(15)));
                });
            });

            return services;
        }

        public static IServiceCollection AddGrpcService(this IServiceCollection services)
        {
            return services.AddScoped<IGrpcService, GrpcService>();
        }

        public static IServiceCollection AddFileServices(this IServiceCollection services)
        {
            services.AddGrpcService();
            services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}