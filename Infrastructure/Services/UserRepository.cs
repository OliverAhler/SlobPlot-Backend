using Application.Features.Auth.DTOs;
using Application.Interfaces;
using Infrastructure.Context;
using Infrastructure.Mappers.User;
using Infrastructure.Models.IDM;
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

    public async Task<UserDto?> GetUserByIdPUidAsync(Guid sub, CancellationToken cancellationToken )
    {
        return await context.Users
            .Where(user => user.SubUid == sub)
            .Select(UserMapper.ToDto)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<SyncUserDto> SyncUserFromIdPAsync(Guid subUid, string username, CancellationToken cancellationToken)
    {
        var existingUser = await context.Users
            .FirstOrDefaultAsync(u => u.SubUid == subUid && !u.IsDeleted, cancellationToken);
    
        if (existingUser != null)
        {
            // Only update if username changed
            if (existingUser.UserName == username) return new SyncUserDto(IsNewUser: false);
            
            existingUser.UserName = username;
            existingUser.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
        
            return new SyncUserDto(IsNewUser: false);
        }

        // User doesn't exist - create new
        var newUser = new DbUser
        {
            SubUid = subUid,
            UserName = username,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    
        context.Users.Add(newUser);
        await context.SaveChangesAsync(cancellationToken);
    
        return new SyncUserDto(IsNewUser: true);
    }
}