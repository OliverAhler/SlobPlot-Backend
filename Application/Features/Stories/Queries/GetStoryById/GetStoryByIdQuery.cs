using Application.Common.Interfaces;
using Application.Common.Interfaces.Handlers;
using Application.Common.Interfaces.Handlers.Messaging;
using Application.Features.Stories.DTOs;
using Domain.Common;
using Domain.Common.Authorization;
using Domain.UserManagement.ValueObjects;

namespace Application.Features.Stories.Queries.GetStoryById;

public record GetStoryByIdQuery(Guid Id) : IQuery<Result<StoryDetailDto>>;

public class GetStoryByIdHandler(IStoryQueries storyQueries, ICurrentUserService currentUser, IAuthorizationService authorizationService) : IQueryHandler<GetStoryByIdQuery, Result<StoryDetailDto>>
{
    public async Task<Result<StoryDetailDto>> Handle(GetStoryByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await authorizationService.AuthorizeAndFetch(
            ct => storyQueries.GetStoryByIdAsync(query.Id, ct),
            story => UserId.From(story.AuthorId),
            AuthorizationPolicy.PublicOrOwner,
            cancellationToken
        );
        
        if(!result.IsSuccess)
            return Result<StoryDetailDto>.Failure(result.Error);

        var story = result.Value;
        
        var currentUserId = await currentUser.GetUserIdAsync(cancellationToken);
        var enrichedDto = story with { IsOwner = story.AuthorId == currentUserId };
        
        return Result<StoryDetailDto>.Success(enrichedDto);
    }
}
