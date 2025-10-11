using Application.Features.Auth.DTOs;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<IReadOnlyCollection<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<UserDto?> GetUserByIdPUidAsync(Guid sub, CancellationToken cancellationToken);
    Task<SyncUserDto> SyncUserFromIdPAsync(Guid subUid, string username, CancellationToken cancellationToken);
}