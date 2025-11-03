using Application.Features.Stories;
using Domain.StoryManagement.Aggregates;
using Infrastructure.Persistence;
using Infrastructure.Stories.Mappers;

namespace Infrastructure.Stories.Repositories;

public class StoryRepository(ApplicationDbContext context) : IStoryRepository
{
    public void AddStory(Story story)
    {
        context.Stories.Add(story.ToDb());
    }
    
    public void UpdateStory(Story story)
    {
        context.Stories.Update(story.ToDb());
    }
}