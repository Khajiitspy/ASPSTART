using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASPSTART.Data.Entities;

[Table("tbl_users")]
public class UserEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(900)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? LastName { get; set; } = string.Empty;

    [StringLength(255)]
    public string Avatar { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
}
