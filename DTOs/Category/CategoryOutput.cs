namespace CP.Api.DTOs;

public class CategoryOutput
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Note { get; set; }
}