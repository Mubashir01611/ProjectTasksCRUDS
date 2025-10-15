using CentTask1.DBC;
using CentTask1.DTO.TaskDtos;
using CentTask1.Models;
using CentTask1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CentTask1.Controllers
{
    public class TasksController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly DataContext _context;
        private readonly TaskService _projectTaskService;

        public TasksController(TaskService projectTaskService,DataContext context,ProjectService projectService)
        {
            _projectService = projectService;
             _context = context;
            _projectTaskService = projectTaskService;
        }

        //created this function to load the table in div container 
        public IActionResult LoadTaskTable()
        {
            return PartialView("_GetAllTasks");
        }
        // GET all tasks from db to display in table
        [HttpGet]
        public async Task<IActionResult> GetAllTasks ()
        {
            var projectTasks = await _projectTaskService.GetAllTasksAsync();
            return Json(projectTasks);
        }

        //Details
        public async Task<IActionResult> Details(int id)
        {
            var task = await _projectTaskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return PartialView("_Details", task);
        }

        //CreateMethod to Load CreateOrEdit Modal
        public async Task< IActionResult> CreateProjectTask(int? id)
        {
            var projects = _context.Projects
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

            ViewBag.ProjectList = projects;
            ProjectTask task = id.HasValue
            ? await _projectTaskService.GetTaskByIdAsync(id.Value) ?? new ProjectTask()
            : new ProjectTask
            {
                StartDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(7)
            };

            return PartialView("_Create", task); // reuse same partial
        }

        //CreateOrEdit Method to handle form submission
        [HttpPost]
        public async Task<IActionResult> Create(GetTaskDto task)
        {

            if (!ModelState.IsValid)
            {
                return PartialView("_Create", task); // reuse same view
            }

            if (task.id > 0)
            {
                await _projectTaskService.UpdateTaskAsync(task.id, task);
            }
            else
            {
                await _projectTaskService.CreateTaskAsync(task);
            }

            return Json(new { success = true });
  
        }
    
        //Delete
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isDeleted = await _projectTaskService.DeleteTaskAsync(id);

            if (!isDeleted)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        public async Task<IActionResult> GetProjectsForDropdown(string term)
        {
            var projects = await _projectService.GetAllProjectsAsync();
            var filtered = string.IsNullOrEmpty(term)
                ? projects
                : projects.Where(p => p.Name.Contains(term, StringComparison.OrdinalIgnoreCase));

            var result = filtered.Select(p => new { id = p.Id, text = p.Name }).ToList();
            return Json(new { results = result });
        }
    }
}
