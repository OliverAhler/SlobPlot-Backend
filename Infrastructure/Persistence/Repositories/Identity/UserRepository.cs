using Application.Features.Auth;
using Domain.Aggregates.Users;
using Infrastructure.Mappers.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Identity;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetUserBySubAsync(Guid subUid, CancellationToken cancellationToken)
    {
        return await context.Users
            .Where(u => u.SubUid == subUid && !u.IsDeleted)
            .ProjectToDomain()
            .FirstOrDefaultAsync(cancellationToken);
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