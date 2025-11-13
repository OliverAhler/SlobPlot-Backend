using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Features.Stories.Queries.CurrentUserStories;
using Application.Features.Stories.Queries.GetStoriesByAuthor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var result = await dispatcher.Dispatch(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }
}