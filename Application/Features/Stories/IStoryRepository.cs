using Domain.StoryManagement.Aggregates;

namespace Application.Features.Stories;

public interface IStoryRepository
{
    void AddStory(Story story);
    void UpdateStory(Story story);
}