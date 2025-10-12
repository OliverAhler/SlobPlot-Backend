using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Aggregates.Stories;

public class Story : AggregateRoot
{
    public StoryId Id { get; private set; }
    public UserId UserId { get; private set; }
    public string Title { get; private set; }
    public string SubTitle { get; private set; }
    public string Summary { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private Story() {}
}