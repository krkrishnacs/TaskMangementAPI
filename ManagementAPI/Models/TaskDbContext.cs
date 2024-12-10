using Microsoft.EntityFrameworkCore;

namespace ManagementAPI.Models
{
    public class TaskDbContext : DbContext
    {
        public  TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }
        public DbSet<TaskEntity> Tasks { get; set; }
    }
}
