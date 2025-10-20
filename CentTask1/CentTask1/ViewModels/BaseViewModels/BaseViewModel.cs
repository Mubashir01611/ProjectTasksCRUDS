namespace CentTask1.ViewModels.BaseViewModels
{
    public class BaseViewModel
    {
        public Guid TaskId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; } 
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
    }
}
