using System.ComponentModel.DataAnnotations;

namespace ManagementAPI.Models
{
    public class TaskEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public TaskStatus Status { get; set; } = TaskStatus.Pending;

        [Required]
        public DateTime DueDate { get; set; }
    }
    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }
}
