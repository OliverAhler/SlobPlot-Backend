namespace Domain.StoryManagement.ValueObjects;

public record StoryId(Guid Value)
{
    public static StoryId From(Guid value) => new(value);
}