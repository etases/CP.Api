using CP.Api.Context;
using CP.Api.Models;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Text;

namespace CP.Api.Services
{
    public class AccountServices
    {
        private readonly ApplicationDbContext context;

        public AccountServices(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool CreateAccount(Account account)
        {
            bool ok = false;

            var existingAccount = context.Accounts.Where(a => a.Username == account.Username && !a.IsBanned);

            if (existingAccount.Count() == 0)
            {
                var salt = Hasher.GenerateSalt();
                var hashedPassword = Hasher.HashPassword(salt, account.HashedPassword);
                var newAccount = new Account() { Username = account.Username, HashedPassword = hashedPassword, SaltedPassword = salt };
                context.Accounts.Add(newAccount);
                context.SaveChanges();
                ok = true;
            }

            return ok;
        }

        public bool ChangeInfo(Account account)
        {
            bool ok = false;
            var existingUsers = context.Accounts.Where(a => a.Username == account.Username && a.IsBanned == false);

            if (existingUsers.Count() == 1 || existingUsers.First().Username == account.Username)
            {
                var newInfo = context.Accounts.Single(a => a.Username == account.Username);
                newInfo.SaltedPassword = Hasher.GenerateSalt();
                newInfo.Username = account.Username ?? newInfo.Username;
                newInfo.FirstName = account.FirstName ?? newInfo.FirstName;
                newInfo.LastName = account.LastName ?? newInfo.LastName;
                newInfo.Address = account.Address ?? newInfo.Address;
                newInfo.HashedPassword = Hasher.HashPassword(newInfo.SaltedPassword, account.HashedPassword);
                context.SaveChanges();
                ok = true;
            }

            return ok;
        }

        public void DisableAccount(Account account)
        {
            var TmpAccount = context.Accounts.Single(a => a.Username == account.Username);
            TmpAccount.IsDisabled = true;
            context.SaveChanges();
        }

        public void EnableAccount(Account account)
        {
            var TmpAccount = context.Accounts.Single(a => a.Username == account.Username);
            TmpAccount.IsDisabled = false;
            context.SaveChanges();
        }

        public void BanAccount(Account account)
        {
            var TmpAccount = context.Accounts.Single(a => a.Username == account.Username);
            TmpAccount.IsBanned = true;
            context.SaveChanges();
        }

        public void UnbanAccount(Account account)
        {
            var TmpAccount = context.Accounts.Single(a => a.Username == account.Username);
            TmpAccount.IsBanned = false;
            context.SaveChanges();
        }

        //public LoginResponse Refresh(string refreshToken)
        //{
        //    LoginResponse response = null;

        //    var user = context.Accounts
        //        .Where(u => u.RefreshToken == refreshToken && u.IsBanned == false).SingleOrDefault();

        //    if (user != null)
        //    {
        //        var ts = GetEpoch();

        //        // Refresh token expires 90 days after when user logged in, thus ExpiresOn + (90 - 1) days
        //        if (user.ExpiresOn + (Constants.REFRESH_VALID_DAYS - 1) * Constants.ONE_DAY_IN_SECONDS > ts)
        //        {
        //            user.Login(ts);
        //            context.SaveChanges();
        //            response = user.CreateMapped<LoginResponse>();
        //        }
        //    }

        //    return response;
        //}

        //public void ExpireRefreshToken(string token)
        //{
        //    var ts = GetEpoch();
        //    var user = context.Accounts.SingleOrDefault(u => u.AccessToken == token);
        //    user.ExpiresOn = ts - Constants.REFRESH_VALID_DAYS * Constants.ONE_DAY_IN_SECONDS;
        //    context.SaveChanges();
        //}

        //public bool VerifyAccount(string token)
        //{
        //    var user = context.Accounts.Where(u => u.AccessToken == token).SingleOrDefault();
        //    var ts = GetEpoch();
        //    bool ok = (user?.ExpiresOn ?? 0) > ts;

        //    return ok;
        //}



        // Chua biet cach lam JWT ;-;
        public LoginResponse Login(string Username, string Password)
        {
            string key = "093847c5fh32049857cfh230498v5723-409c5f72304j9587234j 0512-656d6gb9sr8dtr4uyf321kfgnsodfnalkdqw564er68u17yd68u41rs68a7w3v1bt6a61";

            // Create Security key using private key above
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Create a Token
            var header = new JwtHeader(credentials);

            //Some PayLoad that contain information about the  customer
            var payload = new JwtPayload
           {
               { Username, Password}
           };

            //
            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            // Token to String so you can use it in your client
            var tokenString = handler.WriteToken(secToken);

            var token = handler.ReadJwtToken(tokenString);

            var existingUsers = context.Accounts.Where(a => a.Username == Username && a.IsBanned == false).ToList();

            var existingUser = context.Accounts.Where(a => a.Username == Username && a.IsBanned == false)
                .ToList().SingleOrDefault(a => Hasher.HashPassword(a.SaltedPassword, Password) == a.HashedPassword);

            if (existingUser != null)
                return new LoginResponse
                {
                    Status = true,
                    Accounts = existingUser,
                    JWT = token.Payload.First().Value
                };
            else
                return new LoginResponse
                {
                    Status = false,
                    Accounts = null,
                    JWT = token.Payload.First().Value
                };
        }

        public void Logout(Account account)
        {
            var user = context.Accounts.Single(u => u.Username == account.Username);
            user.Logout();
            context.SaveChanges();
        }

        //public void ExpireToken(string token)
        //{
        //    var ts = GetEpoch();
        //    var user = context.Accounts.SingleOrDefault(u => u.AccessToken == token);
        //    user.ExpiresOn = ts - Constants.ONE_DAY_IN_SECONDS;
        //    context.SaveChanges();
        //}

        //private long GetEpoch()
        //{
        //    var ts = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;   // We declare the epoch to be 1/1/1970.

        //    return ts;
        //}
    }
}
