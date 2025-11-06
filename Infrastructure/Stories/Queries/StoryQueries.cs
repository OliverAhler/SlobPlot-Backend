using Application.Features.Stories;
using Application.Features.Stories.DTOs;
using Domain.StoryManagement.ValueObjects;
using Domain.UserManagement.ValueObjects;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Stories.Queries;

public class StoryQueries(ApplicationDbContext context) : IStoryQueries
{
    public async Task<IReadOnlyCollection<StoryListItemDto>> GetStoriesAsync(CancellationToken ct = default)
    {
        return await context.Stories
            .AsNoTracking()
            .Where(s => !s.IsDeleted && s.IsPublic)
            .Select(s => new StoryListItemDto(
                s.Id, 
                s.Title, 
                s.UserProfile.DisplayName,
                s.StoryGenres.Select(p => p.Genre.DisplayName).ToList(),
                s.UpdatedAt,
                s.CreatedAt
            ))
            .ToListAsync(ct);
    }
    
    public async Task<StoryDetailDto?> GetStoryByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Stories
            .AsNoTracking()
            .Where(s => s.Id == id && !s.IsDeleted)
            .Select(s => new StoryDetailDto(
                s.Id,
                s.UserId,
                s.UserProfile.DisplayName,
                s.Title,
                s.Subtitle,
                s.Summary,
                s.StoryGenres.Select(p => p.Genre.DisplayName).ToList(),
                s.IsPublic,
                s.CreatedAt,
                s.UpdatedAt
            ))
            .FirstOrDefaultAsync(ct);
    }
    
    public async Task<IReadOnlyCollection<StoryListDetailedDto>> GetExpandedStoriesByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await context.Stories
            .AsNoTracking()
            .Where(s => s.UserId == userId && !s.IsDeleted)
            .Select(s => new StoryListDetailedDto(
                s.Id, 
                s.Title, 
                s.UserProfile.DisplayName,
                s.StoryGenres.Select(p => p.Genre.DisplayName).ToList(),
                s.IsPublic,
                "Complete",
                s.UpdatedAt,
                s.CreatedAt
            ))
            .ToListAsync(ct);
    }
    
    public async Task<IReadOnlyCollection<StoryListItemDto>> GetStoriesByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await context.Stories
            .AsNoTracking()
            .Where(s => s.UserId == userId && !s.IsDeleted)
            .Select(s => new StoryListItemDto(
                s.Id, 
                s.Title, 
                s.UserProfile.DisplayName,
                s.StoryGenres.Select(p => p.Genre.DisplayName).ToList(),
                s.UpdatedAt,
                s.CreatedAt
            ))
            .ToListAsync(ct);
    }
    
    public async Task<IReadOnlyCollection<StoryListItemDto>> GetPublicStoriesByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await context.Stories
            .AsNoTracking()
            .Where(s => s.UserId == userId && !s.IsDeleted && s.IsPublic)
            .Select(s => new StoryListItemDto(
                s.Id, 
                s.Title, 
                s.UserProfile.DisplayName,
                s.StoryGenres.Select(p => p.Genre.DisplayName).ToList(),
                s.UpdatedAt,
                s.CreatedAt
            ))
            .ToListAsync(ct);
    }
    
    public async Task<IReadOnlyCollection<StoryListItemDto>> SearchStoriesAsync(string searchTerm, CancellationToken ct = default)
    {
        var lowerSearchTerm = searchTerm.ToLower(); 
        
        return await context.Stories
            .AsNoTracking()
            .Where(s => s.Title.ToLower().Contains(lowerSearchTerm) && !s.IsDeleted)
            .Select(s => new StoryListItemDto(
                s.Id, 
                s.Title, 
                s.UserProfile.DisplayName,
                s.StoryGenres.Select(p => p.Genre.DisplayName).ToList(),
                s.UpdatedAt,
                s.CreatedAt
            ))
            .ToListAsync(ct);
    }
}