using CP.Api.DTOs;
using CP.Api.Models;

namespace CP.Api.Interfaces
{
    public interface IAccountService
    {
        LoginResponse Login(string Username, string Password);
        RegisterResponse CreateAccount(Account account);
        UpdateProfileResponse UpdateProfile(Account account);
        void DisableAccount(int id);
        void EnableAccount(int id);
        void BanAccount(int id);
        void UnbanAccount(int id);
    }
}
