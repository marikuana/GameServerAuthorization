using GameServerAuthorization.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace GameServerAuthorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private IAuth _auth;
        private AuthOptions _authOptions;

        public AccountController(IAuth auth, AuthOptions authOptions)
        {
            _auth = auth;
            _authOptions = authOptions;
        }

        [HttpPost("[action]")]
        public IActionResult Login(LoginModel loginModel)
        {
            IAccount? account = _auth.GetAccount(loginModel.Login, loginModel.Password);
            if (account == null)
                return Forbid();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
            };

            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                claims: claims,
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
            return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
    }
}
