using taskManager_api_1036.Model.ViewModel;

namespace taskManager_api_1036.Data.Repository.IRepository
{
    public interface IEmployeeRepository
    {
        Task<PagedTaskViewModel> GetMyTasksAsync(int userId, int page, int pageSize,
            string? search, string? status);
        Task<bool> UpdateTaskStatusAsync(int taskId, int userId, string newStatus);
    }
}
