using Microsoft.AspNetCore.Mvc;
using Proekt.Entites;
using Proekt.Service;
using Proekt.SQL_DB;

namespace Proekt.Contollers
{
    public class AuthController : ControllerBase
    {
        private AuthenticationService authService;
        private readonly Manager _manager;

        public AuthController(AuthenticationService authenticationService)
        {
            authService = authenticationService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            try
            {
                authService.RegisterUser(request);
                return Ok("Пользователь успешно зареган");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = authService.LoginUser(request);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}