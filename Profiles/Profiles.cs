using AutoMapper;

namespace CP.Api.Profiles;

public static class Profiles
{
    public static void AddProfile(IMapperConfigurationExpression config)
    {
        config.AddProfile<AccountProfile>();
        config.AddProfile<RoleProfile>();
        config.AddProfile<VoteProfile>();
    }
}