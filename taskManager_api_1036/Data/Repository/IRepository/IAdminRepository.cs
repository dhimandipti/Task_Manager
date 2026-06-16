using taskManager_api_1036.Model;
using taskManager_api_1036.Model.ViewModel;

namespace taskManager_api_1036.Data.Repository.IRepository
{
    public interface IAdminRepository
    {
        Task<User> CreateUserAsync(CreateUserViewModel model);
        Task<List<User>> GetAllUsersAsync();
        Task<TaskManagement> CreateTaskAsync(TaskViewModel model);
        Task<TaskManagement> AssignTaskAsync(int taskId, int userId);
        Task<List<TaskViewModel>> GetAllTasksAsync();
        Task<bool> DeleteTaskAsync(int taskId);
        Task<TaskManagement> UpdateTaskAsync(int id, TaskViewModel model);
    }
}
