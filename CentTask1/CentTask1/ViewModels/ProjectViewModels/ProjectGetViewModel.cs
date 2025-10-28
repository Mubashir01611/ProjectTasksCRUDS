using System.ComponentModel.DataAnnotations;
using CentTask1.ViewModels.BaseViewModels;

namespace CentTask1.ViewModels.ProjectViewModels
{
    public class ProjectGetViewModel : BaseViewModel
    {
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        public string? Description { get; set; }
        public double? Budget { get; set; }
        [Display(Name = "Client Name")]
        public string? ClientName { get; set; }
        [Display(Name = "Project Status")]
        public bool Status { get; set; }
        public string? Manager { get; set; }
    }
}
