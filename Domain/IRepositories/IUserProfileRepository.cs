using Domain.Aggregates.Users;
using Domain.ValueObjects;

namespace Domain.IRepositories;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
    Task AddAsync(UserProfile profile, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserProfile profile, CancellationToken cancellationToken = default);
}