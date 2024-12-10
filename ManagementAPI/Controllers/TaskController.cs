using ManagementAPI.Interfaces;
using ManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;
        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] string? status, [FromQuery] DateTime? dueDate, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var tasks = await _taskService.GetTasksAsync(status, dueDate, page, pageSize);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching tasks.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null) return NotFound(new { Message = "Task not found." });

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the task.", Detail = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskEntity task)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var createdTask = await _taskService.CreateTaskAsync(task);

                return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, new { Message = "Task created successfully.", Task = createdTask });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the task.", Detail = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskEntity task)
        {
            try
            {
                if (id != task.Id) return BadRequest(new { Message = "ID mismatch." });

                var success = await _taskService.UpdateTaskAsync(task);
                if (!success) return NotFound(new { Message = "Task not found." });

                return Ok(new { Message = "Task updated successfully.", Task = task });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the task.", Detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var success = await _taskService.DeleteTaskAsync(id);
                if (!success) return NotFound(new { Message = "Task not found." });

                return Ok(new { Message = "Task deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the task.", Detail = ex.Message });
            }
        }

    }
}
