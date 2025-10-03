using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

[Table("icons", Schema = "idm")]
public class DbIcon
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("code")]
    public string Code { get; set; }
    
    [Column("label")]
    public string Label { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; }
}