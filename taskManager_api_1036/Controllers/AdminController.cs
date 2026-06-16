using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taskManager_api_1036.Data.Repository.IRepository;
using taskManager_api_1036.Model.ViewModel;

namespace taskManager_api_1036.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepo;
        public AdminController(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }    
     
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _adminRepo.CreateUserAsync(model);
            return Ok(user);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _adminRepo.GetAllUsersAsync();
            return Ok(users);
        }     
       
        [HttpPost("create-task")]
        public async Task<IActionResult> CreateTask(TaskViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = await _adminRepo.CreateTaskAsync(model);
            return Ok(task);
        }
            
     
        [HttpPost("assign-task")]
       
        public async Task<IActionResult> AssignTask([FromBody] AssignTaskViewModel model)
        {
            if (!ModelState.IsValid)return BadRequest(ModelState);
            var task = await _adminRepo.AssignTaskAsync(model.TaskId, model.UserId);
            if (task == null)return NotFound("Task or User not found");
            return Ok(task);
        }       
      
        [HttpGet("tasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _adminRepo.GetAllTasksAsync();
            return Ok(tasks);
        }
       
    
        [HttpDelete("delete-task/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _adminRepo.DeleteTaskAsync(id);
            if (!result) return NotFound("Task not found");
            return Ok("Task deleted successfully");
        }
     
        [HttpPut("update-task/{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskViewModel model)
        {
            var result = await _adminRepo.UpdateTaskAsync(id, model);
            if (result == null)return NotFound("Task not found");
            return Ok(result);
        }
    }
}
    
