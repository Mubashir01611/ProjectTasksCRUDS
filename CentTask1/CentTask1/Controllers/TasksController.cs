using CentTask1.DBC;
using CentTask1.Models;
using CentTask1.Services;
using Microsoft.AspNetCore.Mvc;

namespace CentTask1.Controllers
{
    public class TasksController : Controller
    {
        private readonly ProjectTaskService _projectTaskService;

        public TasksController(ProjectTaskService projectTaskService)
        {
            _projectTaskService = projectTaskService;
        }

        //created this function to load the table in div container 
        public IActionResult LoadTaskTable()
        {
            return PartialView("GetAllTasks");
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

            return PartialView("Details", task);
        }

        //CreateMethod to Load CreateOrEdit Modal
        public async Task< IActionResult> CreateProjectTask(int? id)
        {
            ProjectTask task = id.HasValue
            ? await _projectTaskService.GetTaskByIdAsync(id.Value) ?? new ProjectTask()
            : new ProjectTask();
 
            return PartialView("Create", task); // reuse same partial
        }

        //CreateOrEdit Method to handle form submission
        [HttpPost]
        public async Task<IActionResult> Create(ProjectTask task)
        {

            if (!ModelState.IsValid)
            {
                return PartialView("Create", task); // reuse same view
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

        public IActionResult createsample()
        {
            return View();
        }
    }
}
