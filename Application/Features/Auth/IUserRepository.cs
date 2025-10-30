using Domain.UserManagement.Aggregates;

namespace Application.Features.Auth;

public interface IUserRepository
{
    Task<User?> GetUserBySubAsync(Guid subUid, CancellationToken ct); 
    void AddUser(User user);
    void UpdateUser(User user);
}