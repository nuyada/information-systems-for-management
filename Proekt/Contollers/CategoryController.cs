using Microsoft.AspNetCore.Mvc;
using Proekt.Service;

namespace Proekt.Contollers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
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
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] string name)
        {
            try
            {
                _categoryService.CreateCategory(name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/tasks")]
        public IActionResult GetTasksByCategoryId(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var tasks = _categoryService.GetCategoryById(id,userId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}