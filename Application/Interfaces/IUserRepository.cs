using Application.Features.Auth.DTOs;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<IReadOnlyCollection<UserProfileDto>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<UserProfileDto?> GetUserByUidAsync(Guid uid, CancellationToken cancellationToken);
}