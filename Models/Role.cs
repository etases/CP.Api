using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP.Api.Models;

public class Role
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public string? Note { get; set; }
    
    [InverseProperty("Role")]
    public virtual ICollection<Account> Accounts { get; set; } = null!;
}