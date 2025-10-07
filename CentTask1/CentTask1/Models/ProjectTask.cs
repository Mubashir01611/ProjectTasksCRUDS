using System.ComponentModel.DataAnnotations;

namespace CentTask1.Models
{
    public class ProjectTask
    {
        public int id { get; set; }
        [Required]
        public  string name { get; set; }
        public  string? description { get; set; }
        [Required]
        public  DateOnly StartDate { get; set; }
        [Required]
        public DateOnly DueDate { get; set; }
        public  string? priority { get; set; }   
        public  string? AssignedTo { get; set; }
        public  string? EquipmentType { get; set; }
        public  string? TWR { get; set; }
    }
}
