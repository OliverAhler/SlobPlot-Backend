using Domain.Aggregates.Users;
using Domain.ValueObjects.Identity;

namespace Application.IRepositories;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
    void AddUserProfile(UserProfile profile);
    void UpdateUserProfile(UserProfile profile);
}