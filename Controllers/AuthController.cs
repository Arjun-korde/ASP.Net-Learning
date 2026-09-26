using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Services;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterRequest request
        )
        {
            try
            {
                await _authService.RegisterAsync(request);

                return StatusCode(
                    StatusCodes.Status201Created
                );
            }
            catch(InvalidOperationException e)
            {
                return Conflict( new
                {
                    message = e.Message
                }
                );
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginRequest request
        )
        {
            var response = await _authService.LoginAsync(request);

            if(response == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                }
                );
            }

            return Ok(response);
        }
    }
}
