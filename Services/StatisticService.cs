using CP.Api.Context;
using CP.Api.DTOs.Statistic;

namespace CP.Api.Services;

public interface IStatisticService
{
    StatisticOutput? GetRegisterStatistic(StatisticInput input);
    StatisticOutput? GetAccountStatistic(int accountId, StatisticInput input);
    StatisticOutput? GetCategoryStatistic(int categoryId, StatisticInput input);
}

public class StatisticService : IStatisticService
{
    private readonly ApplicationDbContext _context;

    public StatisticService(ApplicationDbContext context)
    {
        _context = context;
    }

    public StatisticOutput? GetAccountStatistic(int accountId, StatisticInput input)
    {
        StatisticOutput result = new();

        try
        {
            result = new StatisticOutput
            {
                Date = input.Date,
                Day = _context.Comments.Where(c =>
                    c.AccountId == accountId &&
                    c.CreatedDate.Day == input.Day &&
                    c.CreatedDate.Month == input.Month &&
                    c.CreatedDate.Year == input.Year).Count(),
                Month = _context.Comments.Where(c =>
                    c.AccountId == accountId &&
                    c.CreatedDate.Month == input.Month &&
                    c.CreatedDate.Year == input.Year).Count(),
                Year = _context.Comments.Where(c =>
                    c.AccountId == accountId &&
                    c.CreatedDate.Year == input.Year).Count(),
                Total = _context.Comments.Where(c =>
                    c.AccountId == accountId).Count()
            };
        }
        catch { return null; }

        return result;
    }

    public StatisticOutput? GetCategoryStatistic(int categoryId, StatisticInput input)
    {
        StatisticOutput result = new();

        try
        {
            result = new StatisticOutput
            {
                Date = input.Date,
                Day = _context.Comments.Where(c =>
                    c.CategoryId == categoryId &&
                    c.CreatedDate.Day == input.Day &&
                    c.CreatedDate.Month == input.Month &&
                    c.CreatedDate.Year == input.Year).Count(),
                Month = _context.Comments.Where(c =>
                    c.CategoryId == categoryId &&
                    c.CreatedDate.Month == input.Month &&
                    c.CreatedDate.Year == input.Year).Count(),
                Year = _context.Comments.Where(c =>
                    c.CategoryId == categoryId &&
                    c.CreatedDate.Year == input.Year).Count(),
                Total = _context.Comments.Where(c =>
                    c.CategoryId == categoryId).Count()
            };
        }
        catch
        {
            return null;
        }

        return result;
    }

    public StatisticOutput? GetRegisterStatistic(StatisticInput input)
    {
        StatisticOutput result = new();

        try
        {
            result = new StatisticOutput
            {
                Date = input.Date,
                Day = _context.Accounts.Where(a =>
                    a.CreatedDay.Day == input.Day &&
                    a.CreatedDay.Month == input.Month &&
                    a.CreatedDay.Year == input.Year).Count(),
                Month = _context.Accounts.Where(a =>
                    a.CreatedDay.Month == input.Month &&
                    a.CreatedDay.Year == input.Year).Count(),
                Year = _context.Accounts.Where(a =>
                    a.CreatedDay.Year == input.Year).Count(),
                Total = _context.Accounts.Count()
            };
        }
        catch
        {
            return null;
        }

        return result;
    }
}