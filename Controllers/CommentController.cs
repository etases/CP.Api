using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CP.Api.Core.Models;
using CP.Api.DTOs.Account;
using CP.Api.DTOs.Comment;
using CP.Api.DTOs.Response;
using CP.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace CP.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IConfiguration _configuration;

        public CommentController(ICommentService commentService, IConfiguration configuration)
        {
            _commentService = commentService;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public ActionResult<ResponseDTO<CommentOutput>> Get(int id)
        {
            CommentOutput? comment = _commentService.GetComment(id);
            if (comment == null)
            {
                return NotFound(new ResponseDTO{ Message = "Comment not found", Success = false });
            }

            return Ok(new ResponseDTO<CommentOutput> { Data = comment, Success = true, Message = "Comment found" });
        }

        [HttpGet("Category/{id}")]
        public ActionResult<ResponseDTO<CommentOutput>> GetByCategory(int id)
        {
            ICollection<CommentOutput> comment = _commentService.GetCommentByCategory(id);
            if (comment == null)
            {
                return NotFound(new ResponseDTO { Message = "Comment not found", Success = false });
            }

            return Ok(new ResponseDTO<ICollection<CommentOutput>> { Data = comment, Success = true, Message = "Comment found" });
        }

        [HttpGet("Parent/{id}")]
        public ActionResult<ResponseDTO<CommentOutput>> GetByParent(int id)
        {
            ICollection<CommentOutput> comment = _commentService.GetCommentByCategory(id);
            if (comment == null)
            {
                return NotFound(new ResponseDTO { Message = "Comment not found", Success = false });
            }

            return Ok(new ResponseDTO<ICollection<CommentOutput>> { Data = comment, Success = true, Message = "Comment found" });
        }

        [HttpPost]
        //[Authorize]
        public ActionResult<ResponseDTO<CommentOutput>> AddComment(CommentInput comment)
        {
            //var userId = int.Parse(User.FindFirst("Id")!.Value);
            var userId = 1;
            CommentOutput c = _commentService.AddComment(userId, comment);
            if (c == null)
            {
                return BadRequest(new ResponseDTO { Message = "Can not add Comment", Success = false });
            }

            return Ok(new ResponseDTO<CommentOutput> { Data = c, Success = true, Message = "Comment added" });
        }

        [HttpPut("{id}")]
        //[Authorize]
        public ActionResult<ResponseDTO<CommentOutput>> UpdateComment(int id, CommentUpdate comment)
        {
            CommentOutput c = _commentService.UpdateComment(id, comment);
            if (c == null)
            {
                return NotFound(new ResponseDTO { Message = "Comment not found", Success = false });
            }

            return Ok(new ResponseDTO<CommentOutput> { Data = c, Success = true, Message = "Comment updated" });
        }


        [HttpDelete("{id}")]
        //[Authorize]
        public ActionResult<ResponseDTO> DeleteComment(int id)
        {
            if (!_commentService.DeleteComment(id))
            {
                return NotFound(new ResponseDTO<CommentOutput> { Message = "Comment not found", Success = false });
            }

            return Ok(new ResponseDTO { Success = true, Message = "Comment deleted" });
        }


    }

}
