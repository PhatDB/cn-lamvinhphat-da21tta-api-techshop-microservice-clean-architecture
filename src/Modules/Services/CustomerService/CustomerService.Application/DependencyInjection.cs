﻿using CustomerService.Application.MappingProfiles;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
            services.AddAutoMapper(typeof(CustomerMappingProfile));
            return services;
        }
    }
}