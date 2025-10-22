using CentTask1.DTO.ProjectDto;
using CentTask1.Entities;
using CentTask1.ViewModels.ProjectViewModels;

namespace CentTask1.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectCreateViewModel> CreateProjectAsync(ProjectCreateViewModel project);
        Task<IEnumerable<GetProjectDto>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(Guid id);
        Task<GetProjectDto?> UpdateProjectAsync(Guid id, GetProjectDto updatedProject);
        Task<bool> DeleteProjectAsync(Guid id);
    }
}
