using Application.Features.Stories;
using Domain.StoryManagement.Aggregates;
using Domain.StoryManagement.ValueObjects;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Stories.Repositories;

public class StoryRepository(ApplicationDbContext context) : IStoryRepository
{
    public async Task<Story?> GetStoryById(StoryId storyId, CancellationToken cancellationToken)
    {
        return await context.Stories
            .Include(s => s.Chapters)
            .FirstOrDefaultAsync(story => story.Id == storyId, cancellationToken);
    }

    public void AddStory(Story story)
    {
        context.Stories.Add(story);
    }

    public void UpdateStory(Story story)
    {
        context.Stories.Update(story);
    }
}