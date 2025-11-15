using Application.Features.UserProfiles;
using Application.Features.UserProfiles.DTOs;
using Domain.UserManagement.ValueObjects;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Queries;

public class UserProfileQueries(ApplicationDbContext context) : IUserProfileQueries
{
    public async Task<UserProfileDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userIdValue = UserId.From(userId);

        return await context.UserProfiles
            .AsNoTracking()
            .Where(p => p.Id == userIdValue)
            .Select(p => new UserProfileDto(
                p.Id.Value,
                p.DisplayName,
                p.Bio,
                p.UpdatedAt)
            )
            .FirstOrDefaultAsync(cancellationToken);
    }
}