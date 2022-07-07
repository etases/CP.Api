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

    public CommentService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public CommentOutput? GetComment(int id)
    {
        var comment = GetComments().SingleOrDefault(c => c.Id == id);
        if (comment == null)
            return null;

        var output = _mapper.Map<CommentOutput>(comment);
        return output;
    }

    public ICollection<CommentOutput> GetCommentByCategory(int id, bool includeChild)
    {
        var comment = GetComments().Where(c => c.CategoryId == id);
        if (!includeChild)
        {
            comment = comment.Where(c => c.ParentId == null);
        }
        return _mapper.Map<ICollection<CommentOutput>>(comment.ToList());
    }

    public ICollection<CommentOutput> GetCommentByParent(int id)
    {
        var comment = GetComments().Where(c => c.ParentId == id).ToList();
        return _mapper.Map<ICollection<CommentOutput>>(comment);
    }

    public CommentOutput? AddComment(int userId, CommentInput commentInput)
    {
        var c = _mapper.Map<Comment>(commentInput);
        c.AccountId = userId;
        var parentId = commentInput.ParentId;
        if (parentId != null && GetComment(parentId.Value) == null) return null;
        _context.Comments.Add(c);
        _context.SaveChanges();
        var output = _mapper.Map<CommentOutput>(c);
        return output;
    }

    public CommentOutput? UpdateComment(int id, CommentUpdate commentUpdate, int userId, bool bypassCheck)
    {
        var c = GetComments().SingleOrDefault(c => c.Id == id && c.IsDeleted == false);
        if (c == null)
            return null;

        if (!bypassCheck)
        {
            if (c.AccountId != userId)
                return null;
        }

        _mapper.Map(commentUpdate, c);
        _context.Comments.Update(c);
        _context.SaveChanges();

        CommentOutput output = _mapper.Map<CommentOutput>(c);
        return output;
    }

    public bool DeleteComment(int id, int userId, bool bypassCheck)
    {
        var comment = GetComments().SingleOrDefault(c => c.Id == id && c.IsDeleted == false);
        if (comment == null)
            return false;
        
        if (!bypassCheck)
        {
            if (comment.AccountId != userId)
                return false;
        }

        comment.IsDeleted = true;
        _context.Comments.Update(comment);
        _context.SaveChanges();
        return true;
    }

    private IQueryable<Comment> GetComments()
    {
        return _context.Comments
            .Include(c => c.Account)
            .Include(c => c.Category);
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