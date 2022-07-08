namespace CP.Api.DTOs.Statistic;

public class StatisticOutput
{
    public DateTime? Date { get; set; } = DateTime.Today.Date;
    public int? Day { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public int? Total { get; set; }
}