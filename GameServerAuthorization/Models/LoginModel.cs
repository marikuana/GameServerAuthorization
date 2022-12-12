using System.ComponentModel.DataAnnotations;

namespace GameServerAuthorization.Models
{
    public class LoginModel
    {
        [Required, MinLength(3), MaxLength(32)]
        public string Login { get; set; }
        [Required, MinLength(3), MaxLength(32)]
        public string Password { get; set; }
    }
}
