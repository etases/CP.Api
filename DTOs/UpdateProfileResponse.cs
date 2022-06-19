using CP.Api.Models;

namespace CP.Api.DTOs
{
    public class UpdateProfileResponse : Account
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
    }
}
