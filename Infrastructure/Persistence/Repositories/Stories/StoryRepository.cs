using Application.Features.Stories;
using Domain.StoryManagement.Aggregates;

namespace Infrastructure.Persistence.Repositories.Stories;

public class StoryRepository(ApplicationDbContext context) : IStoryRepository
{
    public void AddStory(Story story)
    {
        context.Stories.Add(story);
    }
    
    public void UpdateStory(Story story)
    {
        context.Stories.Update(story);
    }
}