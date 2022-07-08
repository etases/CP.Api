namespace CP.Api.Core.Models;

public class JWTModel
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Subject { get; set; } = null!;
}