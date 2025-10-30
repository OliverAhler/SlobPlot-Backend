using Domain.Common;
using Domain.UserManagement.Entities;
using Domain.UserManagement.ValueObjects;

namespace Domain.UserManagement.Aggregates;

public class User : AggregateRoot<UserId>
{
    public Guid SubUid { get; private set; }
    public string UserName { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    
    private UserProfile _profile = null!;
    public UserProfile Profile => _profile;
    
    private User() { }

    #region User Methods
    // Minimal validation - just sanity checks - Source of truth is Authentik IdP (For user info)
    public static Result<User> Create(Guid subUid, string userName)
    {
        if (subUid == Guid.Empty)
            return Result<User>.Failure("SubUid is required");
        
        if (string.IsNullOrWhiteSpace(userName))
            return Result<User>.Failure("Username is required");
        
        var userId = UserId.From(Guid.NewGuid());
        
        var user = new User
        {
            Id = userId,
            SubUid = subUid,
            UserName = userName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            _profile = UserProfile.CreateFor(userId, userName)
        };

        return Result<User>.Success(user);
    }
    
    // Simple update - Authentik already validated the new username
    public void UpdateUserName(string newUserName)
    {
        UserName = newUserName;
        UpdatedAt = DateTime.UtcNow;
    }
    #endregion
    
}