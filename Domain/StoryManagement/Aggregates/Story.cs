using Domain.Common;
using Domain.StoryManagement.Entities;
using Domain.StoryManagement.ValueObjects;
using Domain.UserManagement.ValueObjects;


namespace Domain.StoryManagement.Aggregates;

public class Story : AggregateRoot<StoryId>
{
    public UserId AuthorId { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string? SubTitle { get; private set; }
    public string? Summary { get; private set; }
    public bool IsPrivate { get; private set; }
    
    public int StoryStatusId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    // Chapters collection
    private readonly List<StoryChapter> _chapters = new();
    public IReadOnlyCollection<StoryChapter> Chapters => _chapters.AsReadOnly();
    
    // Genres collection
    internal readonly List<int> _genreIds = new();
    public IReadOnlyCollection<int> GenreIds => _genreIds.AsReadOnly();
    
    private Story() {}
    
    
    public static Result<Story> Create(UserId userId, string title, string? subTitle, string? summary, bool isPrivate, int[] genreIds)
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
            IsPrivate = isPrivate,
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

    public void ToggleIsPrivate(bool isPrivate)
    {
        IsPrivate = isPrivate;
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
    
    private bool CanTransitionTo(int newStatusId)
    {
        // Add  business rules for status transitions
        // Example: Can't go from Completed back to Planned
        return true;
    }
    #endregion

    #region Chapter Management

    

    #endregion
}