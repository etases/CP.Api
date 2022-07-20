using System.ComponentModel.DataAnnotations;

namespace CP.Api.DTOs.Comment;

public class CommentUpdate
{
    public string Content { get; set; } = null!;
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Keyword { get; set; } = "";
}