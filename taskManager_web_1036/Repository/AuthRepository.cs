using Newtonsoft.Json;
using System.Text;
using taskManager_web_1036.Models.ViewModel;
using taskManager_web_1036.Repository.IRepository;
using static taskManager_web_1036.Models.ViewModel.LoginViewModel;

namespace taskManager_web_1036.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthRepository(
            IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
         

        public async Task<LoginResponseViewModel> Login(LoginViewModel model)
        {
            var client = _clientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                "https://localhost:7168/api/user/login", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<LoginResponseViewModel>(responseContent);
                return data;
            }

            return null;
        }

        public async Task<bool> Register(RegisterViewModel model)
        {
            var client = _clientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                "https://localhost:7168/api/user/register", content);

            return response.IsSuccessStatusCode;
        }
        public async Task<LoginResponseViewModel> RefreshToken(string refreshToken)
        {
            var client = _clientFactory.CreateClient();

            var json = JsonConvert.SerializeObject(new
            {
                refreshToken = refreshToken
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                "https://localhost:7168/api/user/refresh-token", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<LoginResponseViewModel>(responseContent);
            }

            return null;
        }

    }
}
