using Application.Features.Auth;
using Application.Features.Auth.DTOs;
using Domain.UserManagement.ValueObjects;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Queries;

public class UserQueries(ApplicationDbContext context) : IUserQueries
{
    public async Task<Guid?> GetUserIdBySubIdAsync(Guid sub, CancellationToken cancellationToken)
    {
        var userId = await context.Users
            .AsNoTracking()
            .Where(u => u.SubUid == sub)
            .Select(u => u.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return userId?.Value;
    }

    public async Task<AuthenticatedUserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var userId = UserId.From(id);

        return await context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new AuthenticatedUserDto(
                u.Id.Value,
                u.SubUid,
                u.UserName,
                u.CreatedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AuthenticatedUserDto?> GetUserBySubAsync(Guid subUid, CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.SubUid == subUid)
            .Select(u => new AuthenticatedUserDto(
                u.Id.Value,
                u.SubUid,
                u.UserName,
                u.CreatedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}