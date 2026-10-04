using Application.Common.Interfaces;
using Application.Features.Stories.DTOs;
using Vesia.Dispatch;
using Vesia.Result;

namespace Application.Features.Stories.Queries.CurrentUserStories;

public record GetCurrentUserStoriesQuery : IQuery<Result<IReadOnlyCollection<StoryListDetailedDto>>>;

public class GetCurrentUserStoriesHandler(IStoryQueries storyQueries, ICurrentUserService currentUserService) : IQueryHandler<GetCurrentUserStoriesQuery, Result<IReadOnlyCollection<StoryListDetailedDto>>>
{
    public async Task<Result<IReadOnlyCollection<StoryListDetailedDto>>> Handle(GetCurrentUserStoriesQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = await currentUserService.GetUserIdAsync(cancellationToken);
        var result = await storyQueries.GetStoriesByAuthorAsync(currentUserId, currentUserId, cancellationToken);

        return Result<IReadOnlyCollection<StoryListDetailedDto>>.Success(result);
    }
}