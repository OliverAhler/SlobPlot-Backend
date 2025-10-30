namespace Application.Features.UserProfiles.DTOs;

public record UserProfileDto(Guid Id, string DisplayName, string? Bio, DateTime UpdatedAt);