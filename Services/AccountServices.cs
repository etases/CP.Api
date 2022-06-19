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

        public LoginResponse Login(string Username, string Password)
        {
            var existingUser = context.Accounts.Where(a => a.Username == Username && a.IsBanned == false && a.IsDisabled == false)
                .ToList().SingleOrDefault(a => Hasher.HashPassword(a.SaltedPassword, Password) == a.HashedPassword);

            if (existingUser != null)
                return new LoginResponse
                {
                    Username = Username,
                    RoleId = existingUser.RoleId
                };
            else
                return null;
        }

        public RegisterResponse CreateAccount(Account account)
        {
            //bool ok = false;

            var existingAccount = context.Accounts.Where(a => a.Username == account.Username);

            if (existingAccount == null)
            {
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
            else
                return null;
        }

        public UpdateProfileResponse UpdateProfile(Account account)
        {
            var existingUser = context.Accounts.Where(a => a.Username == account.Username && a.IsBanned == false && a.IsDisabled == false);

            if (existingUser != null)
            {
                Account newInfo = context.Accounts.Single(a => a.Username == account.Username);
                newInfo.FirstName = account.FirstName ?? newInfo.FirstName;
                newInfo.LastName = account.LastName ?? newInfo.LastName;
                newInfo.Address = account.Address ?? newInfo.Address;

                context.SaveChanges();

                UpdateProfileResponse res = (UpdateProfileResponse)newInfo;
                return res;
            }

            return null;
        }

        public void DisableAccount(int id)
        {
            Account TmpAccount = context.Accounts.Single(a => a.Id == id);
            TmpAccount.IsDisabled = true;
            context.SaveChanges();
        }

        public void EnableAccount(int id)
        {
            Account TmpAccount = context.Accounts.Single(a => a.Id == id);
            TmpAccount.IsDisabled = false;
            context.SaveChanges();
        }

        public void BanAccount(int id)
        {
            Account TmpAccount = context.Accounts.Single(a => a.Id == id);
            TmpAccount.IsBanned = true;
            context.SaveChanges();
        }

        public void UnbanAccount(int id)
        {
            Account TmpAccount = context.Accounts.Single(a => a.Id == id);
            TmpAccount.IsBanned = false;
            context.SaveChanges();
        }
    }
}
