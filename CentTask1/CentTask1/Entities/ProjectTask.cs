using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CentTask1.Entities;

namespace CentTask1.Models
{
    public class ProjectTask
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(5, ErrorMessage = "Name must be at least 5 characters.")]
        public string name { get; set; }

        public string? description { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Due Date is required.")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } 

        public string? priority { get; set; }
        public string? AssignedTo { get; set; }
        public string? EquipmentType { get; set; }
        public string? TWR { get; set; }

        // Foreign Key

        // Navigation Property
        [ForeignKey("ProjectId")]
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
