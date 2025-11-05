using Application.Features.UserProfiles;
using Domain.UserManagement.Entities;
using Domain.UserManagement.ValueObjects;
using Infrastructure.Identity.Mappers;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Repositories;

public class UserProfileRepository(ApplicationDbContext context) : IUserProfileRepository
{
    public async Task<UserProfile?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var profile = await context.UserProfiles
            .AsNoTracking()
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