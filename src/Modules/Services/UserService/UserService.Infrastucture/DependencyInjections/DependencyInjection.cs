using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using UserService.Application.Abtractions;
using UserService.Infrastucture.Authentications;
using UserService.Infrastucture.DependencyInjections.Options;
using UserService.Infrastucture.EmailServices;

namespace UserService.Infrastucture.DependencyInjections
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

            return services;
        }
    }
}