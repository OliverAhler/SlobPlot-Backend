
namespace Application.Common.Interfaces.Handlers;

public interface IDispatcher
{
    Task<TResult> Dispatch<TResult>(ICommand<TResult> command, CancellationToken cancellationToken);
    Task<TResult> Dispatch<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
}