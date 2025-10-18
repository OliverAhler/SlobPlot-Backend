using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Persistence.Entities.Stories;

namespace Infrastructure.Persistence.Entities.Identity;

[Table("user_profiles", Schema = "auth")]
public class DbUserProfile
{
    [Key]
    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    
    [Column("display_name")]
    [MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;
    
    [Column("bio")]
    [MaxLength(5000)]
    public string? Bio { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    // Navigation
    public DbUser User { get; set; } = null!;
    
    public ICollection<DbStory> Stories { get; set; } = new List<DbStory>();
}