using Application.IRepositories;
using Domain.Aggregates.Users;
using Infrastructure.Mappers.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Identity;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var dbUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);
        
        return dbUser?.ToDomain();
    }

    public async Task<User?> GetUserBySubAsync(Guid subUid, CancellationToken cancellationToken )
    {
        var dbUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.SubUid == subUid && !u.IsDeleted, cancellationToken);
        
        return dbUser?.ToDomain();
    }

    public void AddUser(User user)
    {
        var dbUser = user.ToDb();
        context.Users.Add(dbUser);
    }

    public void UpdateUser(User user)
    {
        var dbUser = user.ToDb();
        context.Users.Update(dbUser);
    }
}