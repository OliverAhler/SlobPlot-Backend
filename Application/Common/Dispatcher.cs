using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common;

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public async Task<TResult> Dispatch<TResult>(ICommand<TResult> command)
    {
        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(command.GetType(), typeof(TResult));
        
        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        return await handler.Handle((dynamic)command);
    }
    
    public async Task<TResult> Dispatch<TResult>(IQuery<TResult> query)
    {
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResult));
        
        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        return await handler.Handle((dynamic)query);
    }
}