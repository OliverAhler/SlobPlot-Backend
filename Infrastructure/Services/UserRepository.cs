using Application.Features.Auth.DTOs;
using Application.Interfaces;
using Infrastructure.Context;
using Infrastructure.Mappers.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserRepository(ContextSlobPlot context) : IUserRepository
{
    public async Task<IReadOnlyCollection<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        return await context.Users
            .Where(user => !user.IsDeleted)
            .Select(UserMapper.ToDto)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserDto?> GetUserByUidAsync(Guid uid, CancellationToken cancellationToken )
    {
        return await context.Users
            .Where(user => user.Id == uid)
            .Select(UserMapper.ToDto)
            .FirstOrDefaultAsync(cancellationToken);
    }
}