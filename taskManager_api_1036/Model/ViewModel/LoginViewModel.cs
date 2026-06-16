namespace taskManager_api_1036.Model.ViewModel
{
    public class LoginViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class TokenRequest
    {
        public string RefreshToken { get; set; }
    }
}