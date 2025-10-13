using Domain.Aggregates.Users;
using Domain.IRepositories;
using Infrastructure.Mappers.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Identity;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var dbUser = await context.Users
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);
        
        return dbUser?.ToDomain();
    }

    public async Task<User?> GetUserBySubAsync(Guid subUid, CancellationToken cancellationToken )
    {
        var dbUser = await context.Users
            .FirstOrDefaultAsync(u => u.SubUid == subUid && !u.IsDeleted, cancellationToken);
        
        return dbUser?.ToDomain();
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var dbUser = user.ToDb();
        context.Users.Add(dbUser);
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var dbUser = await context.Users
            .FirstOrDefaultAsync(u => u.Id == user.Id.Value, cancellationToken);
        
        if (dbUser == null)
            throw new InvalidOperationException($"User with ID {user.Id.Value} not found");
        
        // Update properties from domain model
        dbUser.UserName = user.UserName;
        dbUser.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync(cancellationToken);
    }
}