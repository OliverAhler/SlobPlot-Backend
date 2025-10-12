using Domain.IRepositories;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedConfig;

namespace Infrastructure;

public static class ConfigurationService
{
    public static IServiceCollection AddConfigureInfrastructure(this IServiceCollection services, AppSettings config)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        
        services.AddDbContext<ContextSlobPlot>((options) =>
        {
            var dbSettings = config?.Database;
            if (string .IsNullOrEmpty(dbSettings?.ConnectionString))
                throw new InvalidOperationException("Database configuration is missing or invalid");

            options.UseNpgsql(dbSettings.ConnectionString);
        });
        
        return services;
    }
}
