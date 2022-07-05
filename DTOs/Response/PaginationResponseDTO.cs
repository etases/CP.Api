namespace CP.Api.DTOs.Response;

public class PaginationResponseDTO<T> : ResponseDTO<IEnumerable<T>>
{
    public int TotalRecords { get; set; }
}