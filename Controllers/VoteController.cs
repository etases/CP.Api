using CP.Api.DTOs.Response;
using CP.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace CP.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VoteController : ControllerBase
{
    private readonly IVoteService _voteService;

    public VoteController(IVoteService voteService)
    {
        _voteService = voteService;
    }

    //create comment vote
    [HttpPost("{accountId}/{commentId}/{isUpvote}")]
    public ActionResult<ResponseDTO<bool>> Vote(int accountId, int commentId, bool isUpvote)
    {
        bool result = _voteService.Vote(accountId, commentId, isUpvote);
        if (result)
        {
            return Ok(new ResponseDTO<bool> { Success = true, Message = "Vote successfully" });
        }
        else
        {
            return BadRequest(new ResponseDTO<bool> { Success = false, Message = "Vote failed" });
        }
    }

    //get if user have voted the comment
    [HttpGet("{accountId}/{commentId}")]
    public ActionResult<ResponseDTO<bool>> HasVoted(int accountId, int commentId)
    {
        bool result = _voteService.HasVoted(accountId, commentId);
        if (result)
        {
            return Ok(new ResponseDTO<bool> { Success = true, Message = "User have voted the comment" });
        }
        else
        {
            return BadRequest(new ResponseDTO<bool> { Success = false, Message = "User haven't voted the comment" });
        }
    }

    //remove comment vote
    [HttpDelete("{accountId}/{commentId}")]
    public ActionResult<ResponseDTO<bool>> RemoveVote(int accountId, int commentId)
    {
        bool result = _voteService.Unvote(accountId, commentId);
        if (result)
        {
            return Ok(new ResponseDTO<bool> { Success = true, Message = "Remove vote successfully" });
        }
        else
        {
            return BadRequest(new ResponseDTO<bool> { Success = false, Message = "Remove vote failed" });
        }
    }
}