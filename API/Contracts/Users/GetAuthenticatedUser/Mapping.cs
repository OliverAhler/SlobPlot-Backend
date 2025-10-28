using Application.Features.Auth.DTOs;

namespace API.Contracts.Users.GetAuthenticatedUser;

public static class UserDtoExtensions
{
    public static AuthenticatedUserResponse ToResponse(this AuthenticatedUserDto dto)
    {
        return new AuthenticatedUserResponse(
            dto.UserName,
            dto.CreatedAt
        );
    }
}
