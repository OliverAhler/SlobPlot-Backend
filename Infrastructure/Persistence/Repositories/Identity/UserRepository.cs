using Application.Features.Auth;
using Domain.UserManagement.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Identity;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetUserBySubAsync(Guid subUid, CancellationToken cancellationToken)
    {
        return await context.Users
            .Where(u => u.SubUid == subUid)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void AddUser(User user)
    {
        context.Users.Add(user);
    }

    public void UpdateUser(User user)
    {
        context.Users.Update(user);
    }
}