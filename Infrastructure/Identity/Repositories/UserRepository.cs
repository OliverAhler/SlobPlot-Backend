using Application.Features.Auth;
using Domain.UserManagement.Aggregates;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetUserBySubAsync(Guid subUid, CancellationToken cancellationToken)
    {
        return await context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.SubUid == subUid, cancellationToken);
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