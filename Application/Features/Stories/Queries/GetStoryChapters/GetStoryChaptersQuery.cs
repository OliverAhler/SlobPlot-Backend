using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Common.Interfaces.Handlers.Messaging;
using Application.Features.Stories.DTOs;
using Domain.Common;
using Domain.Common.Authorization;
using Domain.UserManagement.ValueObjects;

namespace Application.Features.Stories.Queries.GetStoryChapters;

public record GetStoryChaptersQuery(Guid StoryId) : IQuery<Result<IReadOnlyCollection<ChapterListItemDto>>>;

public class GetStoryChaptersQueryHandler(IStoryQueries storyQueries, IAuthorizationService authorizationService) : IQueryHandler<GetStoryChaptersQuery, Result<IReadOnlyCollection<ChapterListItemDto>>>
{
    public async Task<Result<IReadOnlyCollection<ChapterListItemDto>>> Handle(GetStoryChaptersQuery query, CancellationToken cancellationToken)
    {
        var result = await authorizationService.AuthorizeAndFetch(
            ct => storyQueries.GetStoryByIdAsync(query.StoryId, ct),
            story => UserId.From(story.AuthorId),
            AuthorizationPolicy.PublicOrOwner,
            cancellationToken
        );
        
        if(!result.IsSuccess)
            return Result<IReadOnlyCollection<ChapterListItemDto>>.Failure(result.Error);
        
        var story = result.Value;
        
        var storyChapters = await storyQueries.GetStoryChaptersAsync(story.Id, cancellationToken);
        
        return Result<IReadOnlyCollection<ChapterListItemDto>>.Success(storyChapters);
    }
}