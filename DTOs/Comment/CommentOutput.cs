using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using CP.Api.DTOs.Account;
using CP.Api.DTOs.Category;

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

    public virtual AccountOutput Account { get; set; } = null!;
    //public virtual CategorySimple Category { get; set; } = null!;

}