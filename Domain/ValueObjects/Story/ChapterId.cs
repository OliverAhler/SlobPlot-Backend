namespace Domain.ValueObjects.Story;

public record ChapterId(Guid Value)
{
    public static ChapterId From(Guid value) => new(value);
}