using System.ComponentModel.DataAnnotations;
using CentTask1.Enum;
using CentTask1.Models;
using CentTask1.ViewModels.BaseViewModels;

namespace CentTask1.Entities
{
    public class Project : BaseViewModel
    {

        [Required]
        [StringLength(100)]
        public string ProjectName { get; set; }
        public string? Description { get; set; }
        public double? Budget { get; set; }
        public string? ClientName { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid status.")]
        public ProjectStatus Status { get; set; }
        public string? Manager { get; set; }    

        // Navigation Property
        public ICollection<ProjectTask>? ProjectTasks { get; set; }
    }
}
