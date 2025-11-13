namespace API.Contracts.Stories.CreateChapter;

public record CreateChapterRequest(string Title, string Body, bool IsPublic);