
namespace taskManager_web_1036.Models.ViewModel
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Low";
        public string Status { get; set; } = "Pending";
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; } = "Not Assigned";
        public string CreatedByName { get; set; } = "Unknown";
        public int CreatedByUserId { get; set; }
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


    public class PagedTaskViewModel
    {
        public List<EmployeeTaskViewModel> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

 
    public class UpdateStatusViewModel
    {
        public string Status { get; set; } = string.Empty;
    }
}