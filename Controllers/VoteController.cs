using CP.Api.DTOs.Response;
using CP.Api.DTOs.Vote;
using CP.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace CP.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VoteController : ControllerBase
{
    private readonly IVoteService _voteService;
    private readonly IAccountService _accountService;

    public VoteController(IVoteService voteService, IAccountService accountService)
    {
        _voteService = voteService;
        _accountService = accountService;
    }

    //handle comment vote
    [HttpPost]
    public ActionResult<ResponseDTO> HandleVote(VoteDTO voteDTO)
    {
        if (_accountService.GetAccount(voteDTO.AccountId) == null)
        {
            return NotFound(new ResponseDTO { ErrorCode = 1, Message = "Account Not Found" });
        }
        // TODO: check if comment exists
        if (false)
        {
            return NotFound(new ResponseDTO { ErrorCode = 2, Message = "Comment Not Found" });
        }
        _voteService.HandleVote(voteDTO);
        return Ok(new ResponseDTO { Success = true, Message = "Handle Vote successfully" });
    }

    //get if user have voted the comment
    [HttpGet("{accountId}/{commentId}")]
    public ActionResult<ResponseDTO<bool>> HasVoted(int accountId, int commentId)
    {
        if (_accountService.GetAccount(accountId) == null)
        {
            return NotFound(new ResponseDTO { ErrorCode = 1, Message = "Account Not Found" });
        }
        // TODO: check if comment exists
        if (false)
        {
            return NotFound(new ResponseDTO { ErrorCode = 2, Message = "Comment Not Found" });
        }
        bool hasVoted = _voteService.HasVoted(accountId, commentId);
        return Ok(new ResponseDTO<bool> { Success = true, Message = "Check Data For Has Voted Status", Data = hasVoted });
    }

    //get vote count
    [HttpGet("{commentId}")]
    public ActionResult<ResponseDTO<int>> GetVoteCount(int commentId)
    {
        // TODO: check if comment exists
        if (false)
        {
            return NotFound(new ResponseDTO { ErrorCode = 2, Message = "Comment Not Found" });
        }
        int result = _voteService.GetVoteCount(commentId);
        return Ok(new ResponseDTO<int> { Success = true, Message = "Get vote count successfully", Data = result });
    }
}