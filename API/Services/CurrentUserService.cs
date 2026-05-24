using API.Extensions;
using Application.Common.Interfaces;
using Application.Features.Auth.Queries.GetUserIdBySubId;
using Microsoft.Extensions.Caching.Memory;
using Venly.Dispatch.Interfaces;


namespace API.Services;

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor, 
    IDispatcher dispatcher,
    IMemoryCache cache) : ICurrentUserService
{
    public async Task<Guid> GetUserIdAsync(CancellationToken cancellationToken)
    {
        var sub = httpContextAccessor.HttpContext?.User.GetSubId();
        
        if (sub == null)
            throw new UnauthorizedAccessException("Sub claim not found in token");

        // Check cache first
        var cacheKey = $"user_id_by_sub_{sub.Value}";
        
        if (cache.TryGetValue(cacheKey, out Guid cachedUserId))
            return cachedUserId;

        // Cache miss - query the database
        var query = new GetUserIdBySubIdQuery(sub.Value);
        var result = await dispatcher.DispatchAsync(query, cancellationToken);
        
        if (!result.IsSuccess)
            throw new UnauthorizedAccessException("User profile not found");
        
        // Cache for 1 hour
        cache.Set(cacheKey, result.Value, TimeSpan.FromHours(1));
        
        return result.Value;
    }
    
    public Guid GetSubId()
    {
        var userId = httpContextAccessor.HttpContext?.User.GetSubId();
        
        if (!userId.HasValue)
            throw new UnauthorizedAccessException("User ID not found in token");
        
        return userId.Value;
    }

    public bool IsAuthenticated => 
        httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}