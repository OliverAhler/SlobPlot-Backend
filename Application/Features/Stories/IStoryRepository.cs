using Domain.StoryManagement.Aggregates;
using Domain.StoryManagement.ValueObjects;

namespace Application.Features.Stories;

public interface IStoryRepository
{
    Task<Story?> GetStoryById(StoryId storyId, CancellationToken cancellationToken);
    void AddStory(Story story);
    void UpdateStory(Story story);
}