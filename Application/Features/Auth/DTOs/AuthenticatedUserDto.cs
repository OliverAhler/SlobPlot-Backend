namespace Application.Features.Auth.DTOs;

public record AuthenticatedUserDto(
    Guid Id,
    Guid SubUid,
    string UserName,
    DateTime CreatedAt
);