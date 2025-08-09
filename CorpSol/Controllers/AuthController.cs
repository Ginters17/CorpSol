using CorpSol.Models.DTO;
using CorpSol.Services;
using Microsoft.AspNetCore.Mvc;

namespace CorpSol.Controllers
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
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            return _authService.Login(request);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            return await _authService.RegisterAsync(request);
        }
    }
}
