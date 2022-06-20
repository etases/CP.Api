using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CP.Api.DTOs.Account;
using CP.Api.DTOs.Response;
using CP.Api.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CP.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }
        
        [HttpPost("Register")]
        public ActionResult<ResponseDTO<AccountOutput>> Register(RegisterInput registerInput)
        {
            var result = _accountService.Register(registerInput);
            if (result.existed)
            {
                return BadRequest(new ResponseDTO<AccountOutput>
                {
                    Success = false,
                    Message = "User already existed",
                });
            }
            
            return Ok(new ResponseDTO<AccountOutput>
            {
                Success = true,
                Message = "Register successfully",
                Data = result.output,
            });
        }
        
        [HttpPost("Login")]
        public ActionResult<ResponseDTO<string>> Login(LoginInput loginInput)
        {
            var result = _accountService.Login(loginInput);

            if (result.output == null)
            {
                return BadRequest(new ResponseDTO<string>
                {
                    Success = false,
                    Message = "Login failed"
                });
            }
            
            var account = result.output;
            
            //create claims details based on the user information
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.Role, account.Role.Name),
                new Claim(ClaimTypes.Name, account.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

            return Ok(new ResponseDTO<string>
            {
                Success = true,
                Message = "Login successfully",
                Data = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<ResponseDTO<AccountOutput>> UpdateProfile(int id, UpdateProfileInput updateProfileInput)
        {
            var res = _accountService.UpdateProfile(id, updateProfileInput);
            if (res == null)
            {
                return BadRequest(new ResponseDTO<AccountOutput>
                {
                    Success = false,
                    Message = "Update profile failed"
                });
            }
            
            return Ok(new ResponseDTO<AccountOutput>
            {
                Success = true,
                Message = "Update profile successfully",
                Data = res
            });
        }

        [Authorize]
        [HttpPut("Ban/{id}")]
        public ActionResult<ResponseDTO> BanAccount(int id, bool ban)
        {
            if (_accountService.SetBanStatus(id, ban))
            {
                return Ok(new ResponseDTO
                {
                    Success = true,
                    Message = "Set Ban account successfully"
                });
            }
            
            return BadRequest(new ResponseDTO
            {
                Success = false,
                Message = "Set Ban account failed"
            });
        }

        [Authorize]
        [HttpPut("Disable/{id}")]
        public ActionResult DisableAccount(int id, bool disable)
        {
            if (_accountService.SetDisableStatus(id, disable))
            {
                return Ok(new ResponseDTO
                {
                    Success = true,
                    Message = "Set Disable account successfully"
                });
            }
            
            return BadRequest(new ResponseDTO
            {
                Success = false,
                Message = "Set Disable account failed"
            });
        }
    }
}
