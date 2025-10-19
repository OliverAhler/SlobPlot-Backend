using Application.Features.UserProfiles.DTOs;

namespace Application.Features.UserProfiles;

public interface IUserProfileQueries
{
    Task<UserProfileDto?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}