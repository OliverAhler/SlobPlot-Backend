using Application.Common.Interfaces;
using Application.Features.Auth;
using Application.Features.Genres;
using Application.Features.Statuses;
using Application.Features.Stories;
using Application.Features.UserProfiles;
using Application.IRepositories;
using Infrastructure.Persistence;
using Infrastructure.Identity.Queries;
using Infrastructure.Identity.Repositories;
using Infrastructure.Master.Queries;
using Infrastructure.Master.Repositories;
using Infrastructure.Services;
using Infrastructure.Stories.Queries;
using Infrastructure.Stories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedConfig;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, AppSettings config)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        
        //Master Queries
        services.AddScoped<IGenreQueries, GenreQueries>();
        services.AddScoped<IStatusQueries, StatusQueries>();
        
        //Master Commands
        services.AddScoped<IGenreRepository, GenreRepository>();
        
        //Identity Commands
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        
        //Identity Queries
        services.AddScoped<IUserQueries, UserQueries>();
        services.AddScoped<IUserProfileQueries, UserProfileQueries>();
        
        //Story Commands
        services.AddScoped<IStoryRepository, StoryRepository>();
        
        //Story Queries
        services.AddScoped<IStoryQueries, StoryQueries>();
                
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