using System.ComponentModel.DataAnnotations;
using CentTask1.Enum;
using CentTask1.ViewModels.BaseViewModels;

namespace CentTask1.ViewModels.ProjectViewModels
{
    public class ProjectCreateViewModel : BaseViewModel
    {
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        public string? Description { get; set; }
        public double? Budget { get; set; }
        [Display(Name = "Client Name")]
        public string? ClientName { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid status.")]
        [Display(Name = "Project Status")]
        public ProjectStatus Status { get; set; }

        public string? Manager { get; set; }
    }
}
