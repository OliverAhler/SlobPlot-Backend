namespace Application.Features.Stories.DTOs;

public record StoryListItemDto(
    Guid Id,
    string Title,
    string Author,
    List<string> Genres,
    DateTime UpdatedAt,
    DateTime CreatedAt
);

//Status
//Collaborators
//LastUpdated
//WhenCreated