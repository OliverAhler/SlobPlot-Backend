using Domain.UserManagement.Entities;

namespace Application.Features.UserProfiles;

public interface IUserProfileRepository
{
    void AddUserProfile(UserProfile profile);
    void UpdateUserProfile(UserProfile profile);
}