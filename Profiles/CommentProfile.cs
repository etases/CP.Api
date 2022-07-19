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
                if (src.IsDeleted)
                {
                    des.Content = string.Empty;
                    des.Keyword = string.Empty;
                }

                des.HasChild = src.Children != null && src.Children.Any();
            });
        CreateMap<CommentInput, Comment>();
        CreateMap<CommentUpdate, Comment>();
    }
}