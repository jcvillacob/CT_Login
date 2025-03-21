using CT_Login.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace CT_Login.Services
{
    public class ConductoresBukService : IConductoresBukService
    {
        private readonly HttpClient _httpClient;
        private readonly string _externalApiToken;

        public ConductoresBukService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _externalApiToken = configuration["EXTERNAL_API_TOKEN"];
        }

        public async Task<User> GetConductorAsync(string cedula)
        {
            var url = $"https://coorditanques.buk.co/api/v1/colombia/employees/{cedula}";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("auth_token", _externalApiToken);
            _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            // Se asume que la respuesta se mapea a un objeto dinámico o a un DTO específico.
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Validar si el empleado es conductor y está activo
            if (root.GetProperty("data").GetProperty("current_job").GetProperty("role").GetProperty("name").GetString() == "Conductor" &&
                root.GetProperty("data").GetProperty("status").GetString() == "activo")
            {
                var user = new User
                {
                    UsuarioID = 0,
                    Username = cedula,
                    Nombre = root.GetProperty("data").GetProperty("full_name").GetString(),
                    Cedula = cedula,
                    Roles = new List<Models.Role> { new Models.Role { NombreRol = "Conductor" } },
                    Subroles = new List<Models.Subrole>()
                };
                return user;
            }
            return null;
        }
    }
}
