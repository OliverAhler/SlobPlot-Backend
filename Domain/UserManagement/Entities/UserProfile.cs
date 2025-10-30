using Domain.Common;
using Domain.UserManagement.ValueObjects;

namespace Domain.UserManagement.Entities;

public class UserProfile : Entity<UserId>
{
    public string DisplayName { get; private set; } = null!;
    public string? Bio { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private UserProfile() { }
    
    internal static UserProfile CreateFor(UserId userId, string displayName)
    {
        // No validation needed - User aggregate already validated!
        return new UserProfile
        {
            Id = userId,
            DisplayName = displayName,
            UpdatedAt = DateTime.UtcNow
        };
    }

    #region User Methods
    internal Result UpdateDisplayName(string newDisplayName)
    {
        if (string.IsNullOrWhiteSpace(newDisplayName))
            return Result.Failure("Display name is required");
        
        if (newDisplayName.Length > 100)
            return Result.Failure("Display name cannot exceed 100 characters");
        
        DisplayName = newDisplayName;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
    
    internal Result UpdateBio(string? newBio)
    {
        if (newBio?.Length > 5000)
            return Result.Failure("Bio cannot exceed 5000 characters");
        
        Bio = newBio;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
    #endregion
}