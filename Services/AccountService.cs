using CP.Api.Context;

using AutoMapper;

using CP.Api.DTOs.Account;
using CP.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace CP.Api.Services;

public class AccountService : IAccountService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AccountService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    private IQueryable<Account> GetAccounts(bool checkBanned, bool checkDisabled)
    {
        var accounts = _context.Accounts.Include(a => a.Role).AsQueryable();
        if (checkBanned)
        {
            accounts = accounts.Where(a => a.IsBanned == false);
        }
            
        if (checkDisabled)
        {
            accounts = accounts.Where(a => a.IsDisabled == false);
        }
            
        return accounts;
    }

    public (bool found, AccountOutput? output) Login(LoginInput input)
    {
        var account = GetAccounts(true, true).SingleOrDefault(a => a.Username == input.Username);
        if (account == null)
        {
            return (false, null);
        }
            
        var salt = account.SaltedPassword;
        var hashedPassword = Hasher.HashPassword(salt, input.Password);
        if (hashedPassword != account.HashedPassword)
        {
            return (true, null);
        }
            
        var output = _mapper.Map<AccountOutput>(account);
        return (true, output);
    }

    public (bool existed, AccountOutput? output) Register(RegisterInput input)
    {
        if (GetAccounts(false, false).Any(a => a.Username == input.Username))
        {
            return (true, null);
        }

        var account = _mapper.Map<Account>(input);
        account.SaltedPassword = Hasher.GenerateSalt();
        account.HashedPassword = Hasher.HashPassword(account.SaltedPassword, input.Password);
        _context.Accounts.Add(account);
        _context.SaveChanges();
            
        var output = _mapper.Map<AccountOutput>(account);
        return (false, output);
    }

    public AccountOutput? UpdateProfile(int id, UpdateProfileInput input)
    {
        var account = GetAccounts(true, true).SingleOrDefault(a => a.Id == id);
        if (account == null)
        {
            return null;
        }
            
        _mapper.Map(input, account);
        _context.SaveChanges();
            
        var output = _mapper.Map<AccountOutput>(account);
        return output;
    }

    public bool SetDisableStatus(int id, bool disable)
    {
        var account = GetAccounts(false, false).SingleOrDefault(a => a.Id == id);
        if (account == null)
        {
            return false;
        }
        account.IsDisabled = disable;
        _context.SaveChanges();
        return true;
    }

    public bool SetBanStatus(int id, bool ban)
    {
        var account = GetAccounts(false, false).SingleOrDefault(a => a.Id == id);
        if (account == null)
        {
            return false;
        }
        account.IsBanned = ban;
        _context.SaveChanges();
        return true;
    }
    
    public AccountOutput? GetAccount(int id)
    {
        var account = GetAccounts(false, false).SingleOrDefault(a => a.Id == id);
        if (account == null)
        {
            return null;
        }
        var output = _mapper.Map<AccountOutput>(account);
        return output;
    }
}

public interface IAccountService
{
    (bool found, AccountOutput? output) Login(LoginInput input);
    (bool existed, AccountOutput? output) Register(RegisterInput input);
    AccountOutput? UpdateProfile(int id, UpdateProfileInput input);
    bool SetDisableStatus(int id, bool disable);
    bool SetBanStatus(int id, bool ban);
    AccountOutput? GetAccount(int id);
}