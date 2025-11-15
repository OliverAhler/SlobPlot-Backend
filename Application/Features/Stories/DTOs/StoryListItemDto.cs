namespace Application.Features.Stories.DTOs;

//List elements DTO without any actions - Used for viewing multiple stories - like the main page
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