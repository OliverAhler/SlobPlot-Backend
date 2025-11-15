namespace Application.Features.Stories.DTOs;

public record ChapterDetailDto(Guid Id, Guid StoryId, string Title, string Body, int ChapterNumber);