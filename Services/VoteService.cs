using AutoMapper;

using CP.Api.Context;
using CP.Api.DTOs.Vote;
using CP.Api.Models;

using Microsoft.EntityFrameworkCore;

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
        Vote? vote = _context.Votes.SingleOrDefault(v => v.AccountId == accountId && v.CommentId == commentId);
        return vote != null;
    }


    //implement vote action
    public void HandleVote(VoteDTO voteDTO)
    {
        Vote? vote =
            _context.Votes.SingleOrDefault(v => v.AccountId == voteDTO.AccountId && v.CommentId == voteDTO.CommentId);

        if (vote != null)
        {
            if (vote.IsUpvote != voteDTO.IsUpvote)
            {
                vote.IsUpvote = voteDTO.IsUpvote;
                _context.Votes.Update(vote);
            }
            else
            {
                _context.Votes.Remove(vote);
            }
        }
        else
        {
            vote = _mapper.Map<Vote>(voteDTO);
            _context.Votes.Add(vote);
        }

        _context.SaveChanges();
    }

    //get vote count
    public int? GetVoteCount(int commentId)
    {
        ICollection<Vote>? votes = _context.Comments.Include(c => c.Votes).SingleOrDefault(c => c.Id == commentId)
            ?.Votes;
        if (votes == null)
        {
            return null;
        }

        int upVote = votes.Count(v => v.IsUpvote);
        int downVote = votes.Count(v => !v.IsUpvote);

        return upVote - downVote;
    }
}

public interface IVoteService
{
    void HandleVote(VoteDTO voteDTO);
    bool HasVoted(int accountId, int commentId);
    int? GetVoteCount(int commentId);
}