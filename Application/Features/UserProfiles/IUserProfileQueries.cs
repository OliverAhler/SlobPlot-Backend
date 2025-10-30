using Application.Features.UserProfiles.DTOs;

namespace Application.Features.UserProfiles;

public interface IUserProfileQueries
{
    Task<UserProfileDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
}