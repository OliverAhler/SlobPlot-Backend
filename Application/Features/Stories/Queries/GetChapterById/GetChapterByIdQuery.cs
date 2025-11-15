using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Common.Interfaces.Handlers.Messaging;
using Application.Features.Stories.DTOs;
using Domain.Common;
using Domain.Common.Authorization;
using Domain.UserManagement.ValueObjects;

namespace Application.Features.Stories.Queries.GetChapterById;

public record GetChapterByIdQuery(Guid StoryId, Guid ChapterId) : IQuery<Result<ChapterDetailDto>>;

public class GetChapterByIdQueryHandler(
    IStoryQueries storyQueries, 
    IAuthorizationService authorizationService) 
    : IQueryHandler<GetChapterByIdQuery, Result<ChapterDetailDto>>
{
    public async Task<Result<ChapterDetailDto>> Handle(
        GetChapterByIdQuery query, 
        CancellationToken cancellationToken)
    {
        // First, authorize access to the story
        var storyResult = await authorizationService.AuthorizeAndFetch(
            ct => storyQueries.GetStoryByIdAsync(query.StoryId, ct),
            story => UserId.From(story.AuthorId),
            AuthorizationPolicy.PublicOrOwner,
            cancellationToken
        );
        
        if (!storyResult.IsSuccess)
            return Result<ChapterDetailDto>.Failure(storyResult.Error);
        
        // Then get the chapter
        var chapter = await storyQueries.GetChapterDetailAsync(
            query.StoryId,
            query.ChapterId,
            cancellationToken);
        
        if (chapter is null)
            return Result<ChapterDetailDto>.Failure("Chapter not found");
        
        // Validate chapter belongs to this story
        if (chapter.StoryId != query.StoryId)
            return Result<ChapterDetailDto>.Failure("Chapter not found");
        
        return Result<ChapterDetailDto>.Success(chapter);
    }
}