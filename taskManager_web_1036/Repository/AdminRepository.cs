using Newtonsoft.Json;
using System.Text;
using taskManager_web_1036.Models.ViewModel;
using taskManager_web_1036.Repository.IRepository;

namespace taskManager_web_1036.Repository
{

    public class AdminRepository : IAdminRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor; 

        public AdminRepository(
            IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor) 
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }       
        private HttpClient GetAuthenticatedClient()
        {
            var client = _clientFactory.CreateClient();
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        // ================= USERS =================
        public async Task<List<CreateUserViewModel>> GetUsers()
        {
            var client = GetAuthenticatedClient(); 
            var response = await client.GetAsync("https://localhost:7168/api/admin/users");
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CreateUserViewModel>>(data);
        }

        public async Task<CreateUserViewModel> CreateUser(CreateUserViewModel model)
        {
            var client = GetAuthenticatedClient(); 
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(
                "https://localhost:7168/api/admin/create-user", content);
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CreateUserViewModel>(data);
        }

        // ================= TASKS =================
        public async Task<List<TaskViewModel>> GetTasks()
        {
            var client = GetAuthenticatedClient(); 
            var response = await client.GetAsync("https://localhost:7168/api/admin/tasks");
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<TaskViewModel>>(data);
        }

        public async Task<TaskViewModel> CreateTask(TaskViewModel model)
        {
            var client = GetAuthenticatedClient();

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "https://localhost:7168/api/admin/create-task";
            var response = await client.PostAsync(url, content);       
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API FAILED: {error}");
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TaskViewModel>(data);
        }

        public async Task<bool> DeleteTask(int id)
        {
            var client = GetAuthenticatedClient();
            var response = await client.DeleteAsync(
                $"https://localhost:7168/api/admin/delete-task/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<TaskViewModel> AssignTask(int taskId, int userId)
        {
            var client = GetAuthenticatedClient();
            var model = new
            {
                taskId = taskId,
                userId = userId
            };

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                "https://localhost:7168/api/admin/assign-task",
                content
            );

            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(data); 
            }

            return JsonConvert.DeserializeObject<TaskViewModel>(data);
        }
        public async Task<TaskViewModel?> UpdateTask(int id, TaskViewModel model)
        {
            var client = GetAuthenticatedClient();          
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");         

            model.Id = id; 

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine($"PUT BODY: {json}");

            var response = await client.PutAsync(
                $"https://localhost:7168/api/admin/update-task/{id}",
                content);

            Console.WriteLine($"PUT STATUS: {response.StatusCode}");

            var data = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"PUT RESPONSE: {data}");

            if (!response.IsSuccessStatusCode)
                return null;

            try
            {
                return JsonConvert.DeserializeObject<TaskViewModel>(data);
            }
            catch
            {
                return null;
            }
        }
    }
}

