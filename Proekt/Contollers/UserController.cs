namespace Proekt.Contollers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Proekt.Entites;
    using Proekt.Models;
    using Proekt.Service;
    using System.Collections.Generic;

    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        private int GetCurrentUserId()
        {
            var userIdClaim = User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }
            if (!int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid user ID in token.");
            }
            return userId;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        
        [HttpGet("получить инфу о юзере")]
        public IActionResult GetUserById()
        {
            var id = GetCurrentUserId();
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }
            return Ok(user);
        }

        [HttpPut("изменить данные")]
        public IActionResult UpdateUser([FromBody] UserModel user)
        {
            var id = GetCurrentUserId();
           
            try
            {
                _userService.UpdateUser(user,id);
                return Ok("Пользователь успешно обновлен.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userService.DeleteUser(id);
                return Ok("Пользователь успешно удален.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}