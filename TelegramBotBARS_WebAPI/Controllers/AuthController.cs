using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using TelegramBotBARS_WebAPI.Options;

namespace TelegramBotBARS_WebAPI.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly TokenOptions _options;
        private readonly User _user;

        public AuthController(IOptions<TokenOptions> options, IConfiguration configuration)
        {
            _options = options.Value;
            _user = configuration.GetSection("User").Get<User>();
        }

        [HttpPost, Route("login")]
        public IActionResult Login(User user)
        {
            if (_user.Login == user.Login && _user.Password == user.Password)
            {
                var secretKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_options.SigningKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
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
