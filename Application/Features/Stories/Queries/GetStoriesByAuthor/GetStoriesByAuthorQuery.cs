using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Common.Interfaces.Handlers.Messaging;
using Application.Features.Stories.DTOs;
using Domain.Common;

namespace Application.Features.Stories.Queries.GetStoriesByAuthor;

public record GetStoriesByAuthorQuery(Guid AuthorId) : IQuery<Result<IReadOnlyCollection<StoryListDetailedDto>>>;

public class GetStoriesByAuthorHandler(IStoryQueries storyQueries, ICurrentUserService currentUserService) : IQueryHandler<GetStoriesByAuthorQuery, Result<IReadOnlyCollection<StoryListDetailedDto>>>
{
    public async Task<Result<IReadOnlyCollection<StoryListDetailedDto>>> Handle(GetStoriesByAuthorQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = await currentUserService.GetUserIdAsync(cancellationToken);
        var result = await storyQueries.GetStoriesByAuthorAsync(query.AuthorId, currentUserId, cancellationToken);

        return Result<IReadOnlyCollection<StoryListDetailedDto>>.Success(result);
    }
}