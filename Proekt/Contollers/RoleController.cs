using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proekt.Repository;

namespace Proekt.Contollers
{
    [ApiController]
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public RoleController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("assign")]
        public IActionResult AssignRole([FromBody] AssignRoleRequest request)
        {
            try
            {
                _userRepository.AssingRoleToUser(request.UserId, request.RoleId);
                return Ok("Роль успешно назначена");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class AssignRoleRequest
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
