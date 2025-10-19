namespace Application.Features.Stories.DTOs;

public record StoryDetailDto(
    Guid Id,
    Guid UserId,
    string AuthorName,
    string Title,
    string? SubTitle,
    string? Summary,
    DateTime CreatedAt,
    DateTime UpdatedAt
);