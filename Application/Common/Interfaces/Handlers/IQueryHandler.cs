using Application.Common.Interfaces.Handlers;

namespace Application.Common.Interfaces;

public interface IQueryHandler<in TQuery, TResult> : IHandler
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
}