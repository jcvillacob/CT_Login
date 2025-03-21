using CT_Login.Models;
using System.Threading.Tasks;

namespace CT_Login.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<LoginResponse> RefreshTokenAsync(string token);
    }
}
