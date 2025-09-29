using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

[Table("user_profiles", Schema = "idm")]
public class DbUserProfile
{
    [Key]
    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("display_name")]
    [Required]
    [MaxLength(255)]
    public string DisplayName { get; set; } = string.Empty;

    [Column("bio")]
    public string? Bio { get; set; }

    [Column("profile_icon_id")]
    public int? ProfileIconId { get; set; }

    [Column("profile_color")]
    [Required]
    [StringLength(7, MinimumLength = 7)]
    public string ProfileColor { get; set; } = string.Empty;

    [Column("preferences")]
    [Required]
    public string Preferences { get; set; } = string.Empty;
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public DbUser User { get; set; } = null!;

    [ForeignKey(nameof(ProfileIconId))]
    public DbProfileIcon? ProfileIcon { get; set; }
}