namespace Application.Features.Auth.DTOs;

public record UserProfileDto(
    Guid Id,
    string DisplayName,
    string? Bio,
    string? IconColor,
    string? IconCode,
    DateTime CreatedAt
);