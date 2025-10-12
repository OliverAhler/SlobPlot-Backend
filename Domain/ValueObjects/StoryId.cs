namespace Domain.ValueObjects;

public record StoryId(Guid Value)
{
    public static StoryId From(Guid value) => new(value);
}