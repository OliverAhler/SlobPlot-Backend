using Domain.Common;

namespace Domain.StoryManagement.Entities;

public class Genre : Entity<int>
{
    public string DisplayName { get; private set; } = string.Empty;

    private Genre() { }
}
