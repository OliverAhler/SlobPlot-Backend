using Application.Features.Stories.DTOs;

namespace Application.Features.Stories;

public interface IStoryQueries
{
    Task<IReadOnlyCollection<StoryListItemDto>> GetStoriesAsync(CancellationToken ct = default);
    Task<StoryDetailDto?> GetStoryByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyCollection<StoryListItemDto>> GetStoriesByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<IReadOnlyCollection<StoryListItemDto>> GetPublicStoriesByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<IReadOnlyCollection<StoryListItemDto>> SearchStoriesAsync(string searchTerm, CancellationToken ct = default);
}