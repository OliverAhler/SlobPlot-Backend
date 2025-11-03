using Application.Common.Interfaces;
using Application.Features.Stories.DTOs;
using Domain.Common;

namespace Application.Features.Stories.Queries.GetStoryById;

public record GetStoryByIdQuery(Guid Id) : IQuery<Result<StoryDetailDto>>;

public class GetStoryByIdHandler(IStoryQueries storyQueries, ICurrentUserService currentUser) : IQueryHandler<GetStoryByIdQuery, Result<StoryDetailDto>>
{
    public async Task<Result<StoryDetailDto>> Handle(GetStoryByIdQuery query, CancellationToken cancellationToken)
    {
        var currentUserId = await currentUser.GetUserIdAsync(cancellationToken);
        var story = await storyQueries.GetStoryByIdAsync(query.Id, cancellationToken);
        
        if (story is null)
            return Result<StoryDetailDto>.Failure($"Story with id {query.Id} not found");
        
        if(story.IsPrivate && story.UserId != currentUserId)
            return Result<StoryDetailDto>.Failure($"Story with id {query.Id} is private");
        
        
        return Result<StoryDetailDto>.Success(story);
    }
}
