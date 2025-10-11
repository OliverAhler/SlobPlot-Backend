using Application.Common;
using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigurationService
{
    public static IServiceCollection AddConfigureApplication(this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, Dispatcher>();
        
        services.Scan(scan => scan
            .FromAssemblyOf<IHandler>()
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<IHandler>()
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}