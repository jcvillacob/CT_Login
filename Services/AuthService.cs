using CT_Login.Models;
using CT_Login.Repositories;
using CT_Login.Helpers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CT_Login.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConductoresBukService _bukService;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConductoresBukService bukService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _bukService = bukService;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            User payload = null;

            // Caso especial: username == password, se usa el servicio externo.
            if (request.Username == request.Password)
            {
                payload = await _bukService.GetConductorAsync(request.Username);
                if (payload == null)
                    return new LoginResponse { State = "error", Token = "El empleado no es conductor" };
            }
            else
            {
                // Buscar usuario en la BD
                var user = await _userRepository.GetUserByUsernameAsync(request.Username);
                if (user == null)
                    return new LoginResponse { State = "error", Token = "Usuario no encontrado." };

                // Comparación directa de contraseñas (nota: se recomienda usar hashing)
                if (request.Password != user.Password)
                    return new LoginResponse { State = "error", Token = "Contraseña incorrecta." };

                // Obtener roles y subroles (optimizados en una sola llamada al SP)
                var (roles, subroles) = await _userRepository.GetUserRolesAndSubrolesAsync(user.UsuarioID);
                user.Roles = roles;
                user.Subroles = subroles;
                payload = user;
            }

            // Generar el token JWT
            string jwtSecret = _configuration["JWT_SECRET_KEY"];
            // Se establece una expiración de 5 horas
            string token = JwtHelper.GenerateToken(payload, jwtSecret, 5);
            return new LoginResponse { State = "successful", Token = token };
        }

        public async Task<LoginResponse> RefreshTokenAsync(string token)
        {
            // La implementación de refresh-token puede variar.
            // Una opción es validar el token, extraer las claims y generar uno nuevo.
            // Aquí se muestra un ejemplo básico.
            var principal = JwtHelper.GetPrincipalFromExpiredToken(token, _configuration["JWT_SECRET_KEY"]);
            if (principal == null)
                return new LoginResponse { State = "error", Token = "Token inválido" };

            // Se puede reconstruir el payload a partir de las claims y generar un nuevo token.
            // Por simplicidad, se reutiliza el token.
            string newToken = JwtHelper.GenerateTokenFromClaims(principal.Claims, _configuration["JWT_SECRET_KEY"], 5);
            return new LoginResponse { State = "successful", Token = newToken };
        }
    }
}
