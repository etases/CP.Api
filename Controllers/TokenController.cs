using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CP.Api.Context;
using CP.Api.Models;
using CP.Api.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public TokenController(IConfiguration config, ApplicationDbContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Account account)
        {
            var salt = Hasher.GenerateSalt();
            var hashedPassword = Hasher.HashPassword(salt, account.HashedPassword);
            if (account != null && account.Username != null && hashedPassword != null)
            {
                var user = _context.Accounts.Where(a => a.Username == account.Username && a.HashedPassword == hashedPassword).FirstOrDefault();

                if (true)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Username", account.Username),
                    new Claim("HashedPassword", hashedPassword)
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }

            return BadRequest();
        }

    }
}
