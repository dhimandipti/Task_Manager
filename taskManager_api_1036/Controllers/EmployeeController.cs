using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using taskManager_api_1036.Data.Repository.IRepository;
using taskManager_api_1036.Model.ViewModel;

namespace taskManager_api_1036.Controllers
{
    [Route("api/employee")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks(
           [FromQuery] int page = 1,
           [FromQuery] int pageSize = 10,
           [FromQuery] string? search = null,
           [FromQuery] string? status = null)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token." });
            var result = await _employeeRepository.GetMyTasksAsync(
                userId, page, pageSize, search, status);
            return Ok(result);
        }
       
        [HttpPatch("update-status/{taskId}")]
        public async Task<IActionResult> UpdateTaskStatus(
            int taskId, [FromBody] UpdateStatusViewModel model)
        {
            if (!ModelState.IsValid)return BadRequest(ModelState);
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token." });
            var success = await _employeeRepository.UpdateTaskStatusAsync(
                taskId, userId, model.Status);
            if (!success)
                return NotFound(new { message = "Task not found or not assigned to you." });
            return Ok(new { message = "Task status updated successfully." });
        }
    }
}
    

