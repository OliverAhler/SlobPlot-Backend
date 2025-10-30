namespace Domain.UserManagement.ValueObjects;

public record UserId(Guid Value)
{
    public static UserId From(Guid value) => new(value);
}