using Microsoft.AspNetCore.Mvc;
using Proekt.Entites;
using Proekt.Models;
using Proekt.Service;
using System.Security.Claims;

[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly ITaskService taskService;

    public TaskController(ITaskService taskService) => this.taskService = taskService;

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

    [HttpGet]
    public IActionResult GetAllTasks()
    {
        var userId = GetCurrentUserId();
        var tasks = taskService.GetAllTasks(userId);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public IActionResult GetTaskById(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var task = taskService.GetTaskById(id,userId);
            return Ok(task);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult CreateTask([FromBody] TaskE task)
    {
        if (task == null)
        {
            return BadRequest("Task data is missing.");
        }
        try
        {
            var userId = GetCurrentUserId();
            task.UserId = userId;
            taskService.CreateTask(task, userId);
            return Ok("Task created successfully.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, [FromBody] TaskE task)
    {
        try
        {
            var userId = GetCurrentUserId();
            task.Id = id;
            taskService.UpdateTask(task, userId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            taskService.DeleteTask(id, userId);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{taskId}/assign-category/{categoryId}")]
    public IActionResult AssignCategoryToTask(int taskId, int categoryId)
    {
        try
        {
            var userId = GetCurrentUserId();
            taskService.AssignCategoryToTask(taskId, categoryId, userId);
            return Ok("Категория успешно назначена задаче.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("group/{groupId}/tasks")]
    public IActionResult GetGroupTasks(int groupId)
    {
        var id = GetCurrentUserId();
        var tasks = taskService.GetGroupTasks(groupId,id);
        return Ok(tasks);
    }

    [HttpPost("group/{groupId}/tasks")]
    public IActionResult CreateGroupTask([FromBody] GroupTaskE task, int groupId)
    {
        if (task == null)
        {
            return BadRequest("Task data is missing.");
        }
        var id = GetCurrentUserId();
        try
        {
            var userId = GetCurrentUserId();
            task.CreatedBy = userId;
            task.GroupId = groupId;
            taskService.CreateGroupTask(task,id);
            return Ok("Group task created successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("group/tasks/{taskId}/{GroupId}")]
    public IActionResult GetGroupTaskById(int taskId,int GroupId)
    {
        var id = GetCurrentUserId();
        var task = taskService.GetGroupTaskById(taskId,id,GroupId);
        if (task == null)
        {
            return NotFound("Task not found.");
        }
        return Ok(task);
    }

    [HttpPut("group/tasks/{taskId}")]
    public IActionResult UpdateGroupTask(int taskId, [FromBody] GroupTaskE task)
    {
        var id = GetCurrentUserId();
        try
        {
            var userId = GetCurrentUserId();
            task.Id = taskId;
            taskService.UpdateGroupTask(task,id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("group/tasks/{taskId}/{GroupeId}")]
    public IActionResult DeleteGroupTask(int taskId,int GroupeId)
    {
        var id = GetCurrentUserId();
        try
        {
            taskService.DeleteGroupTask(taskId,id,GroupeId);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}