using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Persistence.Entities;

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
    public string? StatusDescription { get; set; }
}