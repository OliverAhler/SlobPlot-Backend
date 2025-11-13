namespace Application.Features.Stories.DTOs;

public record StoryListDetailedDto(
    Guid Id,
    string Title,
    string Author,
    List<string> Genres,
    bool IsPublic,
    string Status,
    bool IsOwner,
    DateTime UpdatedAt,
    DateTime CreatedAt
);