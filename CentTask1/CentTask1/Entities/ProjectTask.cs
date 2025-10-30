
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CentTask1.Entities;
using CentTask1.Enum;
using CentTask1.ViewModels.BaseViewModels;

namespace CentTask1.Models
{
    public class ProjectTask : BaseViewModel
    { 

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(5, ErrorMessage = "Name must be at least 5 characters.")]
        public string TaskName { get; set; } 
        public string? Description { get; set; } 
        public string? Priority { get; set; }
        //public string? AssignedTo { get; set; }
        public string? EquipmentType { get; set; }
        public string? TWR { get; set; }
        public ProjectTaskStatus Status { get; set; }

        // Foreign Key 
        // Navigation Property
        [ForeignKey("ProjectId")]
        public Guid? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
