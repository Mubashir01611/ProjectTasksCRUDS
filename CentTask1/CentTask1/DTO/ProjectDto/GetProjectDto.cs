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
        public decimal? Budget { get; set; }
        public string? ClientName { get; set; }
        public string? Status { get; set; } = string.Empty;
        public string? Manager { get; set; }
    }
}
