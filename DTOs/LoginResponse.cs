using CP.Api.Models;

namespace CP.Api.DTOs
{
    public class LoginResponse : Account
    {
        public string Username { get; set; }
        public int RoleId { set; get; }
    }
}
