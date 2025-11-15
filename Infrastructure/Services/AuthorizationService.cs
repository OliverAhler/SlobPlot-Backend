using Application.Common.Interfaces;
using Domain.Common;
using Domain.Common.Authorization;
using Domain.UserManagement.ValueObjects;

namespace Infrastructure.Services;

//USAGE:
// var result = await authorizationService.AuthorizeAndFetch<Story>(
//     ct => storyRepository.GetStoryById(storyId, ct), // How to fetch
//     story => story.AuthorId,                         // ← How to get the owner (lambda function)
//     AuthorizationPolicy.MustBeOwner,
//     cancellationToken
// );

public class AuthorizationService(ICurrentUserService currentUserService) : IAuthorizationService
{
    public async Task<Result<T>> AuthorizeAndFetch<T>(
        Func<CancellationToken, Task<T?>> fetchEntity,
        Func<T, UserId> getOwnerId,
        AuthorizationPolicy policy,
        CancellationToken cancellationToken) where T : class
    {
        var entity = await fetchEntity(cancellationToken);
        
        if (entity is null)
            return Result<T>.Failure($"{typeof(T).Name} not found");
        
        var authResult = await Authorize(entity, getOwnerId, policy, cancellationToken);
        
        if (!authResult.IsSuccess)
            return Result<T>.Failure(authResult.Error);
        
        return Result<T>.Success(entity);
    }
    
    public async Task<Result> Authorize<T>(
        T? entity,
        Func<T, UserId> getOwnerId,
        AuthorizationPolicy policy,
        CancellationToken cancellationToken) where T : class
    {
        if (entity is null)
            return Result.Failure($"{typeof(T).Name} not found");
        
        var currentUserId = await currentUserService.GetUserIdAsync(cancellationToken);
        
        var isAuthorized = policy switch
        {
            AuthorizationPolicy.Public =>
                true,
            AuthorizationPolicy.MustBeOwner =>
                getOwnerId(entity).Value == currentUserId,
            AuthorizationPolicy.MustBeOwnerOrModerator => 
                getOwnerId(entity).Value == currentUserId || await IsModeratorAsync(currentUserId, cancellationToken),
            AuthorizationPolicy.MustBeAdmin => 
                await IsAdminAsync(currentUserId, cancellationToken),
            AuthorizationPolicy.PublicOrOwner =>
                IsPublicEntity(entity) || getOwnerId(entity).Value == currentUserId,
            _ => false
        };
        
        if (!isAuthorized)
            return Result.Failure($"You do not have permission to access this {typeof(T).Name}");
        
        return Result.Success();
    }
    
    private bool IsPublicEntity<T>(T entity)
    {
        var isPublicProperty = entity?.GetType().GetProperty("IsPublic");
        return isPublicProperty?.GetValue(entity) as bool? ?? false;
    }
    
    private async Task<bool> IsModeratorAsync(Guid userId, CancellationToken cancellationToken)
    {
        // TODO: Implement role checking
        return await Task.FromResult(false);
    }
    
    private async Task<bool> IsAdminAsync(Guid userId, CancellationToken cancellationToken)
    {
        // TODO: Implement role checking
        return await Task.FromResult(false);
    }
}