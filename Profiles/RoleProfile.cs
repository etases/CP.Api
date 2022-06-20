using AutoMapper;

using CP.Api.DTOs.Role;
using CP.Api.Models;

namespace CP.Api.Profiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDTO>();
    }
}