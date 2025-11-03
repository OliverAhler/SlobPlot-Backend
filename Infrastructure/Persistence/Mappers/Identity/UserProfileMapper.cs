using Domain.UserManagement.Entities;
using Infrastructure.Persistence.Entities.Identity;

namespace Infrastructure.Persistence.Mappers.Identity;

public static class UserProfileMapper
{
    // Db → Domain
    public static UserProfile ToDomain(this DbUserProfile dbProfile)
    {
        return UserProfile.Reconstitute(
            dbProfile.UserId,
            dbProfile.DisplayName,
            dbProfile.Bio,
            dbProfile.UpdatedAt
        );
    }
    
    // Domain User → DbUser
    public static DbUserProfile ToDb(this UserProfile profile)
    {
        return new DbUserProfile
        {
            UserId = profile.Id.Value,
            DisplayName = profile.DisplayName,
            Bio = profile.Bio,
            UpdatedAt = profile.UpdatedAt
        };
    }
}