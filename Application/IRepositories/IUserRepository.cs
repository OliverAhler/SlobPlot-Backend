using Domain.Aggregates.Users;

namespace Application.IRepositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetUserBySubAsync(Guid sub, CancellationToken cancellationToken);
    void AddUser(User user);
    void UpdateUser(User user);
}