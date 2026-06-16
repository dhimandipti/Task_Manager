using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using taskManager_api_1036.Data.Repository.IRepository;
using taskManager_api_1036.Model;
using taskManager_api_1036.Model.ViewModel;

namespace taskManager_api_1036.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (_userRepository.IsUserExists(user.Email))
            {
                return BadRequest("User already exists");
            }

            if (string.IsNullOrEmpty(user.Role))
            {
                user.Role = "Employee";
            }
            bool result =_userRepository.Register(user);
            if (!result)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(new
            {
                message = "User registered successfully"
            });

        }
      
        [HttpPost("login")]
        public IActionResult Login(LoginViewModel model)
        {
            var user =_userRepository.Authenticate(model.Email,model.Password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }
            return Ok(new
            {
                id = user.Id,
                name = user.FullName,
                email = user.Email,
                role = user.Role,
                token = user.Token,
                refreshToken = user.RefreshToken
            });
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken(TokenRequest model)
        {
            var user = _userRepository.RefreshToken(model.RefreshToken);
            if (user == null)
            {
                return Unauthorized("Invalid refresh token");
            }
            return Ok(new
            {
                token = user.Token,
                refreshToken = user.RefreshToken
            });
        }
    }
}


