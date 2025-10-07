using System.ComponentModel.DataAnnotations;

namespace CentTask1.Models
{
    public class ProjectTask
    {
        public int id { get; set; }
 
        public  string name { get; set; }
        public  string description { get; set; }
        public  DateTime StartDate { get; set; }
        public  DateTime DueDate { get; set; }
        public  string priority { get; set; }   
        public  string AssignedTo { get; set; }
        public  string EquipmentType { get; set; }
        public  string TWR { get; set; }
    }
}
