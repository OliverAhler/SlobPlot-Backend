using Domain.Common;
using Domain.StoryManagement.Entities;
using Domain.StoryManagement.ValueObjects;
using Domain.UserManagement.Entities;
using Domain.UserManagement.ValueObjects;


namespace Domain.StoryManagement.Aggregates;

public class Story : AggregateRoot<StoryId>
{
    public UserId AuthorId { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string? SubTitle { get; private set; }
    public string? Summary { get; private set; }
    public bool IsPublic { get; private set; }
    
    public int StoryStatusId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    // Chapters collection
    private List<StoryChapter> _chapters = [];
    public IReadOnlyCollection<StoryChapter> Chapters => _chapters.AsReadOnly();

    // Genres collection - for domain logic
    internal readonly List<int> _genreIds = [];
    public IReadOnlyCollection<int> GenreIds => _genreIds.AsReadOnly();

    // Navigation properties - for EF Core queries only (not exposed for domain logic)
    private UserProfile? _author;
    public UserProfile? Author => _author;

    private List<Genre> _genres = [];
    public IReadOnlyCollection<Genre> Genres => _genres.AsReadOnly();

    private Story() {}
    
    public static Result<Story> Create(UserId userId, string title, string? subTitle, string? summary, bool isPublic, int[] genreIds)
    {
        if(userId.Value == Guid.Empty)
            return Result<Story>.Failure("Invalid userId");
        
        if(genreIds.Length is 0 or > 5)
            return Result<Story>.Failure("Amount of genres for a story must be between 1 and 5");
        
        title = title.Trim();
        subTitle = string.IsNullOrWhiteSpace(subTitle) ? null : subTitle.Trim();
        summary = string.IsNullOrWhiteSpace(summary) ? null : summary.Trim();
        
        if (title.Length > 100)
            return Result<Story>.Failure("Title cannot exceed 100 characters");
        if (subTitle?.Length > 255)
            return Result<Story>.Failure("SubTitle cannot exceed 255 characters");
        if (summary?.Length > 1500)
            return Result<Story>.Failure("Summary cannot exceed 1500 characters");
        
        var story = new Story
        {
            Id = StoryId.From(Guid.NewGuid()),
            AuthorId = userId,
            Title = title,
            SubTitle = subTitle,
            Summary = summary,
            StoryStatusId = StoryStatus.Planned,
            IsPublic = isPublic,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        story._genreIds.AddRange(genreIds);
        
        // story.AddDomainEvent(new StoryCreatedEvent(story.Id, story.AuthorId));
        
        return Result<Story>.Success(story);
    }

    #region Story Metadata
    public Result UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure("Title is required");
    
        if (title.Length > 100)
            return Result.Failure("Title cannot exceed 100 characters");
    
        Title = title;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
    
    public Result UpdateSubTitle(string? subTitle)
    {
        subTitle = string.IsNullOrWhiteSpace(subTitle) ? null : subTitle.Trim();
    
        if (subTitle?.Length > 255)
            return Result.Failure("Subtitle cannot exceed 255 characters");

        SubTitle = subTitle;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
    public Result UpdateSummary(string? summary)
    {
        summary = string.IsNullOrWhiteSpace(summary) ? null : summary.Trim();
    
        if (summary?.Length > 1500)
            return Result.Failure("Summary cannot exceed 1500 characters");

        Summary = summary;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
    
    public Result UpdateGenres(int[] genreIds)
    {
        if (genreIds.Length is 0 or > 5)
            return Result.Failure("Story must have between 1 and 5 genres");
        
        _genreIds.Clear();
        _genreIds.AddRange(genreIds);
        UpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }

    public void UpdatePrivacy(bool isPublic)
    {
        IsPublic = isPublic;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public Result UpdateStatus(int newStatusId)
    {
        if (!CanTransitionTo(newStatusId))
            return Result.Failure($"Cannot transition from {StoryStatusId} to {newStatusId}");
        
        StoryStatusId = newStatusId;
        UpdatedAt = DateTime.UtcNow;
        
        // AddDomainEvent(new StoryStatusChangedEvent(Id, newStatusId));
        return Result.Success();
    }
    
    private static bool CanTransitionTo(int newStatusId)
    {
        // Add  business rules for status transitions
        // Example: Can't go from Completed back to Planned
        return true;
    }
    #endregion

    #region Chapter Management

    public Result<StoryChapter> AddChapter(string title, string body, bool isPublic)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result<StoryChapter>.Failure("Title is required");
    
        if (string.IsNullOrWhiteSpace(body))
            return Result<StoryChapter>.Failure("Chapter body is required");
        
        var chapterNumber = _chapters.Count != 0
            ? _chapters.Max(c => c.ChapterNumber) + 1 
            : 1;
        
        var chapter = StoryChapter.Create(Id, chapterNumber, title, body, isPublic);
    
        _chapters.Add(chapter);
        UpdatedAt = DateTime.UtcNow;
    
        return Result<StoryChapter>.Success(chapter);
    }
    
    public Result UpdateChapterTitle(ChapterId id, string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure("Title is required");

        var chapter = _chapters.FirstOrDefault(c => c.Id == id);
    
        if (chapter is null)
            return Result.Failure($"Chapter with id {id.Value} not found");
    
        chapter.UpdateTitle(title);
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result UpdateChapterBody(ChapterId id, string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return Result.Failure("Chapter body is required");

        var chapter = _chapters.FirstOrDefault(c => c.Id == id);
    
        if (chapter is null)
            return Result.Failure($"Chapter with id {id.Value} not found");
    
        chapter.UpdateBody(body);
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result UpdateChapterPrivacy(ChapterId id, bool isPublic)
    {
        var chapter = _chapters.FirstOrDefault(c => c.Id == id);
    
        if (chapter is null)
            return Result.Failure($"Chapter with id {id.Value} not found");
    
        chapter.UpdatePrivacy(isPublic);
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result RemoveChapter(ChapterId id)
    {
        var chapter = _chapters.FirstOrDefault(ch => ch.Id == id);
    
        if (chapter is null)
            return Result.Failure($"Chapter with id {id.Value} not found");
        
        _chapters.Remove(chapter);

        ReorderChapters();
        
        UpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }

    public Result ReorderChapter(ChapterId id, int newPosition)
    {
        var size = _chapters.Count;
    
        if (newPosition < 1 || newPosition > size)
            return Result.Failure($"Position must be between 1 and {size}");
    
        var chapter = _chapters.FirstOrDefault(ch => ch.Id == id);
    
        if (chapter is null)
            return Result.Failure($"Chapter with id {id.Value} not found");
    
        _chapters.Remove(chapter);
        _chapters.Insert(newPosition - 1, chapter);

        ReorderChapters();
    
        UpdatedAt = DateTime.UtcNow;
    
        return Result.Success();
    }
    #endregion
    
    private void ReorderChapters()
    {
        for (var i = 0; i < _chapters.Count; i++)
        {
            _chapters[i].UpdateChapterNumber(i + 1);
        }
    }
}