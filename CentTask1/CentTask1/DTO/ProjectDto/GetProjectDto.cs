using System.ComponentModel.DataAnnotations;

namespace CentTask1.DTO.ProjectDto
{
    public class GetProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double? Budget { get; set; }
        public string? ClientName { get; set; }
        public bool Status { get; set; }
        public string? Manager { get; set; }
    }
}
