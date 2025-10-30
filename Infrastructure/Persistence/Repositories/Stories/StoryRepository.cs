using Application.Features.Stories;
using Application.IRepositories;
using Domain.StoryManagement.Aggregates;
using Infrastructure.Mappers.Stories;

namespace Infrastructure.Persistence.Repositories.Stories;

public class StoryRepository(ApplicationDbContext context) : IStoryRepository
{
    public void AddStory(Story story)
    {
        var dbStory = story.ToDb();
        context.Stories.Add(dbStory);
    }
    
    public void UpdateStory(Story story)
    {
        var dbStory = story.ToDb();
        context.Stories.Update(dbStory);
    }
}