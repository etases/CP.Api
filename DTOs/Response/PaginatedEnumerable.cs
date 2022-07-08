namespace CP.Api.DTOs.Response;

public class PaginatedEnumerable<T>
{
    public int TotalRecord { get; set; }
    public int TotalPage { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
    public IEnumerable<T> Items { get; init; } = null!;
}