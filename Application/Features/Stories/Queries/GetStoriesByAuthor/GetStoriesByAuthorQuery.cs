using Application.Common.Interfaces.Handlers;
using Application.Common.Interfaces.Handlers.Messaging;
using Application.Features.Stories.DTOs;
using Domain.Common;

namespace Application.Features.Stories.Queries.GetStoriesByAuthor;

public record GetStoriesByAuthorQuery(Guid AuthorId) : IQuery<Result<IReadOnlyCollection<StoryListDetailedDto>>>;

public class GetStoriesByAuthorHandler(IStoryQueries storyQueries) : IQueryHandler<GetStoriesByAuthorQuery, Result<IReadOnlyCollection<StoryListDetailedDto>>>
{
    public async Task<Result<IReadOnlyCollection<StoryListDetailedDto>>> Handle(GetStoriesByAuthorQuery query, CancellationToken cancellationToken)
    {
        var result = await storyQueries.GetExpandedStoriesByUserIdAsync(query.AuthorId, cancellationToken);

        return Result<IReadOnlyCollection<StoryListDetailedDto>>.Success(result);
    }
}