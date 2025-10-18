using Domain.Aggregates.Stories;

namespace Application.Features.Stories;

public interface IStoryRepository
{
    void AddStory(Story story);
    void UpdateStory(Story story);
}