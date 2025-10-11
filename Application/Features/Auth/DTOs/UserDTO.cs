namespace Application.Features.Auth.DTOs;

public record UserDto(
    Guid Id,
    string UserName,
    DateTime CreatedAt
);