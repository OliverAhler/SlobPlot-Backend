using Domain.Aggregates.Users;
using Domain.IRepositories;
using Domain.ValueObjects;
using Infrastructure.Context;
using Infrastructure.Mappers.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserProfileRepository(ContextUsers context) : IUserProfileRepository
{
    public async Task<UserProfile?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var profile = await context.UserProfiles
            .FirstOrDefaultAsync(u => u.UserId == userId.Value, cancellationToken);

        return profile?.ToDomain();
    }

    public async Task AddAsync(UserProfile profile, CancellationToken cancellationToken = default)
    {
        var dbProfile = profile.ToDb();
        context.UserProfiles.Add(dbProfile);
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UserProfile profile, CancellationToken cancellationToken = default)
    {
        var dbProfile = await context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == profile.UserId.Value, cancellationToken);
        
        if (dbProfile == null)
            throw new InvalidOperationException($"User with ID {dbProfile?.UserId} not found");
        
        // Update properties from domain model
        dbProfile.Bio = profile.Bio;
        dbProfile.DisplayName = profile.DisplayName;
        dbProfile.UpdatedAt = profile.UpdatedAt;
        
        await context.SaveChangesAsync(cancellationToken);

    }
}