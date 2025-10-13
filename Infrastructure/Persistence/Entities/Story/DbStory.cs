using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Persistence.Entities.Identity;
using Infrastructure.Persistence.Entities.Master;

namespace Infrastructure.Persistence.Entities.Story;

[Table("stories", Schema = "story")]
public class DbStory
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("user_profile_id")]
    public Guid UserId { get; set; }
    
    [Column("story_status_id")]
    public int StoryStatusId { get; set; }
    
    [Column("title")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [Column("subtitle")]
    [MaxLength(100)]
    public string? Subtitle { get; set; }
    
    [Column("summary")]
    [MaxLength(1500)]
    public string? Summary { get; set; }
    
    [Column("is_private")]
    public bool IsPrivate { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
    
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
    
    // Navigation
    public DbUserProfile UserProfile { get; set; } = null!;
    public DbStoryStatus StoryStatus { get; set; } = null!;
    public ICollection<DbStoryGenre> StoryGenres { get; set; } = new List<DbStoryGenre>();
    public ICollection<DbChapter> Chapters { get; set; } = new List<DbChapter>();
}