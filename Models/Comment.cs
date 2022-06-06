using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP.Api.Models;

public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string Content { get; set; } = null!;
    public int? ParentId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public int AccountId { get; set; }

    public bool Resolved { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
    
    [ForeignKey("AccountId")]
    [InverseProperty("Comments")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("Comments")]
    public virtual Category Category { get; set; } = null!;
    
    [ForeignKey("ParentId")]
    [InverseProperty("Children")]
    public virtual Comment? Parent { get; set; }
    
    [InverseProperty("Parent")]
    public virtual ICollection<Comment> Children { get; set; } = null!;
}