using taskManager_api_1036.Model;

namespace taskManager_api_1036.Data.Repository.IRepository
{
    public interface IUserRepository
    {

      
        User Authenticate(string username, string password);
        bool Register(User user);
        bool IsUserExists(string username);
        User GetUserById(int id);
        IEnumerable<User> GetUsers();
        bool Update(User user);
        bool Delete(User user);
        User RefreshToken(string refreshToken);
        bool Save();
       

    }
}
