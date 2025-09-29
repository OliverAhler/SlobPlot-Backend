using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

[Table("profile_icons", Schema = "idm")]
public class DbProfileIcon
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("code")]
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Column("label")]
    [Required]
    [MaxLength(100)]
    public string Label { get; set; } = string.Empty;

    [Column("is_active")]
    public bool IsActive { get; set; }
}