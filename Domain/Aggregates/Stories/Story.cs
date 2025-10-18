using Domain.Common;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Story;

namespace Domain.Aggregates.Stories;

public class Story : AggregateRoot
{
    public StoryId Id { get; private set; }
    public UserId UserId { get; private set; }
    public string Title { get; private set; }
    public string? SubTitle { get; private set; }
    public string? Summary { get; private set; }
    public bool IsPrivate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private Story() {}
    
    
    #region Database Reconstitute
    public static Story Reconstitute(Guid id, Guid userId, string title, string subTitle, string summary, bool isPrivate, DateTime createdAt, DateTime updatedAt)
    {
        return new Story
        {
            Id = StoryId.From(id),
            UserId = UserId.From(userId),
            Title = title,
            SubTitle = subTitle,
            Summary = summary,
            IsPrivate = isPrivate,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }
    #endregion
    
    public static Result<Story> Create(UserId userId, string title, string? subTitle, string? summary, bool isPrivate)
    {
        if(userId.Value == Guid.Empty)
            return Result<Story>.Failure("Invalid userId");
        
        title = title.Trim();
        subTitle = string.IsNullOrWhiteSpace(subTitle) ? null : subTitle.Trim();
        summary = string.IsNullOrWhiteSpace(summary) ? null : summary.Trim();
        
        if (title.Length > 100)
            return Result<Story>.Failure("Title cannot exceed 100 characters");
        if (subTitle?.Length > 255)
            return Result<Story>.Failure("SubTitle cannot exceed 255 characters");
        if (summary?.Length > 1500)
            return Result<Story>.Failure("Summary cannot exceed 1500 characters");
        
        return Result<Story>.Success(new Story
        {
            Id = StoryId.From(Guid.NewGuid()),
            UserId = userId,
            Title = title,
            SubTitle = subTitle,
            Summary = summary,
            IsPrivate = isPrivate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
    }

    #region Story Methods
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

    public void ToggleIsPrivate(bool isPrivate)
    {
        IsPrivate = isPrivate;
        UpdatedAt = DateTime.UtcNow;
    }
    #endregion
}