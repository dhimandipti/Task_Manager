using taskManager_web_1036.Models.ViewModel;

namespace taskManager_web_1036.Repository.IRepository
{
    public interface IEmployeeRepository
    {
        Task<PagedTaskViewModel> GetMyTasksAsync( int userId, int page, int pageSize,string? search, string? status);
        Task<bool> UpdateTaskStatusAsync(int taskId, int userId, string newStatus);
    }
}
