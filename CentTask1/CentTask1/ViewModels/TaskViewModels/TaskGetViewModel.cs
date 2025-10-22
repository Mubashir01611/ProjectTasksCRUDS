using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CentTask1.ViewModels.TaskViewModels
{
    public class TaskGetViewModel : BaseViewModels.BaseViewModel
    {
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        [Display(Name = "Equipment Type")]
        public string? EquipmentType { get; set; }
        public string? TWR { get; set; }
        public string? ProjectId { get; set; }
        [Display(Name = "Project Name")]
        public string? ProjectName { get; set; }
    }
}
