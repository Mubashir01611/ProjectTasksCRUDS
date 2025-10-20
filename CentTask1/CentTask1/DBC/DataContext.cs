using CentTask1.Entities;
using CentTask1.Models;
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

        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectTask>()
                .HasKey(p => p.Id);  // Explicitly define GUID as PK
            modelBuilder.Entity<Project>()
                .HasKey(p => p.Id);  // Explicitly define GUID as PK
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<ProjectTask>()
        //        .HasOne(pt => pt.Project)              // Each task has one project
        //        .WithMany(p => p.ProjectTasks)                // Each project has many tasks
        //        .HasForeignKey(pt => pt.ProjectId)     // Foreign key in ProjectTask
        //        .OnDelete(DeleteBehavior.SetNull);     // Optional: set FK to null if project is deleted
        //}

    }
}
