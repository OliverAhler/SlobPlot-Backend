namespace Application.Features.Stories.DTOs;

public record StoryDetailDto(
    Guid Id,
    Guid AuthorId,
    string AuthorName,
    string Title,
    string? SubTitle,
    string? Summary,
    List<string> Genres,
    bool IsPublic,
    bool IsOwner,
    DateTime CreatedAt,
    DateTime UpdatedAt
);