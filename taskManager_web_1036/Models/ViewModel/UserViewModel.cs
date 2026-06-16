using System.ComponentModel.DataAnnotations;

namespace taskManager_web_1036.Models.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "Employee";
        public DateTime CreatedAt { get; set; }
    }
}
