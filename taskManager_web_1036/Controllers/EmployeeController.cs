
using Microsoft.AspNetCore.Mvc;
using taskManager_web_1036.Models.ViewModel;
using taskManager_web_1036.Repository.IRepository;

namespace taskManager_web_1036.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
        public EmployeeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }     
        public override void OnActionExecuting(
            Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Employee")
                context.Result = new RedirectToActionResult("Login", "Account", null);

            base.OnActionExecuting(context);
        }
     
        public async Task<IActionResult> MyTasks(
    int page = 1, int pageSize = 5,
    string? search = null, string? status = null)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var token = HttpContext.Session.GetString("JWToken");           

            if (userId == null)
            {                
                return RedirectToAction("Login", "Account");
            }
            var result = await _employeeRepo.GetMyTasksAsync(
                userId.Value, page, pageSize, search, status);      
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            return View(result);
        }      
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(
            int taskId, string newStatus)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var result = await _employeeRepo.UpdateTaskStatusAsync(
                taskId, userId.Value, newStatus);

            if (result)
                TempData["Success"] = "Status updated successfully!";
            else
                TempData["Error"] = "Failed to update status.";

            return RedirectToAction("MyTasks");
        }
    }
}