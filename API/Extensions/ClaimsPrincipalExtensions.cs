using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetNickname(this ClaimsPrincipal user)
    {
        return user.FindFirst("nickname")?.Value;
    }
    
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var subClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(subClaim, out var userId) ? userId : null;
    }
}