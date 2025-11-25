namespace Application.Features.Auth.DTOs;

public record AuthenticatedUserDto(
    Guid Id,
    string UserName,
    DateTime CreatedAt
);