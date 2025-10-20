using CentTask1.DTO.ProjectDto;
using CentTask1.Entities;

namespace CentTask1.Interfaces
{
    public interface IProjectService
    {
        Task<GetProjectDto> CreateProjectAsync(GetProjectDto project);
        Task<IEnumerable<GetProjectDto>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task<GetProjectDto?> UpdateProjectAsync(int id, GetProjectDto updatedProject);
        Task<bool> DeleteProjectAsync(int id);
    }
}
