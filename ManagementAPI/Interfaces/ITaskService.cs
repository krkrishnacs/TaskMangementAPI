using ManagementAPI.Models;

namespace ManagementAPI.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskEntity>> GetTasksAsync(string? status, DateTime? dueDate, int page, int pageSize);
        Task<TaskEntity> GetTaskByIdAsync(int id);
        Task<TaskEntity> CreateTaskAsync(TaskEntity task);
        Task<bool> UpdateTaskAsync(TaskEntity task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
