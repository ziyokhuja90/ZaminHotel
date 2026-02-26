using Microsoft.AspNetCore.Mvc;
using ZaminHotel.Application.Interfaces;

namespace ZaminHotel.Controllers
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            string fullName,
            string email,
            string password)
        {
            var token = await _authService.RegisterAsync(fullName, email, password);

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            string email,
            string password)
        {
            var token = await _authService.LoginAsync(email, password);

            return Ok(new { token });
        }
    }
}