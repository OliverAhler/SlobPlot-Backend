using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using SharedConfig;

namespace API.Extensions;

public class InternalGatewayOnlyAttribute() : TypeFilterAttribute(typeof(InternalGatewayFilter));

public class InternalGatewayFilter(IOptions<AppSettings> settings) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var secretKey = settings.Value.Gateway.SecretKey;
        
        if (!context.HttpContext.Request.Headers.TryGetValue("X-Internal-Gateway", out var headerValue) 
            || headerValue != secretKey)
        {
            context.Result = new UnauthorizedObjectResult("Unauthorized");
        }
    }
}
