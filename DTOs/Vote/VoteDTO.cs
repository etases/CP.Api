namespace CP.Api.DTOs.Vote;

public class VoteDTO
{
    public int AccountId { get; set; }
    public int CommentId { get; set; }
    public bool IsUpvote { get; set; } // true = upvote, false = downvote
}