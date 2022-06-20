using CP.Api.DTOs.Account;

namespace CP.Api.Interfaces
{
    public interface IAccountService
    {
        (bool found, AccountOutput? output) Login(LoginInput input);
        (bool existed, AccountOutput? output) Register(RegisterInput input);
        AccountOutput? UpdateProfile(int id, UpdateProfileInput input);
        bool SetDisableStatus(int id, bool disable);
        bool SetBanStatus(int id, bool ban);
    }
}
