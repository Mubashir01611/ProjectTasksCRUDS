using CentTask1.DBC;
using CentTask1.DTO.ProjectDto;
using CentTask1.Entities;
using CentTask1.Interfaces;
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
        public async Task<GetProjectDto> CreateProjectAsync(GetProjectDto project)
        {
            var projectEntity = new Project
            {
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Budget = project.Budget,
                ClientName = project.ClientName,
                Status = project.Status,
                Manager = project.Manager
            };
            _dataContext.Projects.Add(projectEntity);
            await _dataContext.SaveChangesAsync();
            var projectDto = new GetProjectDto
            {
                Id = project.Id,
                Name = project.Name,
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
        public async Task<IEnumerable<GetProjectDto>> GetAllProjectsAsync()
        {
            var projects = await  _dataContext.Projects.ToListAsync();
            var projectDtos = projects.Select(project => new GetProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Budget = project.Budget,
                ClientName = project.ClientName,
                Status = project.Status,
                Manager = project.Manager
            }).ToList();
            return projectDtos;
        }

        //GetById
        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            var project= await _dataContext.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            return project;
        }

        //Update
        public async Task<GetProjectDto?> UpdateProjectAsync(int id, GetProjectDto updatedProject)
        {
            var existingProject = await _dataContext.Projects.FindAsync(id);
            if (existingProject == null)
            {
                return null;
            }
            existingProject.Name = updatedProject.Name;
            existingProject.Description = updatedProject.Description;
            existingProject.StartDate = updatedProject.StartDate;
            existingProject.EndDate = updatedProject.EndDate;
            existingProject.Budget = updatedProject.Budget;
            existingProject.ClientName = updatedProject.ClientName;
            existingProject.Status = updatedProject.Status;
            existingProject.Manager = updatedProject.Manager;
            await _dataContext.SaveChangesAsync();

            var projectDto = new GetProjectDto
            {
                Id = existingProject.Id,
                Name = existingProject.Name,
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
        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _dataContext.Projects.FindAsync(id);
            if (project == null)
            {
                return false;
            }
            _dataContext.Projects.Remove(project);
            await _dataContext.SaveChangesAsync();
            return true;
        }


        private bool ProjectExists(int id)
        {
            return _dataContext.Projects.Any(e => e.Id == id);
        }
    }
}
