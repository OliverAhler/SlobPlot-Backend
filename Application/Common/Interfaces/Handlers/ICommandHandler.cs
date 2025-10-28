using Application.Common.Interfaces.Handlers;

namespace Application.Common.Interfaces;

public interface ICommandHandler<in TCommand, TResult> : IHandler
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}