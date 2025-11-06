namespace Application.Features.Stories.DTOs;

public record StoryListDetailedDto(
    Guid Id,
    string Title,
    string Author,
    List<string> Genres,
    bool IsPublic,
    string Status,
    DateTime UpdatedAt,
    DateTime CreatedAt
);