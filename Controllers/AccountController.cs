using CP.Api.Interfaces;
using CP.Api.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CP.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public ActionResult CreateAccount(Account account)
        {
            var result = accountService.CreateAccount(account);

            ActionResult ret = result == null ? BadRequest($"Username {account.Username} already exists.") : Ok();

            return ret;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login(string Username, string Password)
        {
            var result = accountService.Login(Username, Password);

            ActionResult ret = result == null ? (ActionResult)Unauthorized("User not found.") : Ok(result);

            return ret;
        }

        [Authorize]
        [HttpPatch()]
        public ActionResult UpdateProfile(Account account)
        {
            var res = accountService.UpdateProfile(account);

            ActionResult ret = res == null ? BadRequest($"Something wrong while updating.") : Ok();

            return ret;
        }

        [Authorize]
        [HttpPost()]
        public ActionResult BanAccount(int id)
        {
            accountService.BanAccount(id);
            return Ok();
        }

        [Authorize]
        [HttpPost()]
        public ActionResult UnbanAccount(int id)
        {
            accountService.UnbanAccount(id);
            return Ok();
        }

        [Authorize]
        [HttpPost()]
        public ActionResult DisableAccount(int id)
        {
            accountService.DisableAccount(id);
            return Ok();
        }

        [Authorize]
        [HttpPost()]
        public ActionResult EnableAccount(int id)
        {
            accountService.EnableAccount(id);
            return Ok();
        }
    }
}
