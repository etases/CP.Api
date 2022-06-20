namespace CP.Api.DTOs.Response;

public class ResponseDTO<T>
{
    public int ErrorCode { get; set; }

    public bool Success
    {
        get => ErrorCode == 0;
        set => ErrorCode = value ? 0 : 1;
    }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}

public class ResponseDTO : ResponseDTO<object>
{
}