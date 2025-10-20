using Application.Features.Auth;
using Application.Features.Stories;
using Application.Features.UserProfiles;
using Application.IRepositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Repositories.Identity;
using Infrastructure.Persistence.Repositories.Stories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedConfig;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, AppSettings config)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        //Identity
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        
        //Stories
        services.AddScoped<IStoryRepository, StoryRepository>();
        
        services.AddDbContext<ApplicationDbContext>((options) =>
        {
            var dbSettings = config?.Database;
            if (string .IsNullOrEmpty(dbSettings?.ConnectionString))
                throw new InvalidOperationException("Database configuration is missing or invalid");

            options.UseNpgsql(dbSettings.ConnectionString);
        });
        
        return services;
    }
}
