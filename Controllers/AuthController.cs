using CT_Login.Models;
using CT_Login.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CT_Login.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            if (response.State == "error")
                return Unauthorized(new { message = response.Token });
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
                return BadRequest(new { message = "Token no proporcionado" });

            var response = await _authService.RefreshTokenAsync(request.Token);
            if (response.State == "error")
                return Unauthorized(new { message = response.Token });
            return Ok(response);
        }
    }
}
