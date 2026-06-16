using Microsoft.AspNetCore.Mvc;
using taskManager_web_1036.Repository.IRepository;
using taskManager_web_1036.Models.ViewModel;

namespace taskManager_web_1036.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepo;

        public AdminController(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }       
         public async Task<IActionResult> Index()
        {
            var tasks = await _adminRepo.GetTasks();   
            var users = await _adminRepo.GetUsers();   
            var vm = new DashboardViewModel
            {
           
                TotalUsers = users.Count,
                TotalAdmins = users.Count(u => u.Role == "Admin"),
                TotalEmployees = users.Count(u => u.Role == "Employee"),

                TotalTasks = tasks.Count,
                TodoTasks = tasks.Count(t => t.Status == "Todo"),
                InProgressTasks = tasks.Count(t => t.Status == "InProgress"),
                CompletedTasks = tasks.Count(t => t.Status == "Completed"),
                UnassignedTasks = tasks.Count(t => t.AssignedUserId == null || t.AssignedUserId == 0),

               
                HighPriorityTasks = tasks.Count(t => t.Priority == "High"),
                MediumPriorityTasks = tasks.Count(t => t.Priority == "Medium"),
                LowPriorityTasks = tasks.Count(t => t.Priority == "Low"),

               
                RecentTasks = tasks
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(5)
                    .ToList(),

              
                RecentUsers = users
                    .Take(5)
                    .Select(u => new UserSummaryViewModel
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        Email = u.Email,
                        Role = u.Role
                    })
                    .ToList()
            };
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await _adminRepo.GetUsers();
            return View(users);
        }

        [HttpGet]
        public IActionResult CreateUser() => View();

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _adminRepo.CreateUser(model);
            return RedirectToAction("Users");
        }   
        public async Task<IActionResult> Tasks()
        {
            var tasks = await _adminRepo.GetTasks();
            var users = await _adminRepo.GetUsers();
            ViewBag.Users = users;
            return View(tasks);
        }

        [HttpGet]
        public IActionResult CreateTask() => View();

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _adminRepo.CreateTask(model);
            return RedirectToAction("Tasks");
        }

        public async Task<IActionResult> DeleteTask(int id)
        {
            await _adminRepo.DeleteTask(id);
            return RedirectToAction("Tasks");
        }

        [HttpPost]
        public async Task<IActionResult> AssignTask(int taskId, int userId)
        {
            var result = await _adminRepo.AssignTask(taskId, userId);
            if (result == null)
            {
                TempData["Error"] = "Task or User not found";
                return RedirectToAction("Tasks");
            }
            TempData["Success"] = "Task assigned successfully";
            return RedirectToAction("Tasks");
        }

        [HttpGet]
        public async Task<IActionResult> EditTask(int id)
        {
            var tasks = await _adminRepo.GetTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(int id, TaskViewModel model)
        {
            model.Id = id;
            if (!ModelState.IsValid) return View(model);
            var result = await _adminRepo.UpdateTask(id, model);
            if (result == null)
            {
                TempData["Error"] = "Failed to update task.";
                return View(model);
            }
            TempData["Success"] = "Task updated successfully!";
            return RedirectToAction("Tasks");
        }
        
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
                context.Result = new RedirectToActionResult("Login", "Account", null);

            base.OnActionExecuting(context);
        }
    }
}
