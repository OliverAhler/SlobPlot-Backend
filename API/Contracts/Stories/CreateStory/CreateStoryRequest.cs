namespace API.Contracts.Stories.CreateStory;

public record CreateStoryRequest(string Title, string SubTitle, string Summary, bool IsPrivate);