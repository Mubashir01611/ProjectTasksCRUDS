using System.ComponentModel.DataAnnotations;
using CentTask1.Models;

namespace CentTask1.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public double? Budget { get; set; }
        public string? ClientName { get; set; }
        public string? Status { get; set; } = string.Empty;
        public string? Manager { get; set; }

        // Navigation Property
        public ICollection<ProjectTask>? ProjectTasks { get; set; }
    }
}
