using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Stories.Entities;

namespace Infrastructure.Master.Entities;

[Table("genres", Schema = "master")]
public class DbGenre
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("display_name")]
    [MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;
    
    // Navigation
    public ICollection<DbStoryGenre> StoryGenres { get; set; } = new List<DbStoryGenre>();
}