using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentTask1.DTO
{
    public class ProjectTaskDto
    {
        public int id { get; set; }
        public string name { get; set; }

        public string? description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string? priority { get; set; }
        public string? AssignedTo { get; set; }
        public string? EquipmentType { get; set; }
        public string? TWR { get; set; }
        public string? ProjectName { get; set; }
    }
}
