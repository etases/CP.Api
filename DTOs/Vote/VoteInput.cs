namespace CP.Api.DTOs.Vote;

public class VoteInput
{
    public int CommentId { get; set; }
    public bool IsUpvote { get; set; } // true = upvote, false = downvote
}