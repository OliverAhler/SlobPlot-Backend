using API.Contracts.Users;
using API.Extensions;
using Application.Common.Interfaces;
using Application.Features.Auth.Commands.SyncUser;
using Application.Features.Auth.Queries.GetUserBySub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IDispatcher dispatcher, ICurrentUserService currentUserService) : ControllerBase
{
    [HttpGet("user")]
    [Authorize]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var query = new GetUserByIdPSubQuery(currentUserService.GetUserId());
        
        var result = await dispatcher.Dispatch(query, cancellationToken);
        
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
        var result = await dispatcher.Dispatch(command, cancellationToken);
    
        return result.IsSuccess 
            ? Ok(new SyncUserResponse(result.Value))
            : BadRequest(result.Error); 
    }
}