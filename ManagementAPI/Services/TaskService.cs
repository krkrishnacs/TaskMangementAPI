using ManagementAPI.Interfaces;
using ManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementAPI.Services
{
    public class TaskService: ITaskService
    {
        private readonly TaskDbContext _context;
        public TaskService(TaskDbContext context)
        {
            _context = context;
        }
        public async Task<List<TaskEntity>> GetTasksAsync(string? status, DateTime? dueDate, int page, int pageSize)
        {
            try
            {
                var query = _context.Tasks.AsQueryable();

                if (!string.IsNullOrEmpty(status))
                    query = query.Where(t => t.Status.ToString() == status);

                if (dueDate.HasValue)
                    query = query.Where(t => t.DueDate <= dueDate.Value);

                return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log exception and rethrow it
                throw new Exception("An error occurred while fetching tasks.", ex);
            }
        }
        public async Task<TaskEntity> GetTaskByIdAsync(int id)
        {
            try
            {
                return await _context.Tasks.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching the task with ID: {id}", ex);
            }
        }
        public async Task<TaskEntity> CreateTaskAsync(TaskEntity task)
        {
            try
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                return task;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while creating the task.", ex);
            }
        }
        public async Task<bool> UpdateTaskAsync(TaskEntity task)
        {
            try
            {
                _context.Tasks.Update(task);
                var rowsAffected = await _context.SaveChangesAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while updating the task with ID: {task.Id}", ex);
            }
        }
        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null) return false;

                _context.Tasks.Remove(task);
                var rowsAffected = await _context.SaveChangesAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while deleting the task with ID: {id}", ex);
            }
        }
    }
}
