using CentTask1.ViewModels.BaseViewModels;

namespace CentTask1.ViewModels.TaskViewModels
{
    public class TaskDetailViewModel : BaseViewModel
    {
       // public int Id { get; set; }
        public string TaskName { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public string? AssignedTo { get; set; }
        public string? EquipmentType { get; set; }
        public string? TWR { get; set; }
        public string? ProjectId { get; set; }
        public string? ProjectName { get; set; }
    }
}
