using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Aggregates.Users;

public class User : AggregateRoot
{
    public UserId Id { get; private set; }
    public Guid SubUid { get; private set; }
    public string UserName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private User() { }
    
    // Minimal validation - just sanity checks - Source of truth is Authentik IdP (For user info)
    public static Result<User> Create(Guid subUid, string userName)
    {
        if (subUid == Guid.Empty)
            return Result<User>.Failure("SubUid is required");
        
        if (string.IsNullOrWhiteSpace(userName))
            return Result<User>.Failure("Username is required");
        
        return Result<User>.Success(new User
        {
            Id = UserId.From(Guid.NewGuid()),
            SubUid = subUid,
            UserName = userName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
    }
    
    // Reconstitute from DB
    public static User Reconstitute(Guid id, Guid subUid, string userName, DateTime createdAt, DateTime updatedAt)
    {
        return new User
        {
            Id = UserId.From(id),
            SubUid = subUid,
            UserName = userName,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }
    
    // Simple update - Authentik already validated the new username
    public void UpdateUserName(string newUserName)
    {
        UserName = newUserName;
        UpdatedAt = DateTime.UtcNow;
    }
}