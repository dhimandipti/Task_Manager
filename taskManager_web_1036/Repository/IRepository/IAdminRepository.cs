using taskManager_web_1036.Models.ViewModel;

namespace taskManager_web_1036.Repository.IRepository
{
    public interface IAdminRepository
    {
        Task<List<CreateUserViewModel>> GetUsers();
        Task<CreateUserViewModel> CreateUser(CreateUserViewModel model);
        Task<List<TaskViewModel>> GetTasks();
        Task<TaskViewModel> CreateTask(TaskViewModel model);
        Task<bool> DeleteTask(int id);
        Task<TaskViewModel> AssignTask(int taskId, int userId);
        Task<TaskViewModel?> UpdateTask(int id, TaskViewModel model);
    }
}
