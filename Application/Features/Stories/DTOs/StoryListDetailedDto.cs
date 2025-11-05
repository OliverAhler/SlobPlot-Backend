namespace Application.Features.Stories.DTOs;

public record StoryListDetailedDto(
    Guid Id,
    string Title,
    string Author,
    List<string> Genres,
    bool IsPrivate,
    string Status,
    DateTime UpdatedAt,
    DateTime CreatedAt
);