using CentTask1.ViewModels.BaseViewModels;

namespace CentTask1.ViewModels.ProjectViewModels
{
    public class ProjectGetViewModel : BaseViewModel
    {
        public string ProjectName { get; set; }
        public string? Description { get; set; }
        public double? Budget { get; set; }
        public string? ClientName { get; set; }
        public bool Status { get; set; }
        public string? Manager { get; set; }
    }
}
