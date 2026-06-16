namespace taskManager_web_1036.Models.ViewModel
{
    public class LoginResponseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;   // ✅ add this
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty; 

    }
}
