using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Identity.Entities;

[Table("users", Schema = "auth")]
public class DbUser
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("sub_uid")]
    public Guid SubUid { get; set; }
    
    [Column("user_name")]
    [MaxLength(30)]
    [MinLength(3)]
    public string UserName { get; set; } = string.Empty;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
    
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
    
    
    // Navigation
    public DbUserProfile? Profile { get; set; }
}