using Application.IRepositories;
using Domain.Aggregates.Users;
using Domain.ValueObjects.Identity;
using Infrastructure.Mappers.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Identity;

public class UserProfileRepository(ApplicationDbContext context) : IUserProfileRepository
{
    public async Task<UserProfile?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var profile = await context.UserProfiles
            .FirstOrDefaultAsync(u => u.UserId == userId.Value, cancellationToken);

        return profile?.ToDomain();
    }

    public void AddUserProfile(UserProfile profile)
    {
        var dbProfile = profile.ToDb();
        context.UserProfiles.Add(dbProfile);
    }

    public void UpdateUserProfile(UserProfile profile)
    {
        var dbProfile = profile.ToDb();
        context.UserProfiles.Update(dbProfile);
    }
}