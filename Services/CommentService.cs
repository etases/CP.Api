using CP.Api.Context;

using AutoMapper;

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
    private IQueryable<Comment> GetComments()
    {
        var comments = _context.Comments.Include(c => c.Account).Include(c => c.Category).AsQueryable();

        return comments;
    }

    public CommentOutput? GetComment(int id)
    {
        var comment = GetComments().SingleOrDefault(c => c.Id == id);
        if (comment == null)
        {
            return null;
        }
        var output = _mapper.Map<CommentOutput>(comment);
        return output;
    }

    public ICollection<CommentOutput> GetCommentByCategory(int id)
    {
        var comment = GetComments().Where(c => c.CategoryId == id).ToList();
        if (comment == null)
        {
            return null;
        }
        var output = _mapper.Map<ICollection<CommentOutput>>(comment);

        return output;
    }

    public ICollection<CommentOutput> GetCommentByParent(int id)
    {
        var comment = GetComments().Where(c => c.ParentId == id).ToList();
        if (comment == null)
        {
            return null;
        }
        var output = _mapper.Map<ICollection<CommentOutput>>(comment);

        return output;
    }

    public CommentOutput? AddComment(int userId, CommentInput comment)
    {
        Comment c = _mapper.Map<Comment>(comment);
        c.AccountId = userId;
        _context.Comments.Add(c);
        _context.SaveChanges();
        CommentOutput output = _mapper.Map<CommentOutput>(c);
        return output;
    }

    public CommentOutput? UpdateComment(int id, CommentUpdate comment)
    {
        var c = GetComments().SingleOrDefault(c => c.Id == id);
        if (c == null)
        {
            return null;
        }
        _mapper.Map(comment, c);
        _context.Comments.Update(c);
        _context.SaveChanges();

        CommentOutput output = _mapper.Map<CommentOutput>(c);
        return output;
    }

    public bool DeleteComment(int id)
    {
        var comment = GetComments().SingleOrDefault(c => c.Id == id);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return true;
        }
        return false;
    }
}

public interface ICommentService
{
    CommentOutput? GetComment(int id);
    ICollection<CommentOutput> GetCommentByCategory(int cateId);
    ICollection<CommentOutput> GetCommentByParent(int parentId);
    CommentOutput? AddComment(int userId, CommentInput comment);
    CommentOutput? UpdateComment(int commentId, CommentUpdate comment);
    bool DeleteComment(int commentId);
}
