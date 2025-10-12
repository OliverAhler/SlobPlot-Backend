using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.IDM;

[Table("user_profiles", Schema = "idm")]
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
}