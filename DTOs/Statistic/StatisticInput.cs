namespace CP.Api.DTOs.Statistic;

public class StatisticInput
{
    public DateTime? Date { get; set; } = DateTime.Today.Date;
    public int? Day => Date?.Day;
    public int? Month => Date?.Month;
    public int? Year => Date?.Year;
}

public enum StatisticType
{
    Register,
    Account,
    Category
}