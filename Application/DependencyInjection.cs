using Microsoft.Extensions.DependencyInjection;
using Venly.Dispatch;
using Venly.Dispatch.Enums;

namespace Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDispatch(options =>
        {
            options.CommandLogging = LoggingMode.All;
            options.QueryLogging = LoggingMode.OptIn;
        });
        // services.AddScoped<IDispatcher, Dispatcher>();
        //
        // services.Scan(scan => scan
        //     .FromAssemblyOf<IHandler>()
        //     .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
        //     .AsImplementedInterfaces()
        //     .WithScopedLifetime());
        //
        // services.Scan(scan => scan
        //     .FromAssemblyOf<IHandler>()
        //     .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
        //     .AsImplementedInterfaces()
        //     .WithScopedLifetime());
        //
        return services;
    }
}