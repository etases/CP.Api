using CP.Api.Context;
using CP.Api.DTOs;
using CP.Api.Interfaces;
using CP.Api.Models;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Text;

namespace CP.Api.Services
{
    public class AccountServices : IAccountService
    {
        private readonly ApplicationDbContext context;

        public AccountServices(ApplicationDbContext context)
        {
            this.context = context;
        }

        public LoginResponse Login(string username, string password)
        {
            var existingUser = context.Accounts.Where(a => a.Username == username && a.IsBanned == false && a.IsDisabled == false).SingleOrDefault(a => Hasher.HashPassword(a.SaltedPassword, password) == a.HashedPassword);

            return existingUser != null ? new LoginResponse {Username = username, RoleId = existingUser.RoleId} : null;
        }

        public RegisterResponse CreateAccount(Account account)
        {
            var existingAccount = context.Accounts.FirstOrDefault(a => a.Username == account.Username);

            if (existingAccount != null)
            {
                return null;
            }

            var salt = Hasher.GenerateSalt();
            var hashedPassword = Hasher.HashPassword(salt, account.HashedPassword);
            var newAccount = new Account() 
            { 
                Username = account.Username, 
                HashedPassword = hashedPassword,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Address = account.Address,
                SaltedPassword = salt
            };

            RegisterResponse res = (RegisterResponse)newAccount;

            context.Accounts.Add(newAccount);
            context.SaveChanges();

            return res;

        }

        public UpdateProfileResponse UpdateProfile(Account account)
        {
            var existingUser = context.Accounts.FirstOrDefault(a => a.Username == account.Username && a.IsBanned == false && a.IsDisabled == false);

            if (existingUser == null)
                return null;

            Account newInfo = context.Accounts.Single(a => a.Username == account.Username);
            newInfo.FirstName = account.FirstName ?? newInfo.FirstName;
            newInfo.LastName = account.LastName ?? newInfo.LastName;
            newInfo.Address = account.Address ?? newInfo.Address;

            context.SaveChanges();

            UpdateProfileResponse res = (UpdateProfileResponse)newInfo;
            return res;
        }

        public void DisableAccount(int id)
        {
            Account tmpAccount = context.Accounts.Single(a => a.Id == id);
            tmpAccount.IsDisabled = true;
            context.SaveChanges();
        }

        public void EnableAccount(int id)
        {
            Account tmpAccount = context.Accounts.Single(a => a.Id == id);
            tmpAccount.IsDisabled = false;
            context.SaveChanges();
        }

        public void BanAccount(int id)
        {
            Account tmpAccount = context.Accounts.Single(a => a.Id == id);
            tmpAccount.IsBanned = true;
            context.SaveChanges();
        }

        public void UnbanAccount(int id)
        {
            Account tmpAccount = context.Accounts.Single(a => a.Id == id);
            tmpAccount.IsBanned = false;
            context.SaveChanges();
        }
    }
}
