using API.Contracts.Stories.CreateStory;
using API.Contracts.Stories.UpdateChapter;
using API.Contracts.Stories.UpdateStory;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Features.Stories.Commands.CreateStory;
using Application.Features.Stories.Commands.UpdateChapter;
using Application.Features.Stories.Commands.UpdateStory;
using Application.Features.Stories.Queries.GetChapterById;
using Application.Features.Stories.Queries.GetStories;
using Application.Features.Stories.Queries.GetStoryById;
using Application.Features.Stories.Queries.GetStoryChapters;
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

    #region Story Patches
    
    [HttpPatch]
    [Route("{storyId:guid}/privacy")]
    [Authorize]
    public async Task<IActionResult> UpdateStoryPrivacy([FromRoute] Guid storyId, [FromBody] UpdateStoryPrivacyRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateStoryPrivacyCommand(storyId, request.IsPublic);

        var result = await dispatcher.Dispatch(command, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result) 
            : NotFound(result.Error);
    }
    
    [HttpPatch]
    [Route("{storyId:guid}/title")]
    [Authorize]
    public async Task<IActionResult> UpdateStoryTitle([FromRoute] Guid storyId, [FromBody] UpdateStoryTitleRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateStoryTitleCommand(storyId, request.Title);

        var result = await dispatcher.Dispatch(command, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result) 
            : NotFound(result.Error);
    }
    
    [HttpPatch]
    [Route("{storyId:guid}/subtitle")]
    [Authorize]
    public async Task<IActionResult> UpdateStorySubTitle([FromRoute] Guid storyId, [FromBody] UpdateStorySubTitleRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateStorySubTitleCommand(storyId, request.SubTitle);

        var result = await dispatcher.Dispatch(command, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result) 
            : NotFound(result.Error);
    }
    
    [HttpPatch]
    [Route("{storyId:guid}/summary")]
    [Authorize]
    public async Task<IActionResult> UpdateStorySummary([FromRoute] Guid storyId, [FromBody] UpdateStorySummaryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateStorySummaryCommand(storyId, request.Summary);

        var result = await dispatcher.Dispatch(command, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result) 
            : NotFound(result.Error);
    }
    
    [HttpPatch]
    [Route("{storyId:guid}/genres")]
    [Authorize]
    public async Task<IActionResult> UpdateStoryGenres([FromRoute] Guid storyId, [FromBody] UpdateStoryGenresRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateStoryGenresCommand(storyId, request.GenreIds);

        var result = await dispatcher.Dispatch(command, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result) 
            : NotFound(result.Error);
    }
    
    #endregion
    
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


    #region Chapters

    [HttpGet]
    [Route("{storyId:guid}/chapters")]
    public async Task<IActionResult> GetStoryChapters([FromRoute] Guid storyId, CancellationToken cancellationToken)
    {
        var query = new GetStoryChaptersQuery(storyId);
        var result = await dispatcher.Dispatch(query, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : BadRequest(result.Error);
    }
    
    [HttpGet("{storyId:guid}/chapters/{chapterId:guid}")]
    public async Task<IActionResult> GetChapterById(
        [FromRoute] Guid storyId,
        [FromRoute] Guid chapterId, 
        CancellationToken cancellationToken)
    {
        var query = new GetChapterByIdQuery(storyId, chapterId);
        var result = await dispatcher.Dispatch(query, cancellationToken);
    
        return result.IsSuccess 
            ? Ok(result.Value) 
            : NotFound(result.Error);
    }
    
    [HttpPatch]
    [Route("{storyId:guid}/chapters/{chapterId:guid}/title")]
    [Authorize]
    public async Task<IActionResult> UpdateChapterTitle(
        [FromRoute] Guid storyId,
        [FromRoute] Guid chapterId,
        [FromBody] UpdateChapterTitleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateChapterTitleCommand(storyId, chapterId, request.Title);
        var result = await dispatcher.Dispatch(command, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result) 
            : BadRequest(result.Error);
    }
    
    [HttpPatch]
    [Route("{storyId:guid}/chapters/{chapterId:guid}/body")]
    [Authorize]
    public async Task<IActionResult> UpdateChapterBody(
        [FromRoute] Guid storyId,
        [FromRoute] Guid chapterId,
        [FromBody] UpdateChapterBodyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateChapterBodyCommand(storyId, chapterId, request.Body);
        var result = await dispatcher.Dispatch(command, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result) 
            : BadRequest(result.Error);
    }

    #endregion
    
}