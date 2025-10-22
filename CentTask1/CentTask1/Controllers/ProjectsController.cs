using CentTask1.Interfaces; 
using CentTask1.ViewModels.ProjectViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CentTask1.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        public IActionResult LoadProjects()
        {
          return PartialView("_GetAllProjects");
        }
        // GET: Projects
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Json(projects);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetProjectByIdAsync(id.Value);
            if (project == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsProjectModal",project);
        }

        //Create
        public async Task<IActionResult> CreateProject(Guid? id)
        {
            ProjectCreateViewModel createViewModel = new ProjectCreateViewModel();
            if(id.HasValue)
            {
                var project = await _projectService.GetProjectByIdAsync(id.Value);
                createViewModel = new ProjectCreateViewModel
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
            }
            return PartialView("_CreateProjectModal", createViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateViewModel projectDto)
        {
            if(!ModelState.IsValid)
            {
                return PartialView("_CreateProjectModal", projectDto);
            }
            if (projectDto.Id != Guid.Empty)
            {
                // Update mode
                await _projectService.UpdateProjectAsync(projectDto.Id, projectDto);
            }
            else
            {
                // Create mode
                await _projectService.CreateProjectAsync(projectDto);
            }
            return Json(new { success = true });
            
        }

        // Delete
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var isDeleted = await _projectService.DeleteProjectAsync(id);
            if (!isDeleted)
            {
                return Json(new { success = false });
            }
            else
            {
                return Json(new { success = true });
            }
        }
    }
}
