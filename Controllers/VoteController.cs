using CP.Api.DTOs.Response;
using CP.Api.DTOs.Vote;
using CP.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace CP.Api.Controllers
{
    /// <summary>
    /// Vote API controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ICommentService _commentService;
        private readonly IVoteService _voteService;

        /// <summary>
        /// Vote controller constructor
        /// </summary>
        /// <param name="voteService">Vote service</param>
        /// <param name="accountService">Account service</param>
        /// <param name="commentService">Comment service</param>
        public VoteController(IVoteService voteService, IAccountService accountService, ICommentService commentService)
        {
            _voteService = voteService;
            _accountService = accountService;
            _commentService = commentService;
        }

        /// <summary>
        /// Handle comment vote
        /// </summary>
        /// <remarks>
        /// FIXME: Handle return status code if action failed
        /// </remarks>
        /// <param name="voteDTO">Vote data</param>
        /// <returns>ResponseDTO</returns>
        [HttpPost]
        public ActionResult<ResponseDTO> HandleVote(VoteDTO voteDTO)
        {
            DTOs.Account.AccountOutput? account = _accountService.GetAccount(voteDTO.AccountId);
            DTOs.Comment.CommentOutput? comment = _commentService.GetComment(voteDTO.CommentId);

            if (account == null)
            {
                return NotFound(new ResponseDTO { ErrorCode = 1, Message = "Account Not Found" });
            }

            if (comment == null)
            {
                return NotFound(new ResponseDTO { ErrorCode = 2, Message = "Comment Not Found" });
            }

            _voteService.HandleVote(voteDTO);
            return Ok(new ResponseDTO { Success = true, Message = "Handle Vote successfully" });
        }

        /// <summary>
        /// Check if user have voted the comment
        /// </summary>
        /// <param name="accountId">Id of the account</param>
        /// <param name="commentId">Id of the comment</param>
        /// <returns>ResponseDTO</returns>
        [HttpGet("{accountId}/{commentId}")]
        public ActionResult<ResponseDTO<bool>> HasVoted(int accountId, int commentId)
        {
            DTOs.Account.AccountOutput? account = _accountService.GetAccount(accountId);
            DTOs.Comment.CommentOutput? comment = _commentService.GetComment(commentId);

            if (account == null)
            {
                return NotFound(new ResponseDTO { ErrorCode = 1, Message = "Account Not Found" });
            }

            if (comment == null)
            {
                return NotFound(new ResponseDTO { ErrorCode = 2, Message = "Comment Not Found" });
            }

            bool hasVoted = _voteService.HasVoted(accountId, commentId);
            return Ok(new ResponseDTO<bool> { Success = true, Message = "Check Data For Has Voted Status", Data = hasVoted });
        }

        /// <summary>
        /// get vote count of comment
        /// </summary>
        /// <param name="commentId">Id of the comment</param>
        /// <returns>ResponseDTO</returns>
        [HttpGet("{commentId}")]
        public ActionResult<ResponseDTO<int>> GetVoteCount(int commentId)
        {
            DTOs.Comment.CommentOutput? comment = _commentService.GetComment(commentId);
            if (comment == null)
            {
                return NotFound(new ResponseDTO { ErrorCode = 2, Message = "Comment Not Found" });
            }

            int result = _voteService.GetVoteCount(commentId);
            return Ok(new ResponseDTO<int> { Success = true, Message = "Get vote count successfully", Data = result });
        }
    }
}
