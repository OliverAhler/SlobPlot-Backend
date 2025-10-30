using Application.Features.UserProfiles;
using Application.Features.UserProfiles.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries.Identity;

public class UserProfileQueries(ApplicationDbContext context) : IUserProfileQueries
{
    public async Task<UserProfileDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.UserProfiles
            .AsNoTracking()
            .Where(u => u.Id.Value == userId)
            .Select(u => new UserProfileDto(
                u.Id.Value,
                u.DisplayName,
                u.Bio,
                u.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }
}