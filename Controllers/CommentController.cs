using CP.Api.DTOs.Comment;
using CP.Api.DTOs.Response;
using CP.Api.Extensions;
using CP.Api.Models;
using CP.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CP.Api.Controllers;

/// <summary>
///     Comment API controller
/// </summary>
[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    /// <summary>
    ///     Comment controller constructor
    /// </summary>
    /// <param name="commentService">Comment service</param>
    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    /// <summary>
    ///     Get comment by id
    /// </summary>
    /// <param name="id">Id of the comment</param>
    /// <returns>ResponseDTO <seealso cref="CommentOutput" /></returns>
    [HttpGet("{id}")]
    public ActionResult<ResponseDTO<CommentOutput>> Get(int id)
    {
        CommentOutput? comment = _commentService.GetComment(id);
        return comment switch
        {
            null => NotFound(new ResponseDTO<CommentOutput> {Message = "Comment not found", Success = false}),
            _ => Ok(new ResponseDTO<CommentOutput> {Data = comment, Success = true, Message = "Comment found"})
        };
    }

    /// <summary>
    ///     Get topic by category id
    /// </summary>
    /// <remarks>
    ///     FIXME: Rename endpoint for better readability
    /// </remarks>
    /// <param name="id">Id of the category</param>
    /// <param name="parameter">Pagination parameter</param>
    /// <returns>PaginationResponseDTO <seealso cref="CommentOutput" /></returns>
    [HttpGet("Category/{id}")]
    public PaginationResponseDTO<CommentOutput> GetByCategory(int id, PaginationParameter parameter)
    {
        PaginatedEnumerable<CommentOutput> pagedOutput = _commentService.GetCommentByCategory(id).GetPage(parameter);
        return new PaginationResponseDTO<CommentOutput>
        {
            Data = pagedOutput.Items,
            Success = true,
            Message = "Get comments successfully",
            TotalRecord = pagedOutput.TotalRecord,
            TotalPage = pagedOutput.TotalPage,
            PageNumber = pagedOutput.PageNumber,
            PageSize = pagedOutput.PageSize,
            HasNextPage = pagedOutput.HasNextPage,
            HasPreviousPage = pagedOutput.HasPreviousPage
        };
    }

    /// <summary>
    ///     Get comment by parent comment id
    /// </summary>
    /// <remarks>
    ///     FIXME: Rename endpoint for better readability
    /// </remarks>
    /// <param name="id">Id of parent id</param>
    /// <returns>ResponseDTO <seealso cref="CommentOutput" /></returns>
    [HttpGet("Parent/{id}")]
    public ActionResult<ResponseDTO<ICollection<CommentOutput>>> GetByParent(int id)
    {
        ICollection<CommentOutput> comment = _commentService.GetCommentByParent(id);
        return Ok(new ResponseDTO<ICollection<CommentOutput>>
        {
            Data = comment, Success = true, Message = "Get comments successfully"
        });
    }

    /// <summary>
    ///     Add new comment
    /// </summary>
    /// <param name="comment">Detail of the comment</param>
    /// <returns>ResponseDTO <seealso cref="CommentOutput" /></returns>
    [HttpPost]
    [Authorize]
    public ActionResult<ResponseDTO<CommentOutput>> AddComment(CommentInput comment)
    {
        int userId = int.Parse(User.FindFirst("Id")!.Value);
        CommentOutput? c = _commentService.AddComment(userId, comment);
        return c switch
        {
            null => NotFound(new ResponseDTO<CommentOutput> {Message = "Comment add failed", Success = false}),
            _ => Ok(new ResponseDTO<CommentOutput> {Data = c, Success = true, Message = "Comment added"})
        };
    }

    /// <summary>
    ///     Update comment by id
    /// </summary>
    /// <param name="id">Id of the comment</param>
    /// <param name="comment">Data of the comment</param>
    /// <returns>ResponseDTO <seealso cref="CommentOutput" /></returns>
    [HttpPut("{id}")]
    [Authorize]
    public ActionResult<ResponseDTO<CommentOutput>> UpdateComment(int id, CommentUpdate comment)
    {
        int userId = int.Parse(User.FindFirst("Id")!.Value);
        bool isAdminRole = User.IsInRole(DefaultRoles.AdministratorString);
        CommentOutput? c = _commentService.UpdateComment(id, comment, userId, isAdminRole);
        return c switch
        {
            null => NotFound(new ResponseDTO<CommentOutput> {Message = "Comment not found", Success = false}),
            _ => Ok(new ResponseDTO<CommentOutput> {Data = c, Success = true, Message = "Comment updated"})
        };
    }

    /// <summary>
    ///     Delete comment by id
    /// </summary>
    /// <param name="id">Id of the comment</param>
    /// <returns>ResponseDTO</returns>
    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult<ResponseDTO> DeleteComment(int id)
    {
        int userId = int.Parse(User.FindFirst("Id")!.Value);
        bool bypassCheck = User.IsInRole(DefaultRoles.AdministratorString) || User.IsInRole(DefaultRoles.ManagerString);
        bool success = _commentService.DeleteComment(id, userId, bypassCheck);
        return success switch
        {
            true => Ok(new ResponseDTO {Success = true, Message = "Comment deleted"}),
            _ => NotFound(new ResponseDTO {Message = "Comment not found", Success = false})
        };
    }
}