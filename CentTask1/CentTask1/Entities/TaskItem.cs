using System.ComponentModel.DataAnnotations;
using CentTask1.Enum;
using CentTask1.ViewModels.BaseViewModels;

namespace CentTask1.Entities
{
    public class TaskItem : BaseViewModel
    { 
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]

        public ProjectTaskStatus Status { get; set; } 

    }
}
