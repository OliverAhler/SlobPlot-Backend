using Application.Features.Auth.DTOs;

namespace Application.Features.Auth;

public interface IUserQueries
{
    Task<UserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<UserDto?> GetUserBySubAsync(Guid sub, CancellationToken cancellationToken);
}