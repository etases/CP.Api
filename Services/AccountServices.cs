using CP.Api.Context;
using CP.Api.Models;

namespace CP.Api.Services
{
    public class AccountServices
    {
        private readonly ApplicationDbContext context;

        public AccountServices(ApplicationDbContext context)
        {
            this.context = context;
        }

        public (bool ok, int id)CreateAccount(Account account)
        {
            bool ok = false;
            int id = -1;

            var existingAccount = context.Accounts.Where(a => a.Username == account.Username && !a.IsBanned);

            if (existingAccount.Count() == 0)
            {
                var salt = Hasher.GenerateSalt();
                var hashedPassword = Hasher.HashPassword(salt, account.HashedPassword);
                var newAccount = new Account() { Username = account.Username, HashedPassword = hashedPassword, SaltedPassword = salt };
                context.Accounts.Add(newAccount);
                context.SaveChanges();
                ok = true;
                id = newAccount.Id;
            }

            return (ok, id);
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

        // Chua biet cach lam JWT ;-;
        public LoginResponse Login(string Username, string Password)
        {
            var existingUsers = context.Accounts.Where(a => a.Username == Username && a.IsBanned == false).ToList();

            var existingUser = context.Accounts.Where(a => a.Username == Username && a.IsBanned == false)
                .ToList().SingleOrDefault(a => Hasher.HashPassword(a.SaltedPassword, Password) == a.HashedPassword);

            if (existingUser != null)
                return new LoginResponse 
                    {
                        Status = true, 
                        Accounts = existingUser
                        // JWT
                    };
            else
                return new LoginResponse
                    {
                        Status = false,
                        Accounts = null
                        // JWT
                    };
        }
    }
}
