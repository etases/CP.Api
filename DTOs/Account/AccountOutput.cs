using CP.Api.DTOs.Role;

namespace CP.Api.DTOs.Account;

public class AccountOutput
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public int RoleId { get; set; }
    public bool IsDisabled { get; set; } = false;
    public bool IsBanned { get; set; } = false;
    public DateTime CreatedDay { get; set; } = DateTime.Now;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public virtual RoleDTO Role { get; set; } = null!;
}