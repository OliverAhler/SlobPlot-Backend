using Application.Features.UserProfiles;
using Domain.UserManagement.Entities;
using Domain.UserManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Identity;

public class UserProfileRepository(ApplicationDbContext context) : IUserProfileRepository
{
    public async Task<UserProfile?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var profile = await context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id.Value == userId.Value, cancellationToken);

        return profile;
    }

    public void AddUserProfile(UserProfile profile)
    {
        context.UserProfiles.Add(profile);
    }

    public void UpdateUserProfile(UserProfile profile)
    {
        context.UserProfiles.Update(profile);
    }
}