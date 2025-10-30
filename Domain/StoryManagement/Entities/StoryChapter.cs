using Domain.Common;
using Domain.StoryManagement.ValueObjects;

namespace Domain.StoryManagement.Entities;

public class StoryChapter : Entity<ChapterId>
{
    public StoryId StoryId { get; private set; } = null!;
    public int ChapterNumber { get; private set; }
    public string Title { get; private set; } = null!;
    public string Body { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private StoryChapter() {}
    
    internal static StoryChapter Create(
        StoryId storyId, 
        int chapterNumber, 
        string title, 
        string body)
    {
        return new StoryChapter
        {
            Id = new ChapterId(Guid.NewGuid()),
            StoryId = storyId,
            ChapterNumber = chapterNumber,
            Title = title,
            Body = body,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
    
    internal void Update(string title, string body)
    {
        Title = title;
        Body = body;
        UpdatedAt = DateTime.UtcNow;
    }
    
    internal void UpdateChapterNumber(int newNumber)
    {
        ChapterNumber = newNumber;
        UpdatedAt = DateTime.UtcNow;
    }
}