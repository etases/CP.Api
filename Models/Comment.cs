using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP.Api.Models;

public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public string Content { get; set; } = null!;
    public string Keyword { get; set; } = string.Empty;

    public int? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    [InverseProperty(nameof(Children))]
    public virtual Comment? Parent { get; set; }

    [InverseProperty(nameof(Parent))] public virtual ICollection<Comment> Children { get; set; } = null!;

    [Required] public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    [InverseProperty("Comments")]
    public virtual Category Category { get; set; } = null!;

    [Required] public int AccountId { get; set; }

    [ForeignKey(nameof(AccountId))]
    [InverseProperty("Comments")]
    public virtual Account Account { get; set; } = null!;
    
    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    [Column(TypeName = "timestamp without time zone")]
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
    
    public bool IsDeleted { get; set; } = false;

    [InverseProperty("Comment")] public ICollection<Vote> Votes { get; set; } = null!;

    [NotMapped]
    public virtual string[] KeywordArray
    {
        get => Keyword.Split(',');
        set => Keyword = string.Join(',', value);
    }
}