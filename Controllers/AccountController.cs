using System.Security.Claims;

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

        [HttpGet]
        public ActionResult Version()
        {
            return Ok(new { Version = "1.00" });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login(string Username, string Password)
        {
            var resp = accountService.Login(Username, Password);

            var ret = resp == null ? (ActionResult)Unauthorized("User not found.") : Ok(resp);

            return ret;
        }

        [Authorize]
        [HttpPost("Logout")]
        public ActionResult Logout(Account account)
        {
            accountService.Logout(account);
            return Ok();
        }

        [Authorize]
        [HttpPost()]
        public ActionResult CreateAccount(Account req)
        {
            ActionResult ret;

            var res = accountService.CreateAccount(req);

            if (!res.ok)
            {
                ret = BadRequest($"Username {req.Username} already exists.");
            }
            else
            {
                ret = Ok(new { Id = res.id });
            }

            return ret;
        }

        [Authorize]
        [HttpPost()]
        public ActionResult BanAccount(Account account)
        {
            accountService.BanAccount(account);
            return Ok();
        }

        [Authorize]
        [HttpPost()]
        public ActionResult UnbanAccount(Account account)
        {
            accountService.UnbanAccount(account);
            return Ok();
        }

        [Authorize]
        [HttpPost()]
        public ActionResult DisableAccount(Account account)
        {
            accountService.DisableAccount(account);
            return Ok();
        }

        [Authorize]
        [HttpPost()]
        public ActionResult EnableAccount(Account account)
        {
            accountService.EnableAccount(account);
            return Ok();
        }

        [Authorize]
        [HttpPatch()]
        public ActionResult ChangeInfo(Account account)
        {
            ActionResult ret;

            bool ok = accountService.ChangeInfo(account);
            ret = ok ? Ok() : BadRequest($"Username {account.Username} already exists.");

            return ret;
        }

        //private string GetToken()
        //{
        //    var claims = User.Identity as ClaimsIdentity;
        //    var token = claims.FindFirst("token").Value;

        //    return token;
        //}

        //[Authorize]
        //[HttpPost("expireToken")]
        //public ActionResult ExpireToken()
        //{
        //    var token = GetToken();
        //    accountService.ExpireToken(token);

        //    return Ok();
        //}

        //[Authorize]
        //[HttpPost("expireRefreshToken")]
        //public ActionResult ExpireRefreshToken()
        //{
        //    var token = GetToken();
        //    accountService.ExpireRefreshToken(token);

        //    return Ok();
        //}
    }
}
