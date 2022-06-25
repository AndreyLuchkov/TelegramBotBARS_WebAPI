using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TelegramBotBARS_WebAPI.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly User _user;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _user = new User
            {
                Login = configuration["User:Login"],
                Password = configuration["User:Password"]
            };
            _configuration = configuration;
        }

        [HttpPost, Route("login")]
        public IActionResult Login(User user)
        {
            if (_user.Login == user.Login && _user.Password == user.Password)
            {
                var secretKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["TokenConfig:SigningKey"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["TokenConfig:Issuer"],
                    audience: _configuration["TokenConfig:Audience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
