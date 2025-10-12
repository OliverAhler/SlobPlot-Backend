using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.Master;

namespace Infrastructure.Models.Story;

[Table("story_genres", Schema = "story")]
public class DbStoryGenre
{
    [Column("story_id")]
    public Guid StoryId { get; set; }
    
    [Column("genre_id")]
    public int GenreId { get; set; }
    
    // Navigation
    public DbStory Story { get; set; } = null!;
    public DbGenre Genre { get; set; } = null!;
}