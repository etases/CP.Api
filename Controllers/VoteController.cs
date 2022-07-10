using CP.Api.DTOs.Comment;
using CP.Api.DTOs.Response;
using CP.Api.DTOs.Vote;
using CP.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CP.Api.Controllers;

/// <summary>
///     Vote API controller
/// </summary>
[ApiController]
[Route("[controller]")]
public class VoteController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IVoteService _voteService;

    /// <summary>
    ///     Vote controller constructor
    /// </summary>
    /// <param name="voteService">Vote service</param>
    /// <param name="commentService">Comment service</param>
    public VoteController(IVoteService voteService, ICommentService commentService)
    {
        _voteService = voteService;
        _commentService = commentService;
    }

    /// <summary>
    ///     Handle comment vote
    /// </summary>
    /// <remarks>
    ///     FIXME: Handle return status code if action failed
    /// </remarks>
    /// <param name="voteInput">Vote data</param>
    /// <returns>ResponseDTO</returns>
    [HttpPost]
    [Authorize]
    public ActionResult<ResponseDTO> HandleVote(VoteInput voteInput)
    {
        int userId = int.Parse(User.FindFirst("Id")!.Value);
        CommentOutput? comment = _commentService.GetComment(voteInput.CommentId);

        if (comment == null)
        {
            return NotFound(new ResponseDTO {ErrorCode = 2, Message = "Comment Not Found"});
        }

        _voteService.HandleVote(new VoteDTO
        {
            AccountId = userId, CommentId = voteInput.CommentId, IsUpvote = voteInput.IsUpvote
        });
        return Ok(new ResponseDTO {Success = true, Message = "Handle Vote successfully"});
    }

    /// <summary>
    ///     Check if user have voted the comment
    /// </summary>
    /// <param name="commentId">Id of the comment</param>
    /// <returns>ResponseDTO</returns>
    [HttpGet("{commentId}")]
    [Authorize]
    public ActionResult<ResponseDTO<bool>> HasVoted(int commentId)
    {
        int userId = int.Parse(User.FindFirst("Id")!.Value);
        CommentOutput? comment = _commentService.GetComment(commentId);

        if (comment == null)
        {
            return NotFound(new ResponseDTO {ErrorCode = 1, Message = "Comment Not Found"});
        }

        bool hasVoted = _voteService.HasVoted(userId, commentId);
        return Ok(new ResponseDTO<bool> {Success = true, Message = "Check Data For Has Voted Status", Data = hasVoted});
    }

    /// <summary>
    ///     get vote count of comment
    /// </summary>
    /// <param name="commentId">Id of the comment</param>
    /// <returns>ResponseDTO</returns>
    [HttpGet("count/{commentId}")]
    public ActionResult<ResponseDTO<int>> GetVoteCount(int commentId)
    {
        int? result = _voteService.GetVoteCount(commentId);
        return result.HasValue
            ? Ok(new ResponseDTO<int> {Success = true, Message = "Get vote count successfully", Data = result.Value})
            : NotFound(new ResponseDTO {ErrorCode = 1, Message = "Comment Not Found"});
    }
}