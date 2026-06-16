using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using taskManager_api_1036.Data.Repository.IRepository;
using taskManager_api_1036.Model;

namespace taskManager_api_1036.Data.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSetting _appSettings;       
        public UserRepository(
             ApplicationDbContext context,
            IOptions<AppSetting> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        //public User Authenticate(string username, string password)
        //{

        //    var userInDb = _context.Users.FirstOrDefault(u =>u.Email.ToLower() == username.ToLower()
        //        &&
        //        u.PasswordHash == password
        //        );
        //    if (userInDb == null) return null;    
        //     // JWT CREATE

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key =Encoding.ASCII.GetBytes(_appSettings.Secret);
        //    var tokenDescriptor = new SecurityTokenDescriptor()
        //        {
        //            Subject =new ClaimsIdentity(new Claim[]
        //            {                    

        //                new Claim(ClaimTypes.NameIdentifier,userInDb.Id.ToString()),                  
        //                new Claim(ClaimTypes.Name,userInDb.FullName),
        //                new Claim(ClaimTypes.Role,  userInDb.Role)

        //            }),
        //            Expires =DateTime.UtcNow.AddDays(7),
        //            SigningCredentials =
        //            new SigningCredentials(
        //            new SymmetricSecurityKey(key),
        //            SecurityAlgorithms.HmacSha256Signature)

        //        };
        //    var token =tokenHandler.CreateToken(tokenDescriptor);         
        //    userInDb.Token = tokenHandler.WriteToken(token);        
        //    userInDb.PasswordHash = "";
        //    return userInDb;

        //}
        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
        public User Authenticate(string username, string password)
        {
            var userInDb = _context.Users.FirstOrDefault(u =>
                u.Email.ToLower() == username.ToLower()
                && u.PasswordHash == password
            );

            if (userInDb == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, userInDb.Id.ToString()),
            new Claim(ClaimTypes.Name, userInDb.FullName),
            new Claim(ClaimTypes.Role, userInDb.Role)
                }),

                // 🔥 CHANGE THIS (IMPORTANT)
                Expires = DateTime.UtcNow.AddMinutes(15),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // ACCESS TOKEN
            userInDb.Token = tokenHandler.WriteToken(token);

            // 🔥 CREATE REFRESH TOKEN
            userInDb.RefreshToken = GenerateRefreshToken();
            userInDb.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            _context.SaveChanges(); // IMPORTANT

            userInDb.PasswordHash = "";
            return userInDb;
        }
        public bool Register(User user)
        {

            _context.Users.Add(user);
            return Save();
        } 

        public bool IsUserExists(string username)
        {

            return _context.Users
                .Any(x =>x.Email.ToLower() == username.ToLower());
        }
        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault( x => x.Id == id);
        }
      
        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }
        public bool Update(User user)
        {
            _context.Users.Update(user);
            return Save();

        }         

        public bool Delete(User user)
        {
            _context.Users.Remove(user);
            return Save();
        }   


        public bool Save()
        {
            return _context.SaveChanges() > 0;

        }
        public User RefreshToken(string refreshToken)
        {
            var user = _context.Users.FirstOrDefault(x =>
                x.RefreshToken == refreshToken
            );

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return null;

            // generate new access token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // rotate refresh token
            user.RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            _context.SaveChanges();

            user.Token = tokenHandler.WriteToken(token);
            user.PasswordHash = "";

            return user;
        }

    }
}