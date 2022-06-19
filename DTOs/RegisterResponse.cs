using CP.Api.Models;

namespace CP.Api.DTOs
{
    public class RegisterResponse : Account
    {
        public string Username { get; set; }
        public string HashedPassword { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string SaltedPassword { set; get; }
    }
}
