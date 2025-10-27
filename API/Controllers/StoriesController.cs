using API.Contracts.Stories.CreateStory;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Features.Stories.Commands.CreateStory;
using Application.Features.Stories.Queries.GetStories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoriesController(IDispatcher dispatcher, ICurrentUserService currentUserService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStories(CancellationToken cancellationToken)
    {
        var query = new GetStoriesQuery();

        var result = await dispatcher.Dispatch(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateStory([FromBody] CreateStoryRequest request, CancellationToken cancellationToken)
    {
        var userId = await currentUserService.GetUserIdAsync(cancellationToken);
        
        var command = new CreateStoryCommand(userId, request.Title, request.SubTitle, request.Summary, request.IsPrivate);
        var result = await dispatcher.Dispatch(command, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : BadRequest(result.Error);
    }
}