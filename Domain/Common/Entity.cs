namespace Domain.Common;

public abstract class Entity<TId>
{
    public TId Id { get; protected set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
            return false;
            
        return Id?.Equals(other.Id) ?? false;
    }
    
    public override int GetHashCode() => Id?.GetHashCode() ?? 0;
}