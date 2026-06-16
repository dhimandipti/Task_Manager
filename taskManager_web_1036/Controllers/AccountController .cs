
using Microsoft.AspNetCore.Mvc;
using taskManager_web_1036.Models.ViewModel;
using taskManager_web_1036.Repository.IRepository;

namespace taskManager_web_1036.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthRepository _authRepository;

        public AccountController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }   
    
       [HttpGet]
        public IActionResult Login()
        {         
            if (HttpContext.Session.GetString("JWToken") != null)
            {
                var role = HttpContext.Session.GetString("Role");
                if (role == "Admin")
                    return RedirectToAction("Index", "Admin");
                if (role == "Employee")
                    return RedirectToAction("MyTasks", "Employee");
            }
            return View();
        }         

      
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _authRepository.Login(model);

            if (result == null)
            {
                ViewBag.Error = "Invalid Email or Password";
                return View(model);
            }         
            HttpContext.Session.SetString("JWToken", result.Token);
            HttpContext.Session.SetString("RefreshToken", result.RefreshToken ?? "");
            HttpContext.Session.SetString("Role", result.Role);
            HttpContext.Session.SetString("Email", result.Email ?? "");
            HttpContext.Session.SetInt32("UserId", result.Id);       
            if (result.Role == "Admin")
                return RedirectToAction("Index", "Admin");
            if (result.Role == "Employee")
                return RedirectToAction("MyTasks", "Employee");

            return RedirectToAction("Login");
        }
    
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }     
           
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authRepository.Register(model);

            if (!result)
            {
                ViewBag.Error = "Registration failed. Please try again.";
                return View(model);
            }

            TempData["Success"] = "Registered successfully! Please login.";
            return RedirectToAction("Login");
        }       
     
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

