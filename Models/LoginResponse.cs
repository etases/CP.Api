namespace CP.Api.Models
{
    public class LoginResponse
    {
        public bool Status { set; get; }
        public virtual Account Accounts { get; set; }
        public object JWT { get; set; }
        // JWT ;-;
    }
}
