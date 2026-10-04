using Domain.Common;
using Domain.Common.Authorization;
using Domain.UserManagement.ValueObjects;
using Vesia.Result;

namespace Application.Common.Interfaces;

public interface IAuthorizationService
{
    // Fetch + Authorize (when you don't have the entity yet)
    Task<Result<T>> AuthorizeAndFetch<T>(
        Func<CancellationToken, Task<T?>> fetchEntity,
        Func<T, UserId> getOwnerId,
        AuthorizationPolicy policy,
        CancellationToken cancellationToken) where T : class;
    
    // Just Authorize (when you already have the entity)
    Task<Result> Authorize<T>(
        T? entity,
        Func<T, UserId> getOwnerId,
        AuthorizationPolicy policy,
        CancellationToken cancellationToken) where T : class;
}