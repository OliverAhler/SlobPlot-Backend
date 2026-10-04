using Microsoft.Extensions.DependencyInjection;
using Vesia.Dispatch;

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
        
        return services;
    }
}