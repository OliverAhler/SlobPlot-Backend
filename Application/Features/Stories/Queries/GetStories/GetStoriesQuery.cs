using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Common.Interfaces.Handlers.Messaging;
using Application.Features.Stories.DTOs;
using Domain.Common;

namespace Application.Features.Stories.Queries.GetStories;

public record GetStoriesQuery() : IQuery<Result<IReadOnlyCollection<StoryListItemDto>>>;

public class GetStoriesCommandHandler(IStoryQueries storyQueries) : IQueryHandler<GetStoriesQuery, Result<IReadOnlyCollection<StoryListItemDto>>>
{
    public async Task<Result<IReadOnlyCollection<StoryListItemDto>>> Handle(GetStoriesQuery query, CancellationToken cancellationToken)
    {
        var result = await storyQueries.GetStoriesAsync(cancellationToken);
        
        return Result<IReadOnlyCollection<StoryListItemDto>>.Success(result);
    }
}