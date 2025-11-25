namespace Application.Features.Stories.DTOs;

public record StoryListDetailedDto(
    Guid Id,
    Guid AuthorId,
    string Title,
    string Author,
    List<string> Genres,
    bool IsPublic,
    string Status,
    DateTime UpdatedAt,
    DateTime CreatedAt
);