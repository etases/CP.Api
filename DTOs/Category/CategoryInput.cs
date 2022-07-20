using System.ComponentModel.DataAnnotations;

namespace CP.Api.DTOs;

public class CategoryInput
{
    public string Name { get; set; } = null!;
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string? Note { get; set; }
}