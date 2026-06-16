using taskManager_web_1036.Models.ViewModel;

namespace taskManager_web_1036.Repository.IRepository
{
    public interface IAuthRepository
    {
        Task<LoginResponseViewModel> Login(LoginViewModel model);
        Task<bool> Register(RegisterViewModel model);
        
    }
}
