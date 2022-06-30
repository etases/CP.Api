namespace CP.Api.DTOs.Comment;

public class CommentInput
{
    public string Content { get; set; } = null!;
    public string Keyword { get; set; } = string.Empty;
    public int? ParentId { get; set; } = null;
    public int CategoryId { get; set; } 



}