using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

[Table("users", Schema = "idm")]
public class DbUser
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("display_name")]
    public string DisplayName { get; set; }
    
    [Column("bio")]
    public string? Bio { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
    
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
    
    [Column("icon_id")]
    public int? IconId { get; set; }
    
    [Column("icon_color")]
    public string? IconColor { get; set; }
    
    // Navigation property
    public DbIcon? Icon { get; set; }
}