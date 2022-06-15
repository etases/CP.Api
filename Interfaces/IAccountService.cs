using CP.Api.Models;

namespace CP.Api.Interfaces
{
    public interface IAccountService
    {
        LoginResponse Login(string Username, string Password);
        (bool ok, int id) CreateAccount(Account req);
        bool VerifyAccount(string token);
        Account GetUser(string token);
        void Logout(Account account);
        bool ChangeInfo(Account account);
        void DisableAccount(Account account);
        void EnableAccount(Account account);
        void BanAccount(Account account);
        void UnbanAccount(Account account);

        void ExpireToken(string token);
        void ExpireRefreshToken(string token);
    }
}
