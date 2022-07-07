using CP.Api.DTOs.Account;

namespace CP.Api.DTOs.Comment;

public class CommentOutput
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public string Keyword { get; set; } = string.Empty;
    public int? ParentId { get; set; } = null;
    public int CategoryId { get; set; } 
    public int AccountId { get; set; } 
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
    public bool HasChild { get; set; } = false;

    public virtual AccountOutput Account { get; set; } = null!;
    public virtual CategoryOutput Category { get; set; } = null!;
}