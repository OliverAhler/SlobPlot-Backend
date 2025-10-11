using System.Security.Claims;
using API.Extensions;
using Application.Common.Interfaces;
using Application.Features.Auth.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserProfileController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        
        if (!userId.HasValue)
            return Unauthorized("Invalid or missing user ID in token");
        
        var username = User.GetNickname();
        var query = new GetUserByUidQuery(userId.Value);
        
        var result = await dispatcher.Dispatch(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }
}