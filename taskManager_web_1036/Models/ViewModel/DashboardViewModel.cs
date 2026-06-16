namespace taskManager_web_1036.Models.ViewModel
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalTasks { get; set; }
        public int TodoTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int UnassignedTasks { get; set; }

        // ── Priority breakdown ───────────────────────────────────
        public int HighPriorityTasks { get; set; }
        public int MediumPriorityTasks { get; set; }
        public int LowPriorityTasks { get; set; }

        // ── Tables ───────────────────────────────────────────────
        public List<TaskViewModel> RecentTasks { get; set; } = new();
        public List<UserSummaryViewModel> RecentUsers { get; set; } = new();
    }

    // Lightweight user summary used only on the dashboard
    public class UserSummaryViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

      
        public string Initials =>
            string.Concat(FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                  .Take(2)
                                  .Select(w => char.ToUpper(w[0])));
    }
}
