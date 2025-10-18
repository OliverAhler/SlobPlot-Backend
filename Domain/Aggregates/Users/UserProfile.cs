using Domain.Common;
using Domain.ValueObjects.Identity;

namespace Domain.Aggregates.Users;

public class UserProfile : AggregateRoot
{
    public UserId UserId { get; private set; }
    public string DisplayName { get; private set; }
    public string? Bio { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private UserProfile() { }

    #region Database Reconstitute
    public static UserProfile Reconstitute(Guid userId, string displayName, string? bio, DateTime updatedAt)
    {
        return new UserProfile
        {
            UserId = UserId.From(userId),
            DisplayName = displayName,
            Bio = bio,
            UpdatedAt = updatedAt
        };
    }
    #endregion
    
    public static Result<UserProfile> Create(UserId userId, string displayName)
    {
        if(userId.Value == Guid.Empty)
            return Result<UserProfile>.Failure("Invalid userId");
        
        if (string.IsNullOrWhiteSpace(displayName))
            return Result<UserProfile>.Failure("Display name is required");
        
        if (displayName.Length > 100)
            return Result<UserProfile>.Failure("Display name cannot exceed 100 characters");
        
        return Result<UserProfile>.Success(new UserProfile
        {
            UserId = userId,
            DisplayName = displayName,
            Bio = null,
            UpdatedAt = DateTime.UtcNow
        });
    }

    #region User Methods
    public Result UpdateDisplayName(string newDisplayName)
    {
        if (string.IsNullOrWhiteSpace(newDisplayName))
            return Result.Failure("Display name is required");
        
        if (newDisplayName.Length > 100)
            return Result.Failure("Display name cannot exceed 100 characters");
        
        DisplayName = newDisplayName;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
    
    public Result UpdateBio(string? newBio)
    {
        if (newBio?.Length > 5000)
            return Result.Failure("Bio cannot exceed 5000 characters");
        
        Bio = newBio;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
    #endregion
}