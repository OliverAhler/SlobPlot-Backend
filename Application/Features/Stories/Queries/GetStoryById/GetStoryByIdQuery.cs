using Application.Common.Interfaces;
using Application.Features.Stories.DTOs;
using Domain.Common;
using Domain.StoryManagement.ValueObjects;

namespace Application.Features.Stories.Queries.GetStoryById;

public record GetStoryByIdQuery(Guid Id) : IQuery<Result<StoryDetailDto>>;

public class GetStoryByIdHandler(IStoryQueries storyQueries) : IQueryHandler<GetStoryByIdQuery, Result<StoryDetailDto>>
{
    public async Task<Result<StoryDetailDto>> Handle(GetStoryByIdQuery query, CancellationToken cancellationToken)
    {
        var story = await storyQueries.GetStoryByIdAsync(query.Id, cancellationToken);
        
        if (story is null)
            return Result<StoryDetailDto>.Failure($"Story with id {query.Id} not found");
        
        return Result<StoryDetailDto>.Success(story);
    }
}
