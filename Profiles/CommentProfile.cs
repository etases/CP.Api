using AutoMapper;

using CP.Api.DTOs.Comment;
using CP.Api.Models;

namespace CP.Api.Profiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentOutput>();
        CreateMap<CommentInput, Comment>();
        CreateMap<CommentUpdate, Comment>();
    }
}