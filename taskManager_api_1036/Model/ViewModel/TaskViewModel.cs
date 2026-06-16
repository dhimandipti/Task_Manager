
using System.ComponentModel.DataAnnotations;
namespace taskManager_api_1036.Model.ViewModel
{
        public class TaskViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Priority { get; set; } = "Medium";
        public string Status { get; set; } = "Pending";
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Valid UserId required")]
        public int CreatedByUserId { get; set; }
        public int Id { get; set; }
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; } = "Not Assigned";
        public string CreatedByName { get; set; } = "Unknown";
        public DateTime CreatedAt { get; set; }
    }   
    public class EmployeeTaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string CreatedByName { get; set; } = "Unknown";
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateStatusViewModel
    {
        [Required]
        [RegularExpression("Pending|InProgress|Completed",
            ErrorMessage = "Status must be Pending, InProgress, or Completed")]
        public string Status { get; set; } = string.Empty;
    }

    public class PagedTaskViewModel
    {
        public List<EmployeeTaskViewModel> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}