using AutoMapper;

using CP.Api.DTOs.Account;
using CP.Api.Models;

namespace CP.Api.Profiles;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<Account, AccountOutput>();
        CreateMap<RegisterInput, Account>();
    }
}