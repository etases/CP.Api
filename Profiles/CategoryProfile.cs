using AutoMapper;

using CP.Api.DTOs;
using CP.Api.Models;

namespace CP.Api.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryDTO, Category>();
    }
}