using Application.Features.Stories.Queries.CurrentUserStories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Venly.Dispatch.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("api/me")]
[Authorize]
public class CurrentUserController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet("stories")]
    public async Task<IActionResult> GetMyStories(CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserStoriesQuery();
        var result = await dispatcher.DispatchAsync(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }
}