using Application.Features.Stories.DTOs;

namespace Application.Features.Stories;

public interface IStoryQueries
{
    Task<IReadOnlyCollection<StoryListItemDto>> GetStoriesAsync(CancellationToken ct = default);
    Task<StoryDetailDto?> GetStoryByIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyCollection<StoryListDetailedDto>> GetStoriesByAuthorAsync(Guid authorId, Guid? currentUserId, CancellationToken ct = default);
    Task<IReadOnlyCollection<StoryListItemDto>> SearchStoriesAsync(string searchTerm, CancellationToken ct = default);

    //Chapters
    Task<IReadOnlyCollection<ChapterListItemDto>> GetStoryChaptersAsync(Guid storyId, CancellationToken ct = default);
    Task<ChapterDetailDto?> GetChapterDetailAsync(Guid storyId, Guid chapterId, CancellationToken ct = default);


}