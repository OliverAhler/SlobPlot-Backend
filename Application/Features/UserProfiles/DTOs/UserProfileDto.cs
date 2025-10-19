namespace Application.Features.UserProfiles.DTOs;

public record UserProfileDto(Guid UserId, string UserName, string DisplayName, string? Bio, DateTime UpdatedAt);