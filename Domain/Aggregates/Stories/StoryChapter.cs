using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Aggregates.Stories;

public class StoryChapter : AggregateRoot
{
    public ChapterId Id { get; private set; }
    public StoryId StoryId { get; private set; }
    public int ChapterNumber { get; private set; }
    public string Title { get; private set; }
    public string Body { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private StoryChapter() {}
}