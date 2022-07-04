using CP.Api.DTOs.Comment;
using CP.Api.DTOs.Response;
using CP.Api.Extensions;
using CP.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CP.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService, IConfiguration configuration)
    {
        _commentService = commentService;
    }

    [HttpGet("{id}")]
    public ActionResult<ResponseDTO<CommentOutput>> Get(int id)
    {
        CommentOutput? comment = _commentService.GetComment(id);
        if (comment == null)
        {
            return NotFound(new ResponseDTO {Message = "Comment not found", Success = false});
        }

        return Ok(new ResponseDTO<CommentOutput> {Data = comment, Success = true, Message = "Comment found"});
    }

    [HttpGet("Category/{id}")]
    public PaginationResponseDTO<CommentOutput> GetByCategory(int id, PaginationParameter parameter)
    {
        var pagedOutput = _commentService.GetCommentByCategory(id)
            .GetCount(out var count)
            .GetPage(parameter);
        return new PaginationResponseDTO<CommentOutput>
        {
            Data = pagedOutput, Success = true, Message = "Get comments successfully", TotalRecords = count
        };
    }

    [HttpGet("Parent/{id}")]
    public PaginationResponseDTO<CommentOutput> GetByParent(int id, PaginationParameter parameter)
    {
        var pagedOutput = _commentService.GetCommentByParent(id)
            .GetCount(out var count)
            .GetPage(parameter);
        return new PaginationResponseDTO<CommentOutput>
        {
            Data = pagedOutput, Success = true, Message = "Get comments successfully", TotalRecords = count
        };
    }

    [HttpPost]
    [Authorize]
    public ActionResult<ResponseDTO<CommentOutput>> AddComment(CommentInput comment)
    {
        int userId = int.Parse(User.FindFirst("Id")!.Value);
        CommentOutput? c = _commentService.AddComment(userId, comment);
        if (c == null)
        {
            return BadRequest(new ResponseDTO {Message = "Can not add Comment", Success = false});
        }

        return Ok(new ResponseDTO<CommentOutput> {Data = c, Success = true, Message = "Comment added"});
    }

    [HttpPut("{id}")]
    [Authorize]
    public ActionResult<ResponseDTO<CommentOutput>> UpdateComment(int id, CommentUpdate comment)
    {
        CommentOutput? c = _commentService.UpdateComment(id, comment);
        if (c == null)
        {
            return NotFound(new ResponseDTO {Message = "Comment not found", Success = false});
        }

        return Ok(new ResponseDTO<CommentOutput> {Data = c, Success = true, Message = "Comment updated"});
    }


    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult<ResponseDTO> DeleteComment(int id)
    {
        if (!_commentService.DeleteComment(id))
        {
            return NotFound(new ResponseDTO<CommentOutput> {Message = "Comment not found", Success = false});
        }

        return Ok(new ResponseDTO {Success = true, Message = "Comment deleted"});
    }
}