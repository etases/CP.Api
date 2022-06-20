using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CP.Api.DTOs.Account;
using CP.Api.DTOs.Response;
using CP.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CP.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;

    public AccountController(IAccountService accountService, IConfiguration configuration)
    {
        _accountService = accountService;
        _configuration = configuration;
    }

    [HttpGet("{id}")]
    public ActionResult<ResponseDTO<AccountOutput>> Get(int id)
    {
        AccountOutput? account = _accountService.GetAccount(id);
        if (account == null)
        {
            return NotFound(new ResponseDTO<AccountOutput> {Message = "Account not found", Success = false});
        }

        return Ok(new ResponseDTO<AccountOutput> {Data = account, Success = true, Message = "Account found"});
    }

    [HttpPost("Register")]
    public ActionResult<ResponseDTO<AccountOutput>> Register(RegisterInput registerInput)
    {
        (bool existed, AccountOutput? output) result = _accountService.Register(registerInput);
        if (result.existed)
        {
            return BadRequest(new ResponseDTO<AccountOutput> {Success = false, Message = "User already existed"});
        }

        return Ok(new ResponseDTO<AccountOutput>
        {
            Success = true, Message = "Register successfully", Data = result.output
        });
    }


    [HttpPost("Login")]
    public ActionResult<ResponseDTO<string>> Login(LoginInput loginInput)
    {
        (bool found, AccountOutput? output) result = _accountService.Login(loginInput);

        if (result.output == null)
        {
            return BadRequest(new ResponseDTO<string> {Success = false, Message = "Login failed"});
        }

        AccountOutput? account = result.output;

        //create claims details based on the user information
        Claim[] claims =
        {
            new(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new(ClaimTypes.Role, account.Role.Name), new(ClaimTypes.Name, account.Username)
        };

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        SigningCredentials signIn = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
            claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

        return Ok(new ResponseDTO<string>
        {
            Success = true, Message = "Login successfully", Data = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }

    [Authorize]
    [HttpPut("{id}")]
    public ActionResult<ResponseDTO<AccountOutput>> UpdateProfile(int id, UpdateProfileInput updateProfileInput)
    {
        AccountOutput? res = _accountService.UpdateProfile(id, updateProfileInput);
        if (res == null)
        {
            return BadRequest(new ResponseDTO<AccountOutput> {Success = false, Message = "Update profile failed"});
        }

        return Ok(new ResponseDTO<AccountOutput> {Success = true, Message = "Update profile successfully", Data = res});
    }

    [Authorize]
    [HttpPut("Ban/{id}")]
    public ActionResult<ResponseDTO> BanAccount(int id, bool ban)
    {
        if (_accountService.SetBanStatus(id, ban))
        {
            return Ok(new ResponseDTO {Success = true, Message = "Set Ban account successfully"});
        }

        return BadRequest(new ResponseDTO {Success = false, Message = "Set Ban account failed"});
    }

    [Authorize]
    [HttpPut("Disable/{id}")]
    public ActionResult DisableAccount(int id, bool disable)
    {
        if (_accountService.SetDisableStatus(id, disable))
        {
            return Ok(new ResponseDTO {Success = true, Message = "Set Disable account successfully"});
        }

        return BadRequest(new ResponseDTO {Success = false, Message = "Set Disable account failed"});
    }
}