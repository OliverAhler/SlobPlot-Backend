namespace Application.Features.Auth.DTOs;

public record UserDto(
    Guid Id,
    Guid SubUid,
    string UserName,
    DateTime CreatedAt
);