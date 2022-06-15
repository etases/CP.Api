using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP.Api.Models;

public class Account
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public string Username { get; set; } = null!;

    [Required] public int RoleId { get; set; }

    [Required] public string HashedPassword { get; set; } = null!;

    [Required] public string SaltedPassword { get; set; } = null!;

    public bool IsDisabled { get; set; } = false;
    public bool IsBanned { get; set; } = false;

    public DateTime CreatedDay { get; set; } = DateTime.Now;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;

    public string AccessToken { get; set; }
    //public int? ExpiresIn { get; set; }
    //public long? ExpiresOn { get; set; }

    [ForeignKey(nameof(RoleId))]
    [InverseProperty("Accounts")]
    public virtual Role Role { get; set; } = null!;
    public string RefreshToken { get; set; }

    [InverseProperty(nameof(Comment.Account))]
    public virtual ICollection<Comment> Comments { get; set; } = null!;

    public void Login(long ts)
    {
        AccessToken = Guid.NewGuid().ToString();
        RefreshToken = Guid.NewGuid().ToString();
        //ExpiresIn = Constants.ONE_DAY_IN_SECONDS;
        //ExpiresOn = ts + ExpiresIn;
    }

    public void Logout()
    {
        AccessToken = null;
        RefreshToken = null;
        //ExpiresIn = null;
        //ExpiresOn = null;
    }
}