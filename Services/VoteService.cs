using AutoMapper;

using CP.Api.Context;
using CP.Api.Models;

namespace CP.Api.Services;

public class VoteService : IVoteService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public VoteService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //check if user has voted the commend
    public bool HasVoted(int accountId, int commentId)
    {
        var vote = _context.Votes.SingleOrDefault(v => v.AccountId == accountId && v.CommentId == commentId);
        if (vote == null)
        {
            return false;
        }

        return true;
    }


    //implement vote action
    public bool Vote(int accountId, int commentId, bool isUpvote)
    {
        var vote = new Vote { AccountId = accountId, CommentId = commentId, IsUpvote = isUpvote };


        if (vote != null && isUpvote != vote.IsUpvote)
        {
            _context.Votes.Add(vote);
            _context.SaveChanges();
        }
        else if (vote != null && isUpvote == vote.IsUpvote)
        {
            _context.Votes.Remove(vote);
            _context.SaveChanges();
        }
        else
        {
            return false;
        }


        return true;
    }

    // unvote a comment
    public bool Unvote(int accountId, int commentId)
    {
        var vote = _context.Votes.SingleOrDefault(v => v.AccountId == accountId && v.CommentId == commentId);
        if (vote != null)
        {
            _context.Votes.Remove(vote);
            _context.SaveChanges();
        }
        else
        {
            return false;
        }

        return true;
    }

    //get vote count
    public int GetVoteCount(int commentId)
    {
        var votes = _context.Votes.Where(v => v.CommentId == commentId);
        var upvotes = votes.Where(v => v.IsUpvote).Count();
        var downvotes = votes.Where(v => !v.IsUpvote).Count();

        return upvotes - downvotes;
    }
}

public interface IVoteService
{
    bool Vote(int accountId, int commentId, bool isUpvote);

    //unvote if already voted
    bool Unvote(int accountId, int commentId);
    bool HasVoted(int accountId, int commentId);

    int GetVoteCount(int commentId);
}