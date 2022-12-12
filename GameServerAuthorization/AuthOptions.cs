using Microsoft.IdentityModel.Tokens;

namespace GameServerAuthorization
{
    public class AuthOptions
    {
        private IConfiguration _configuration;

        public AuthOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Issuer => _configuration["AuthOptions:Issuer"] ?? throw new Exception("Add AuthOptions:Issuer to config");
        public string Audience => _configuration["AuthOptions:Audience"] ?? throw new Exception("Add AuthOptions:Audience to config");
        public string Key => _configuration["AuthOptions:Key"] ?? throw new Exception("Add AuthOptions:Key to config");
        public SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Key));
    }
}
