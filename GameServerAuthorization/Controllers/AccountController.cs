using GameServerAuthorization.Models;using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using GameServerData.Models;
using System.Security.Cryptography;
using GameServerAuthorization.Interfaces;

namespace GameServerAuthorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private IAccountManager _accountManager;
        private AuthOptions _authOptions;

        public AccountController(AuthOptions authOptions, IAccountManager accountManager)
        {
            _authOptions = authOptions;
            _accountManager = accountManager;
        }

        [HttpPost("[action]")]
        public IActionResult Login(LoginModel loginModel)
        {
            Account? account = _accountManager.GetAccount(loginModel.Login, loginModel.Password);
            if (account == null)
                return Unauthorized();

            var claim = new Claim(ClaimTypes.NameIdentifier, account.Id.ToString());

            var token = CreateToken(claim);
            var refreshToken = GenereteRefreshToken();
            _accountManager.UpdateRefreshToken(account.Id, refreshToken, TimeSpan.FromDays(_authOptions.RefreshTokenValidityOnDays));

            return Ok(new TokenModel()
            {
                Accesstoken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            });
        }

        [HttpPost("[action]")]
        public IActionResult RefreshTocken(TokenModel tokenModel)
        {
            var principal = GetClaimsPrincipal(tokenModel.Accesstoken);
            if (principal == null)
                return BadRequest("Invalid access token or refresh token");

            var accountId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value);

            var account = _accountManager.GetAccount(accountId);

            if (account == null || account.RefreshToken != tokenModel.RefreshToken || account.RefreshTokenExpiryTime < DateTime.UtcNow)
                return BadRequest("Invalid access token or refresh token");

            var token = CreateToken(principal.Claims.ToArray());
            var refreshToken = GenereteRefreshToken();
            _accountManager.UpdateRefreshToken(accountId, refreshToken, TimeSpan.FromDays(_authOptions.RefreshTokenValidityOnDays));

            return Ok(new TokenModel()
            {
                Accesstoken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            });
        }

        private JwtSecurityToken CreateToken(params Claim[] claims)
        {
            var token = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                expires: DateTime.UtcNow.AddMinutes(_authOptions.TokenValidityInMinutes),
                claims: claims,
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return token;
        }

        private string GenereteRefreshToken()
        {
            var randomNumber = new byte[64];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetClaimsPrincipal(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = _authOptions.GetSymmetricSecurityKey(),
                ValidateLifetime = false,
            };
            
            var tockenHandler = new JwtSecurityTokenHandler();
            var principial = tockenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principial;
        }
    }
}
