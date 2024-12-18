using BuildingBlocks.Abstractions.Repository;
using CatalogService.Domain.Abstractions.Repositories;
using CatalogService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Persistence.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("Database");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}