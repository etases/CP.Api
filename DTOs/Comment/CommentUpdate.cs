using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using CP.Api.DTOs.Account;
using CP.Api.DTOs.Category;

namespace CP.Api.DTOs.Comment;

public class CommentUpdate
{
    public string Content { get; set; } = null!;
    public string Keyword { get; set; } = string.Empty;

}