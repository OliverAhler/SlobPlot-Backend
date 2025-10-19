using Domain.Common;
using Domain.ValueObjects;
using Domain.ValueObjects.Story;

namespace Domain.Aggregates.Stories;

public class StoryChapter : AggregateRoot
{
    public ChapterId Id { get; private set; } = null!;
    public StoryId StoryId { get; private set; } = null!;
    public int ChapterNumber { get; private set; }
    public string Title { get; private set; } = null!;
    public string Body { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private StoryChapter() {}
}