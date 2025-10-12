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

    public async Task<bool> SyncUserFromIdPAsync(Guid subUid, string username, CancellationToken cancellationToken)
    {
        var existingUser = await context.Users
            .FirstOrDefaultAsync(u => u.SubUid == subUid && !u.IsDeleted, cancellationToken);
    
        if (existingUser != null)
        {
            // Only update if username changed
            if (existingUser.UserName == username) return false;
            
            existingUser.UserName = username;
            existingUser.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);

            return false;
        }

        // User doesn't exist - create new
        var newUser = new DbUser
        {
            Id = Guid.NewGuid(),
            SubUid = subUid,
            UserName = username,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var newProfile = new DbUserProfile()
        {
            UserId = newUser.Id,
            Bio = null,
            DisplayName = username,
            UpdatedAt = DateTime.UtcNow
        };
    
        context.Users.Add(newUser);
        context.UserProfiles.Add(newProfile);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}