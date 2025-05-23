using System.Reflection;
using BuildingBlocks;
using CustomerService.Application.Abtractions;
using CustomerService.Infrastucture.Authentications;
using CustomerService.Infrastucture.DependencyInjections.Options;
using CustomerService.Infrastucture.EmailService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CustomerService.Infrastucture.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                RedisOption redisOption = new();
                configuration.GetSection(nameof(RedisOption)).Bind(redisOption);
                return ConnectionMultiplexer.Connect(redisOption.ConnectionString);
            });

            services.AddHttpContextAccessor();
            services.AddTransient<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddRabbitMq(configuration, Assembly.GetExecutingAssembly());
            return services;
        }
    }
}