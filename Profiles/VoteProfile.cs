using AutoMapper;

using CP.Api.DTOs.Vote;
using CP.Api.Models;

namespace CP.Api.Profiles;

public class VoteProfile : Profile
{
    public VoteProfile()
    {
        CreateMap<VoteDTO, Vote>().ReverseMap();
    }
}