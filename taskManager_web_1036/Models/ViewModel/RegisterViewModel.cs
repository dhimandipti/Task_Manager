using System.ComponentModel.DataAnnotations;

namespace taskManager_web_1036.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; }= string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; }= "Employee";
    }
}
