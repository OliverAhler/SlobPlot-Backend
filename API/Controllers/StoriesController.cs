using API.Contracts.Stories.CreateStory;
using API.Contracts.Stories.UpdateStory;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Features.Stories.Commands.CreateStory;
using Application.Features.Stories.Commands.UpdateStory;
using Application.Features.Stories.Queries.GetStories;
using Application.Features.Stories.Queries.GetStoryById;
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
            : BadRequest(result.Error);
    }
    
    [HttpGet]
    [Route("{storyId:guid}")]
    public async Task<IActionResult> GetStoryById([FromRoute] Guid storyId, CancellationToken cancellationToken)
    {
        var query = new GetStoryByIdQuery(storyId);

        var result = await dispatcher.Dispatch(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }
    
    [HttpPut]
    [Route("{storyId:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateStoryPrivacy([FromRoute] Guid storyId, [FromBody] UpdateStoryPrivacyRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateStoryPrivacyCommand(storyId, request.IsPrivate);

        var result = await dispatcher.Dispatch(command, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result) 
            : NotFound(result.Error);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateStory([FromBody] CreateStoryRequest request, CancellationToken cancellationToken)
    {
        var userId = await currentUserService.GetUserIdAsync(cancellationToken);
        
        var command = new CreateStoryCommand(userId, request.Title, request.SubTitle, request.Summary, request.IsPrivate, request.GenreIds);
        var result = await dispatcher.Dispatch(command, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : BadRequest(result.Error);
    }
}