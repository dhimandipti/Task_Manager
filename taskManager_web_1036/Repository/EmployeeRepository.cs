using Newtonsoft.Json;
using System.Text;
using taskManager_web_1036.Models.ViewModel;
using taskManager_web_1036.Repository.IRepository;

namespace taskManager_web_1036.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string ApiBase = "https://localhost:7168/api/employee";

        public EmployeeRepository(IHttpClientFactory clientFactory,
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
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        public async Task<PagedTaskViewModel> GetMyTasksAsync(int userId, int page, int pageSize,string? search, string? status)
        {
            var client = GetAuthenticatedClient();           
            var url = $"https://localhost:7168/api/employee/my-tasks?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(search)) url += $"&search={Uri.EscapeDataString(search)}";
            if (!string.IsNullOrEmpty(status)) url += $"&status={status}";
            Console.WriteLine($"CALLING: {url}");
            var response = await client.GetAsync(url);        

            var data = await response.Content.ReadAsStringAsync();          

            if (!response.IsSuccessStatusCode)
                return new PagedTaskViewModel();

            return JsonConvert.DeserializeObject<PagedTaskViewModel>(data)
                   ?? new PagedTaskViewModel();
        }

        public async Task<bool> UpdateTaskStatusAsync(int taskId, int userId, string newStatus)
        {
            var client = GetAuthenticatedClient();

            var model = new UpdateStatusViewModel { Status = newStatus };
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");           
            var request = new HttpRequestMessage(HttpMethod.Patch,
                $"https://localhost:7168/api/employee/update-status/{taskId}")
            {
                Content = content
            };

            var response = await client.SendAsync(request);         

            return response.IsSuccessStatusCode;
        }
    }
}