using CT_Login.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CT_Login.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @"SELECT TOP 1 * FROM Prod_CT_Usuarios WHERE Username = @Username";
            var user = await connection.QueryFirstOrDefaultAsync<User>(query, new { Username = username });
            return user;
        }

        public async Task<(List<Role> roles, List<Subrole> subroles)> GetUserRolesAndSubrolesAsync(int usuarioID)
        {
            using var connection = new SqlConnection(_connectionString);
            using var multi = await connection.QueryMultipleAsync("sp_GetUserRolesAndSubroles", new { UsuarioID = usuarioID },
                                                                commandType: CommandType.StoredProcedure);
            var roles = (await multi.ReadAsync<Role>()).AsList();
            var subroles = (await multi.ReadAsync<Subrole>()).AsList();
            return (roles, subroles);
        }
    }
}
