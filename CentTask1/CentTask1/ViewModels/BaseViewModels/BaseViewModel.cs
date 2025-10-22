using System.ComponentModel.DataAnnotations;

namespace CentTask1.ViewModels.BaseViewModels
{
    public class BaseViewModel
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
        [Display(Name = "Updated On")]
        public DateTime UpdatedOn { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
    }
}
