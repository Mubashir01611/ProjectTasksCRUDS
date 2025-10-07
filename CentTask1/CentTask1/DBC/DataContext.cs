using Microsoft.EntityFrameworkCore;

namespace CentTask1.DBC
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DataContext()
        {
        }

        public DbSet<CentTask1.Models.ProjectTask> ProjectTasks { get; set; }
    }
}
