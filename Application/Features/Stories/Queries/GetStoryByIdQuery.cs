using Application.Common.Interfaces;
using Application.Features.Stories.DTOs;
using Domain.Common.Authorization;
using Domain.UserManagement.ValueObjects;
using Vesia.Dispatch;
using Vesia.Result;

namespace Application.Features.Stories.Queries.GetStoryById;

public record GetStoryByIdQuery(Guid Id) : IQuery<Result<StoryDetailDto>>;

public class GetStoryByIdHandler(IStoryQueries storyQueries, IAuthorizationService authorizationService) : IQueryHandler<GetStoryByIdQuery, Result<StoryDetailDto>>
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
        
        return Result<StoryDetailDto>.Success(story);
    }
}
