using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Features.Stories.Queries.GetStoriesByAuthor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/me")]
[Authorize]
public class CurrentUserController(IDispatcher dispatcher, ICurrentUserService currentUserService) : ControllerBase
{
    [HttpGet("stories")]
    public async Task<IActionResult> GetMyStories(CancellationToken cancellationToken)
    {
        var currentUserId = await currentUserService.GetUserIdAsync(cancellationToken);
        
        var query = new GetStoriesByAuthorQuery(currentUserId);
        
        var result = await dispatcher.Dispatch(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }
}