using CentTask1.ViewModels.ProjectViewModels;

namespace CentTask1.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectCreateViewModel> CreateProjectAsync(ProjectCreateViewModel project);
        Task<IEnumerable<ProjectGetViewModel>> GetAllProjectsAsync();
        Task<ProjectDetailViewModel?> GetProjectByIdAsync(Guid id);
        Task<ProjectUpdateViewModel?> UpdateProjectAsync(Guid id, ProjectCreateViewModel updatedProject);
        Task<bool> DeleteProjectAsync(Guid id);
    }
}
