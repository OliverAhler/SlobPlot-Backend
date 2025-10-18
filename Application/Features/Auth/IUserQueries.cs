using Application.Features.Auth.DTOs;
using Application.Features.Auth.Queries.GetUserBySub;
using Domain.Aggregates.Users;

namespace Application.Features.Auth;

public interface IUserQueries
{
    Task<UserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<UserDto?> GetUserBySubAsync(Guid sub, CancellationToken cancellationToken);
}