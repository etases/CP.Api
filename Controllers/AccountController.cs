using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CP.Api.Core.Models;
using CP.Api.DTOs.Account;
using CP.Api.DTOs.Response;
using CP.Api.Models;
using CP.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CP.Api.Controllers
{
    /// <summary>
    /// Account API controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Account controller constructor
        /// </summary>
        /// <param name="accountService">Account service</param>
        /// <param name="configuration">Application configurations</param>
        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        /// <summary>
        /// Get account information by account id
        /// </summary>
        /// <param name="id">Id of the requested account</param>
        /// <returns>ResponseDTO <seealso cref="AccountOutput"/></returns>
        [HttpGet("{id}")]
        public ActionResult<ResponseDTO<AccountOutput>> Get(int id)
        {
            AccountOutput? account = _accountService.GetAccount(id);
            return account switch
            {
                null => NotFound(new ResponseDTO<AccountOutput> { Message = "Account not found", Success = false }),
                _ => Ok(new ResponseDTO<AccountOutput> { Data = account, Success = true, Message = "Account found" })
            };
        }

        /// <summary>
        /// Get account information from token
        /// </summary>
        /// <returns>ResponseDTO <seealso cref="AccountOutput"/></returns>
        [HttpGet("FromToken")]
        [Authorize]
        public ActionResult<ResponseDTO<AccountOutput>> GetFromToken()
        {
            int userId = int.Parse(User.FindFirst("Id")!.Value);
            return Get(userId);
        }

        /// <summary>
        /// Register new account
        /// </summary>
        /// <param name="registerInput">Information to register new account</param>
        /// <returns>ResponseDTO <seealso cref="AccountOutput"/></returns>
        [HttpPost("Register")]
        public ActionResult<ResponseDTO<AccountOutput>> Register(RegisterInput registerInput)
        {
            (bool existed, AccountOutput? output) = _accountService.Register(registerInput);
            return existed switch
            {
                true => Conflict(new ResponseDTO<AccountOutput> { Message = "Account already existed", Success = false }),
                _ => Ok(new ResponseDTO<AccountOutput> { Data = output, Success = true, Message = "Account registered" }),
            };
        }

        /// <summary>
        /// Login account with provided credentials
        /// </summary>
        /// <param name="loginInput">Information to login</param>
        /// <returns>ResponseDTO <seealso cref="string"/></returns>
        [HttpPost("Login")]
        public ActionResult<ResponseDTO<string>> Login(LoginInput loginInput)
        {
            (bool _, AccountOutput? output) = _accountService.Login(loginInput);

            if (output == null)
            {
                return BadRequest(new ResponseDTO<string> { Success = false, Message = "Login failed" });
            }

            JWTModel? jwtConf = _configuration.GetSection("Jwt").Get<JWTModel>();

            //create claims details based on the user information
            Claim[] claims =
            {
            new(JwtRegisteredClaimNames.Sub, jwtConf.Subject),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new(ClaimTypes.Role, output.Role.Name), new(ClaimTypes.Name, output.Username),
            new("Id", output.Id.ToString())
        };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtConf.Key));

            SigningCredentials signIn = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(issuer: jwtConf.Issuer, audience: jwtConf.Audience, claims:
                claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

            return Ok(new ResponseDTO<string>
            {
                Success = true,
                Message = "Login successfully",
                Data = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        /// <summary>
        /// Update account information
        /// </summary>
        /// <param name="updateProfileInput">Updated informations for the account</param>
        /// <returns>ResponseDTO <seealso cref="AccountOutput"/></returns>
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<ResponseDTO<AccountOutput>> UpdateProfile(UpdateProfileInput updateProfileInput)
        {
            int userId = int.Parse(User.FindFirst("Id")!.Value);
            AccountOutput? result = _accountService.UpdateProfile(userId, updateProfileInput);
            return result switch
            {
                null => NotFound(new ResponseDTO<AccountOutput> { Message = "Account not found", Success = false }),
                _ => Ok(new ResponseDTO<AccountOutput> { Data = result, Success = true, Message = "Account updated" })
            };
        }

        /// <summary>
        /// Change account banning status
        /// </summary>
        /// <param name="id">Id of the account</param>
        /// <param name="ban">Ban status</param>
        /// <returns>ResponseDTO</returns>
        [Authorize(Roles = DefaultRoles.AdministratorString)]
        [HttpPut("Ban/{id}")]
        public ActionResult<ResponseDTO> BanAccount(int id, bool ban)
        {
            bool success = _accountService.SetBanStatus(id, ban);
            return success switch
            {
                true => Ok(new ResponseDTO { Success = true, Message = "Ban status updated" }),
                _ => NotFound(new ResponseDTO { Success = false, Message = "Account not found" }),
            };
        }

        /// <summary>
        /// Change account visibility status
        /// </summary>
        /// <param name="id">Id of the account</param>
        /// <param name="disable">Visibility status</param>
        /// <returns>ResponseDTO</returns>
        [Authorize(Roles = DefaultRoles.AdministratorString)]
        [HttpPut("Disable/{id}")]
        public ActionResult<ResponseDTO> DisableAccount(int id, bool disable)
        {
            bool success = _accountService.SetDisableStatus(id, disable);
            return success switch
            {
                true => Ok(new ResponseDTO { Success = true, Message = "Visibility status updated" }),
                _ => NotFound(new ResponseDTO { Success = false, Message = "Account not found" }),
            };
        }
    }
}
