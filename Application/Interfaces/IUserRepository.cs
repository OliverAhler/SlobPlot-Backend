using Application.Features.Auth.DTOs;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<IReadOnlyCollection<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<UserDto?> GetUserByUidAsync(Guid uid, CancellationToken cancellationToken);
}