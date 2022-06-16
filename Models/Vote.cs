using System.ComponentModel.DataAnnotations.Schema;

namespace CP.Api.Models;

public class Vote
{
    public int AccountId { get; set; }
    public int CommentId { get; set; }
    public bool IsUpvote { get; set; } // true = upvote, false = downvote

    [ForeignKey(nameof(AccountId))]
    [InverseProperty("Votes")]
    public Account Account { get; set; } = null!;

    [ForeignKey(nameof(CommentId))]
    [InverseProperty("Votes")]
    public Comment Comment { get; set; } = null!;
}