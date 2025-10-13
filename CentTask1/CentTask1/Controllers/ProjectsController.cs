using CentTask1.DTO.ProjectDto;
using CentTask1.Entities;
using CentTask1.Models;
using CentTask1.Services;
using Microsoft.AspNetCore.Mvc;

namespace CentTask1.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ProjectService _projectService;

        public ProjectsController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: Projects
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return PartialView("_GetAllProjects", projects);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(project);
        }

        // GET: Projects/Create
        public async Task<IActionResult> CreateProject(int? id)
        {
            Project task = id.HasValue
            ? await _projectService.GetProjectByIdAsync(id.Value) ?? new Project()
            : new Project();
            var dto = new GetProjectDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                Budget = task.Budget,
                ClientName = task.ClientName,
                Status = task.Status,
                Manager = task.Manager
            };
            return PartialView("_CreateProjectModal", dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GetProjectDto projectDto)
        {
            if (ModelState.IsValid)
            {
                var createdProject = await _projectService.CreateProjectAsync(projectDto);
                return RedirectToAction(nameof(GetAllProjects));
            }
            return View(projectDto);
        }

        // POST: Projects/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _projectService.DeleteProjectAsync(id);
            return RedirectToAction(nameof(GetAllProjects));
        }
    }
}
