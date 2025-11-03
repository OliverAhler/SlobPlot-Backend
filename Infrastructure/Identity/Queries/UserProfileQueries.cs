using Application.Features.UserProfiles;
using Application.Features.UserProfiles.DTOs;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Queries;

public class UserProfileQueries(ApplicationDbContext context) : IUserProfileQueries
{
    public async Task<UserProfileDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await context.UserProfiles
            .AsNoTracking()
            .Where(u => u.UserId == userId)
            .Select(u => new UserProfileDto(
                u.UserId,
                u.DisplayName,
                u.Bio,
                u.UpdatedAt)
            )
            .FirstOrDefaultAsync(cancellationToken);
    }
}