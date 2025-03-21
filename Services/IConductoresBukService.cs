using CT_Login.Models;
using System.Threading.Tasks;

namespace CT_Login.Services
{
    public interface IConductoresBukService
    {
        Task<User> GetConductorAsync(string cedula);
    }
}
