using API.Extensions;
using Application.Common.Interfaces;

namespace API.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid GetUserId()
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();
        
        if (!userId.HasValue)
            throw new UnauthorizedAccessException("User ID not found in token");
        
        return userId.Value;
    }

    public bool IsAuthenticated => 
        httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}