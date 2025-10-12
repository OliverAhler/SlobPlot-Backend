using API.Contracts.Users;
using API.Extensions;
using Application.Common.Interfaces;
using Application.Features.Auth.Commands;
using Application.Features.Auth.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SharedConfig;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IDispatcher dispatcher, IOptions<AppSettings> config) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        
        if (!userId.HasValue)
            return Unauthorized("Invalid or missing user ID in token");
        
        var username = User.GetNickname();
        var query = new GetUserByIdPSubQuery(userId.Value);
        
        var result = await dispatcher.Dispatch(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }
    
    [HttpPost]
    [Route("sync")]
    public async Task<IActionResult> SyncUser([FromBody] SyncUserRequest? request, CancellationToken cancellationToken)
    {
        if (!Request.Headers.TryGetValue("X-Internal-Gateway", out var headerValue) || headerValue != config.Value.Gateway.SecretKey)
            return Unauthorized("This endpoint is only accessible from the gateway");
        
        if (request is null)
            return BadRequest("Request body is required");

        if (!Guid.TryParse(request.SubUid, out var subUid))
            return BadRequest("Invalid SubUid format");
    
        if (string.IsNullOrWhiteSpace(request.UserName))
            return BadRequest("UserName is required");
    
        var command = new SyncUserCommand(subUid, request.UserName);
        var result = await dispatcher.Dispatch(command, cancellationToken);
        
        if(!result.IsSuccess)
            return BadRequest(result.Error);

        var response = new SyncUserResponse(isNewUser: result.Value);
        return Ok(response);
    }
}