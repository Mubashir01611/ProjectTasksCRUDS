using Microsoft.AspNetCore.Mvc;
using CentTask1.Entities;
using CentTask1.Services;

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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                var createdProject = await _projectService.CreateProjectAsync(project);
                return RedirectToAction(nameof(GetAllProjects));
            }
            return View(project);
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
