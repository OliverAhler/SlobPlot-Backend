using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Stories.Entities;

[Table("chapters", Schema = "story")]
public class DbChapter
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("story_id")]
    public Guid StoryId { get; set; }
    
    [Column("chapter_number")]
    public int ChapterNumber { get; set; }
    
    [Column("title")]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;
    
    [Column("body")]
    public string Body { get; set; } = string.Empty;
    
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
    public DbStory Story { get; set; } = null!;
}