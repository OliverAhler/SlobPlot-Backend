using API.Contracts.Users.SyncUser;
using API.Extensions;
using Application.Common.Interfaces;
using Application.Features.Auth.Commands.SyncUser;
using Application.Features.Auth.Queries.GetUserBySub;
using Application.Features.Stories.Queries.GetStoriesByAuthor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Venly.Dispatch.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IDispatcher dispatcher, ICurrentUserService currentUserService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var query = new GetUserByIdPSubQuery(currentUserService.GetSubId());
        
        var result = await dispatcher.DispatchAsync(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }
    
    [HttpPost("sync")]
    [InternalGatewayOnly]
    public async Task<IActionResult> SyncUser([FromBody] SyncUserRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.SubUid, out var subUid))
            return BadRequest("Invalid SubUid format");

        var command = new SyncUserCommand(subUid, request.UserName);
        var result = await dispatcher.DispatchAsync(command, cancellationToken);
    
        return result.IsSuccess 
            ? Ok(new SyncUserResponse(result.Value))
            : BadRequest(result.Error); 
    }
    
    [HttpGet]
    [Route("{userid:guid}/stories")]
    public async Task<IActionResult> GetUserStories([FromRoute] Guid userid, CancellationToken cancellationToken)
    {
        var query = new GetStoriesByAuthorQuery(userid);
        
        var result = await dispatcher.DispatchAsync(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }
}