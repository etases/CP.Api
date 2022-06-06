using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP.Api.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public string? Note { get; set; }
    
    [InverseProperty("Category")]
    public virtual ICollection<Comment> Comments { get; set; } = null!;
}