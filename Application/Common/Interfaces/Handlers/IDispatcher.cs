
namespace Application.Common.Interfaces;

public interface IDispatcher
{
    Task<TResult> Dispatch<TResult>(ICommand<TResult> command);
    Task<TResult> Dispatch<TResult>(IQuery<TResult> query);
}