using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Persistence.Entities.Stories;

namespace Infrastructure.Persistence.Entities.Master;

[Table("story_status", Schema = "master")]
public class DbStatus
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("display_name")]
    [MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;
    
    [Column("status_description")]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;
    
    // Navigation
    public ICollection<DbStory> Stories { get; set; } = new List<DbStory>();
}