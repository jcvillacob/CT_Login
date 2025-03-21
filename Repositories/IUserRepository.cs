using CT_Login.Models;
using System.Threading.Tasks;

namespace CT_Login.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<(List<Role> roles, List<Subrole> subroles)> GetUserRolesAndSubrolesAsync(int usuarioID);
    }
}
