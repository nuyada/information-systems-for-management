using Microsoft.AspNetCore.Mvc;
using Proekt.Models;
using Proekt.Service;

namespace Proekt.Contollers
{
    public class GroupController: ControllerBase
    {
        private readonly GroupService Service;
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
        public GroupController(GroupService Service) => this.Service = Service;
        [HttpPost("groups")]
        public IActionResult CreateGroup([FromBody] GroupE group)
        {
            if (group == null)
            {
                return BadRequest("Group data is missing.");
            }

            try
            {
                var userId = GetCurrentUserId();
                group.CreatedBy = userId;
                var groupId = Service.CreateGroup(group);
                return Ok(new { GroupId = groupId, Message = "Group created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("user/groups")]
        public IActionResult GetUserGroups()
        {
            try
            {
                var userId = GetCurrentUserId();
                var groups = Service.GetUserGroups(userId);
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("{groupId}/add-user/{userId}")]
        public IActionResult AddUserToGroup(int groupId, int userId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                Service.AddUserToGroup(userId, groupId, currentUserId);
                return Ok("User added to group successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("{groupId}")]
        public IActionResult DeleteGroup(int groupId)
        {
            try
            {
                var userId = GetCurrentUserId();
                Service.DeleteGroup(groupId, userId);
                return Ok("Group deleted successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{groupId}")]
        public IActionResult GetGroupById(int groupId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var group = Service.GetGroupById(groupId, userId);
                if (group == null)
                {
                    return NotFound("Group not found or you are not a member.");
                }
                return Ok(group);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
