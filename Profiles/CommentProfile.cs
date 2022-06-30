using AutoMapper;

using CP.Api.DTOs.Comment;
using CP.Api.Models;

namespace CP.Api.Profiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentOutput>()
            .AfterMap((src, des) =>
            {
                if (!src.IsDeleted) return;
                des.Content = string.Empty;
                des.Keyword = string.Empty;
            });
        CreateMap<CommentInput, Comment>();
        CreateMap<CommentUpdate, Comment>();
    }
}