using Application.Features.Stories;
using Domain.StoryManagement.Aggregates;
using Domain.StoryManagement.ValueObjects;
using Infrastructure.Persistence;
using Infrastructure.Stories.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Stories.Repositories;

public class StoryRepository(ApplicationDbContext context) : IStoryRepository
{
    public async Task<Story?> GetStoryById(StoryId storyId, CancellationToken cancellationToken)
    {
        var story = await context.Stories
            .AsNoTracking()
            .FirstOrDefaultAsync(story => story.Id == storyId.Value, cancellationToken);

        return story?.ToDomain();
    }
    
    public void AddStory(Story story)
    {
        context.Stories.Add(story.ToDb());
    }
    
    public void UpdateStory(Story story)
    {
        context.Stories.Update(story.ToDb());
    }
}