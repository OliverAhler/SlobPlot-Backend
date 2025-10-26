using Application.Features.Auth.DTOs;

namespace Application.Features.Auth;

public interface IUserQueries
{
    Task<AuthenticatedUserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<AuthenticatedUserDto?> GetUserBySubAsync(Guid sub, CancellationToken cancellationToken);
}