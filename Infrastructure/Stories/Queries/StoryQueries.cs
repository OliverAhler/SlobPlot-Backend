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
            .Include(s => s.Author)
            .Include(s => s.Genres)
            .Where(s => s.IsPublic)
            .Select(s => new StoryListItemDto(
                s.Id.Value,
                s.Title,
                s.Author!.DisplayName,
                s.Genres.Select(g => g.DisplayName).ToList(),
                s.UpdatedAt,
                s.CreatedAt
            ))
            .ToListAsync(ct);
    }

    public async Task<StoryDetailDto?> GetStoryByIdAsync(Guid id, CancellationToken ct = default)
    {
        var storyId = StoryId.From(id);

        return await context.Stories
            .AsNoTracking()
            .Include(s => s.Author)
            .Include(s => s.Genres)
            .Where(s => s.Id == storyId)
            .Select(s => new StoryDetailDto(
                s.Id.Value,
                s.AuthorId.Value,
                s.Author!.DisplayName,
                s.Title,
                s.SubTitle,
                s.Summary,
                s.Genres.Select(g => g.DisplayName).ToList(),
                s.IsPublic,
                false, //IsOwner - Calculated in Application layer
                s.CreatedAt,
                s.UpdatedAt
            ))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyCollection<StoryListDetailedDto>> GetStoriesByAuthorAsync(Guid authorId, Guid? currentUserId, CancellationToken ct = default)
    {
        var authorUserId = UserId.From(authorId);
        var currentUserIdValue = currentUserId.HasValue ? UserId.From(currentUserId.Value) : (UserId?)null;

        return await context.Stories
            .AsNoTracking()
            .Include(s => s.Author)
            .Include(s => s.Genres)
            .Where(s => s.AuthorId == authorUserId
                     && (s.IsPublic || (currentUserIdValue != null && s.AuthorId == currentUserIdValue)))
            .Select(s => new StoryListDetailedDto(
                s.Id.Value,
                s.Title,
                s.Author!.DisplayName,
                s.Genres.Select(g => g.DisplayName).ToList(),
                s.IsPublic,
                "Complete",
                currentUserIdValue != null && s.AuthorId == currentUserIdValue,
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
            .Include(s => s.Author)
            .Include(s => s.Genres)
            .Where(s => s.Title.ToLower().Contains(lowerSearchTerm))
            .Select(s => new StoryListItemDto(
                s.Id.Value,
                s.Title,
                s.Author!.DisplayName,
                s.Genres.Select(g => g.DisplayName).ToList(),
                s.UpdatedAt,
                s.CreatedAt
            ))
            .ToListAsync(ct);
    }

    #region Chapters

    public async Task<IReadOnlyCollection<ChapterListItemDto>> GetStoryChaptersAsync(Guid storyId, CancellationToken ct = default)
    {
        var storyIdValue = StoryId.From(storyId);

        return await context.Chapters
            .AsNoTracking()
            .Where(c => c.StoryId == storyIdValue)
            .OrderBy(c => c.ChapterNumber)
            .Select(chapter => new ChapterListItemDto(
                chapter.Id.Value,
                chapter.Title,
                chapter.ChapterNumber,
                chapter.CreatedAt)
            )
            .ToListAsync(ct);
    }

    public async Task<ChapterDetailDto?> GetChapterDetailAsync(Guid storyId, Guid chapterId, CancellationToken ct = default)
    {
        return await context.Chapters
            .AsNoTracking()
            .Where(c => c.Id == ChapterId.From(chapterId) && c.StoryId == StoryId.From(storyId))
            .Select(chapter => new ChapterDetailDto(
                chapter.Id.Value,
                chapter.StoryId.Value,
                chapter.Title,
                chapter.Body,
                chapter.ChapterNumber)
            )
            .FirstOrDefaultAsync(ct);
    }

    #endregion
}