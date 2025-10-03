using Application.Features.Auth.DTOs;
using Application.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserRepository(ContextSlobPlot context) : IUserRepository
{
    private readonly ContextSlobPlot _context = context;

    public async Task<IReadOnlyCollection<UserProfileDto>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        return await _context.Users
            .Where(user => !user.IsDeleted)
            .Select(user => new UserProfileDto(
                user.Id,
                user.DisplayName,
                user.Bio,
                user.IconColor,
                user.Icon.Code,
                user.CreatedAt
            )).ToListAsync(cancellationToken);
    }

    public async Task<UserProfileDto?> GetUserByUidAsync(Guid uid, CancellationToken cancellationToken )
    {
        return await _context.Users
            .Where(user => user.Id == uid)
            .Select(user => new UserProfileDto(
                user.Id,
                user.DisplayName,
                user.Bio,
                user.IconColor,
                user.Icon.Code,
                user.CreatedAt
            )).FirstOrDefaultAsync(cancellationToken);
    }
}