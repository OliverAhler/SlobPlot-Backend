using Domain.Aggregates.Users;

namespace Application.Features.UserProfiles;

public interface IUserProfileRepository
{
    void AddUserProfile(UserProfile profile);
    void UpdateUserProfile(UserProfile profile);
}