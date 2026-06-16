using Microsoft.EntityFrameworkCore;
using taskManager_api_1036.Data.Repository.IRepository;
using taskManager_api_1036.Model.ViewModel;

namespace taskManager_api_1036.Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PagedTaskViewModel> GetMyTasksAsync(
                    int userId, int page, int pageSize,
                    string? search, string? status)
        {
            var query = _context.Tasks
                .Include(t => t.CreatedBy)
                .Where(t => t.AssignedUserId == userId)
                .AsQueryable();

           
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(t =>
                    t.Title.Contains(search) ||
                    t.Description.Contains(search));

        
            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(t => t.Status == status);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new EmployeeTaskViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status,
                    CreatedByName = t.CreatedBy != null
                        ? t.CreatedBy.FullName
                        : "Unknown",
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return new PagedTaskViewModel
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

    
        // UPDATE TASK STATUS       
        public async Task<bool> UpdateTaskStatusAsync(int taskId, int userId, string newStatus)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.AssignedUserId == userId);
            if (task == null) return false;
            task.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
    

