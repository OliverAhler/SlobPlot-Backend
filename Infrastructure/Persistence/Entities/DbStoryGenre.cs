using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Persistence.Entities;

[Table("story_genres", Schema = "story")]
public class DbStoryGenre
{
    [Column("story_id")]
    public Guid StoryId { get; set; }
    
    [Column("genre_id")]
    public int GenreId { get; set; }
    
    // Navigation
    public DbGenre Genre { get; set; } = null!;
}