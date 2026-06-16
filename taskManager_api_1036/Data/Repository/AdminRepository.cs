using Microsoft.EntityFrameworkCore;
using taskManager_api_1036.Data;
using taskManager_api_1036.Data.Repository.IRepository;
using taskManager_api_1036.Model;
using taskManager_api_1036.Model.ViewModel;

namespace taskManager_api_1036.Data.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

   
        // CREATE USER     
        public async Task<User> CreateUserAsync(CreateUserViewModel model)
        {
            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = model.Password, 
                Role = model.Role,
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }      
        // GET ALL USERS
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }       
        // CREATE TASK
      
        public async Task<TaskManagement> CreateTaskAsync(TaskViewModel model)
        {
            var task = new TaskManagement
            {
                Title = model.Title,
                Description = model.Description,
                Priority = model.Priority,
                Status = model.Status,
                CreatedByUserId = model.CreatedByUserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<TaskManagement?> AssignTaskAsync(int taskId, int userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null) return null;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;
            task.AssignedUserId = userId;
            await _context.SaveChangesAsync();          
            return await _context.Tasks
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }
       
        public async Task<List<TaskViewModel>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.AssignedUser)
                .Include(t => t.CreatedBy)
                .Select(t => new TaskViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status,
                    CreatedByUserId = t.CreatedByUserId,

                    AssignedUserId = t.AssignedUserId,
                    AssignedUserName = t.AssignedUser != null
                        ? t.AssignedUser.FullName
                        : "Not Assigned",

                    CreatedByName = t.CreatedBy != null
                        ? t.CreatedBy.FullName
                        : "Unknown",

                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();
        }
      
        // DELETE TASK
    
        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
            if (task == null) return false;
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
        //update
        public async Task<TaskManagement> UpdateTaskAsync(int id, TaskViewModel model)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)return null;
            task.Title = model.Title;
            task.Description = model.Description;
            task.Priority = model.Priority;
            task.Status = model.Status;
            await _context.SaveChangesAsync();
            return task;
        }
    }
}