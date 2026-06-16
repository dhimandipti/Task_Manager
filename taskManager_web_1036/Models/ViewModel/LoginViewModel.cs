using System.ComponentModel.DataAnnotations;

namespace taskManager_web_1036.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
            = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
            = string.Empty;

        public class RefreshTokenResponseViewModel
        {
            public string Token { get; set; } = string.Empty;
            public string RefreshToken { get; set; } = string.Empty;
            public DateTime Expiry { get; set; }
        }
    }
}
