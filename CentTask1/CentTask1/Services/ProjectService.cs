using CentTask1.DBC; 
using CentTask1.Entities;
using CentTask1.Interfaces;
using CentTask1.ViewModels.ProjectViewModels;
using CentTask1.ViewModels.TaskViewModels;
using Microsoft.EntityFrameworkCore;

namespace CentTask1.Services
{
    public class ProjectService: IProjectService
    {
        private readonly DataContext _dataContext;

        public ProjectService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //create
        public async Task<ProjectCreateViewModel> CreateProjectAsync(ProjectCreateViewModel project)
        {
            var projectEntity = new Project
            {
                ProjectName = project.ProjectName,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Budget = project.Budget,
                ClientName = project.ClientName,
                Status = project.Status,
                Manager = project.Manager,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            };
            _dataContext.Projects.Add(projectEntity);
            await _dataContext.SaveChangesAsync();
            var projectDto = new ProjectCreateViewModel
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Budget = project.Budget,
                ClientName = project.ClientName,
                Status = project.Status,
                Manager = project.Manager
            };
            return projectDto;
        }

        //GetAllProjects
        public async Task<IEnumerable<ProjectGetViewModel>> GetAllProjectsAsync()
        {
            var projects = await  _dataContext.Projects.Where(p => p.IsDeleted == false).Select(
                project => new ProjectGetViewModel
                {
                    Id = project.Id,
                    ProjectName = project.ProjectName,
                    Description = project.Description,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Budget = project.Budget,
                    ClientName = project.ClientName,
                    Status = project.Status,
                    Manager = project.Manager
                }
                ).ToListAsync();
            return projects;
        }

        //GetById
        public async Task<ProjectDetailViewModel?> GetProjectByIdAsync(Guid id)
        {
            var project= await _dataContext.Projects
                .FirstOrDefaultAsync(m => m.Id == id && m.IsDeleted == false);
            if (project == null)
                return null;

            var projectDetail = new ProjectDetailViewModel
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Description = project.Description,
                Budget = project.Budget,
                ClientName = project.ClientName,
                Status = project.Status,
                Manager = project.Manager,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                CreatedOn = project.CreatedOn,
                UpdatedOn = project.UpdatedOn
            };
            return projectDetail;
        }

        //Update
        public async Task<ProjectUpdateViewModel?> UpdateProjectAsync(Guid id, ProjectCreateViewModel updatedProject)
        {
            var existingProject = await _dataContext.Projects.FindAsync(id);
            if (existingProject == null)
            {
                return null;
            }
            existingProject.ProjectName = updatedProject.ProjectName;
            existingProject.Description = updatedProject.Description;
            existingProject.StartDate = updatedProject.StartDate;
            existingProject.EndDate = updatedProject.EndDate;
            existingProject.Budget = updatedProject.Budget;
            existingProject.ClientName = updatedProject.ClientName;
            existingProject.Status = updatedProject.Status;
            existingProject.Manager = updatedProject.Manager;
            existingProject.UpdatedOn = DateTime.UtcNow;
            await _dataContext.SaveChangesAsync();

            var projectDto = new ProjectUpdateViewModel
            {
                Id = existingProject.Id,
                ProjectName = existingProject.ProjectName,
                Description = existingProject.Description,
                StartDate = existingProject.StartDate,
                EndDate = existingProject.EndDate,
                Budget = existingProject.Budget,
                ClientName = existingProject.ClientName,
                Status = existingProject.Status,
                Manager = existingProject.Manager
            };
            return projectDto;
        }

        //Delete
        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            var project = await _dataContext.Projects.FindAsync(id);
            if (project == null)
            {
                return false;
            }
            project.IsDeleted = true;
            _dataContext.Projects.Update(project);
            await _dataContext.SaveChangesAsync();
            return true;
        }
 
    }
}
