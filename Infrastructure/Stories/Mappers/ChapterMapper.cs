using Domain.StoryManagement.Entities;
using Infrastructure.Stories.Entities;

namespace Infrastructure.Stories.Mappers;

public static class ChapterMapper
{
    // Db → Domain
    public static StoryChapter ToDomain(this DbChapter dbChapter)
    {
        return StoryChapter.Reconstitute(
            dbChapter.Id,
            dbChapter.StoryId,
            dbChapter.ChapterNumber,
            dbChapter.Title,
            dbChapter.Body,
            dbChapter.IsPublic,
            dbChapter.CreatedAt,
            dbChapter.UpdatedAt
        );
    }
    
    // Domain → Db
    public static DbChapter ToDb(this StoryChapter chapter)
    {
        return new DbChapter
        {
            Id = chapter.Id.Value,
            StoryId = chapter.StoryId.Value,
            ChapterNumber = chapter.ChapterNumber,
            Title = chapter.Title,
            Body = chapter.Body,
            IsPublic = chapter.IsPublic,
            CreatedAt = chapter.CreatedAt,
            UpdatedAt = chapter.UpdatedAt
        };
    }
}