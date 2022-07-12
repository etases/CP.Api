using AutoMapper;

using CP.Api.Context;
using CP.Api.DTOs.Comment;
using CP.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace CP.Api.Services;

public class CommentService : ICommentService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUpdateHubService _updateHubService;

    public CommentService(ApplicationDbContext context, IMapper mapper, IUpdateHubService updateHubService)
    {
        _context = context;
        _mapper = mapper;
        _updateHubService = updateHubService;
    }

    public CommentOutput? GetComment(int id)
    {
        Comment? comment = GetComments().SingleOrDefault(c => c.Id == id);
        if (comment == null)
        {
            return null;
        }

        CommentOutput? output = _mapper.Map<CommentOutput>(comment);
        return output;
    }

    public ICollection<CommentOutput> GetCommentByCategory(int id, bool includeChild)
    {
        IQueryable<Comment> comment = GetComments().Where(c => c.CategoryId == id);
        if (!includeChild)
        {
            comment = comment.Where(c => c.ParentId == null);
        }

        return _mapper.Map<ICollection<CommentOutput>>(comment.ToList());
    }

    public ICollection<CommentOutput> GetCommentByParent(int id)
    {
        List<Comment> comment = GetComments().Where(c => c.ParentId == id).ToList();
        return _mapper.Map<ICollection<CommentOutput>>(comment);
    }

    public CommentOutput? AddComment(int userId, CommentInput commentInput)
    {
        Comment? c = _mapper.Map<Comment>(commentInput);
        c.AccountId = userId;
        int? parentId = commentInput.ParentId;
        if (parentId.HasValue)
        {
            var parent = GetComment(parentId.Value);
            if (parent == null) return null;
            commentInput.CategoryId = parent.CategoryId;
        }

        _context.Comments.Add(c);
        _context.SaveChanges();

        if (parentId.HasValue)
        {
            _updateHubService.NotifyCommentUpdate(parentId.Value);
        }
        else
        {
            _updateHubService.NotifyCategoryUpdate(c.CategoryId);
        }

        CommentOutput? output = _mapper.Map<CommentOutput>(c);
        return output;
    }

    public CommentOutput? UpdateComment(int id, CommentUpdate commentUpdate, int userId, bool bypassCheck)
    {
        Comment? c = GetComments().SingleOrDefault(c => c.Id == id && c.IsDeleted == false);
        if (c == null)
        {
            return null;
        }

        if (!bypassCheck)
        {
            if (c.AccountId != userId)
            {
                return null;
            }
        }

        _mapper.Map(commentUpdate, c);
        _context.Comments.Update(c);
        _context.SaveChanges();

        _updateHubService.NotifyCommentUpdate(c.Id);
        if (!c.ParentId.HasValue)
        {
            _updateHubService.NotifyCategoryUpdate(c.CategoryId);
        }

        CommentOutput output = _mapper.Map<CommentOutput>(c);
        return output;
    }

    public bool DeleteComment(int id, int userId, bool bypassCheck)
    {
        Comment? comment = GetComments().SingleOrDefault(c => c.Id == id && c.IsDeleted == false);
        if (comment == null)
        {
            return false;
        }

        if (!bypassCheck)
        {
            if (comment.AccountId != userId)
            {
                return false;
            }
        }

        comment.IsDeleted = true;
        _context.Comments.Update(comment);
        _context.SaveChanges();

        _updateHubService.NotifyCommentUpdate(comment.Id);
        if (!comment.ParentId.HasValue)
        {
            _updateHubService.NotifyCategoryUpdate(comment.CategoryId);
        }

        return true;
    }

    private IQueryable<Comment> GetComments()
    {
        return _context.Comments
            .Include(c => c.Account)
            .Include(c => c.Category)
            .Include(c => c.Children);
    }
}

public interface ICommentService
{
    CommentOutput? GetComment(int id);
    ICollection<CommentOutput> GetCommentByCategory(int cateId, bool includeChild = false);
    ICollection<CommentOutput> GetCommentByParent(int parentId);
    CommentOutput? AddComment(int userId, CommentInput commentInput);
    CommentOutput? UpdateComment(int commentId, CommentUpdate commentUpdate, int userId, bool bypassCheck);
    bool DeleteComment(int commentId, int userId, bool bypassCheck);
}