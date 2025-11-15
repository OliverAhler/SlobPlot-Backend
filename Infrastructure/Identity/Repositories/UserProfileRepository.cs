using Application.Features.UserProfiles;
using Domain.UserManagement.Entities;
using Domain.UserManagement.ValueObjects;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Repositories;

public class UserProfileRepository(ApplicationDbContext context) : IUserProfileRepository
{
    public async Task<UserProfile?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);
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