using System.Linq.Expressions;
using Domain.Aggregates.Stories;
using Infrastructure.Persistence.Entities.Stories;

namespace Infrastructure.Mappers.Stories;

public static class StoryMapper
{
    // Expression that EF Core CAN translate to SQL
    private static readonly Expression<Func<DbStory, Story>> ToDomainExpression = dbStory =>
        Story.Reconstitute(
            dbStory.Id,
            dbStory.UserId,
            dbStory.Title,
            dbStory.Subtitle,
            dbStory.Summary,
            dbStory.IsPrivate,
            dbStory.StoryStatusId,
            dbStory.CreatedAt,
            dbStory.UpdatedAt
        );
    
    public static IQueryable<Story> ProjectToDomain(this IQueryable<DbStory> query)
    {
        return query.Select(ToDomainExpression);
    }
    
    // Db → Domain
    public static Story ToDomain(this DbStory dbStory)
    {
        return Story.Reconstitute(
            dbStory.Id,
            dbStory.UserId,
            dbStory.Title,
            dbStory.Subtitle,
            dbStory.Summary,
            dbStory.IsPrivate,
            dbStory.StoryStatusId,
            dbStory.CreatedAt,
            dbStory.UpdatedAt
        );
    }
    
    // Domain → Db
    public static DbStory ToDb(this Story story)
    {
        return new DbStory()
        {
            Id = story.Id.Value,
            UserId = story.UserId.Value,
            Title = story.Title,
            Subtitle = story.SubTitle,
            Summary = story.Summary,
            StoryStatusId = story.StoryStatusId,
            IsPrivate = story.IsPrivate,
            CreatedAt = story.CreatedAt,
            UpdatedAt = story.UpdatedAt,
            StoryGenres = story.StoryGenres.Select(genreId => new DbStoryGenre
            {
                StoryId = story.Id.Value,
                GenreId = genreId
            }).ToList()

        };
    }
}
