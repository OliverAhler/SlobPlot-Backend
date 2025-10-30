using Application.Features.Stories;
using Application.Features.Stories.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries.Stories;

public class StoryQueries(ApplicationDbContext context) : IStoryQueries
{
    public async Task<IReadOnlyCollection<StoryListItemDto>> GetStoriesAsync(CancellationToken ct = default)
    {
        return await context.Stories
            .AsNoTracking()
            .Where(s => !s.IsPrivate)
            .Join(
                context.UserProfiles,
                story => story.AuthorId,
                profile => profile.Id,
                (story, profile) => new StoryListItemDto(
                    story.Id.Value,
                    story.Title,
                    profile.DisplayName
                ))
            .ToListAsync(ct);
    }
    
    public async Task<StoryDetailDto?> GetStoryByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Stories
            .AsNoTracking()
            .Where(s => s.Id.Value == id)
            .Join(
                context.UserProfiles,
                story => story.AuthorId,
                profile => profile.Id,
                (story, profile) => new StoryDetailDto(
                    story.Id.Value,
                    story.AuthorId.Value,
                    profile.DisplayName,
                    story.Title,
                    story.SubTitle,
                    story.Summary,
                    story.CreatedAt,
                    story.UpdatedAt))
            .FirstOrDefaultAsync(ct);
    }
    
    public async Task<IReadOnlyCollection<StoryListItemDto>> GetStoriesByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await context.Stories
            .AsNoTracking()
            .Where(s => s.AuthorId.Value == userId)
            .Join(
                context.UserProfiles,
                story => story.AuthorId,
                profile => profile.Id,
                (story, profile) => new StoryListItemDto(
                    story.Id.Value,
                    story.Title,
                    profile.DisplayName
                ))
            .ToListAsync(ct);
    }
    
    public async Task<IReadOnlyCollection<StoryListItemDto>> GetPublicStoriesByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await context.Stories
            .AsNoTracking()
            .Where(s => s.AuthorId.Value == userId && !s.IsPrivate)
            .Join(
                context.UserProfiles,
                story => story.AuthorId,
                profile => profile.Id,
                (story, profile) => new StoryListItemDto(
                    story.Id.Value,
                    story.Title,
                    profile.DisplayName
                ))
            .ToListAsync(ct);
    }
    
    public async Task<IReadOnlyCollection<StoryListItemDto>> SearchStoriesAsync(string searchTerm, CancellationToken ct = default)
    {
        var lowerSearchTerm = searchTerm.ToLower(); 
        
        return await context.Stories
            .AsNoTracking()
            .Where(s => s.Title.ToLower().Contains(lowerSearchTerm))
            .Join(
                context.UserProfiles,
                story => story.AuthorId,
                profile => profile.Id,
                (story, profile) => new StoryListItemDto(
                    story.Id.Value,
                    story.Title,
                    profile.DisplayName
                ))
            .ToListAsync(ct);
    }
}