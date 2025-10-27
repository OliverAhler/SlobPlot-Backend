using Application.Features.Auth;
using Application.Features.Auth.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries.Identity;

public class UserQueries(ApplicationDbContext context) : IUserQueries
{
    public async Task<Guid?> GetUserIdBySubIdAsync(Guid sub, CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.SubUid == sub && !u.IsDeleted)
            .Select(u => u.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AuthenticatedUserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Id == id && !u.IsDeleted)
            .Select(u => new AuthenticatedUserDto(
                u.Id,
                u.SubUid,
                u.UserName,
                u.CreatedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AuthenticatedUserDto?> GetUserBySubAsync(Guid subUid, CancellationToken cancellationToken )
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.SubUid == subUid && !u.IsDeleted)
            .Select(u => new AuthenticatedUserDto(
                u.Id,
                u.SubUid,
                u.UserName,
                u.CreatedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}