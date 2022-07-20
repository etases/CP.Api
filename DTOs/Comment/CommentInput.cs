using System.ComponentModel.DataAnnotations;

namespace CP.Api.DTOs.Comment;

public class CommentInput
{
    public string Content { get; set; } = null!;
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Keyword { get; set; } = "";
    public int? ParentId { get; set; } = null;
    public int CategoryId { get; set; }
}