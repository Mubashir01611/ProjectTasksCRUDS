using CentTask1.DBC;
using CentTask1.DTO.TaskDtos;
using CentTask1.Interfaces;
using CentTask1.Models;
using CentTask1.Services;
using CentTask1.ViewModels.TaskViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CentTask1.Controllers
{
    public class TasksController : Controller
    {
        private readonly IProjectService _projectService; 
        private readonly ITaskService _projectTaskService;

        public TasksController(ITaskService projectTaskService, IProjectService projectService)
        {
            _projectService = projectService; 
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
        public async Task<IActionResult> Details(Guid id)
        {
            var task = await _projectTaskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return PartialView("_Details", task);
        }

        //CreateMethod to Load CreateOrEdit Modal
        public async Task< IActionResult> CreateProjectTask(Guid? id)
        {
            TaskCreateViewModel task = new TaskCreateViewModel();
            //TaskCreateViewModel task = id.HasValue
            //? await _projectTaskService.GetTaskByIdAsync(id.Value) ?? new TaskCreateViewModel()
            //: new TaskCreateViewModel
            //{
            //    StartDate = DateTime.Today,
            //    EndDate = DateTime.Today
            //};//guid 

            return PartialView("_Create", task); // reuse same partial
        }

        //CreateOrEdit Method to handle form submission
        [HttpPost]
        public async Task<IActionResult> Create(TaskCreateViewModel task)
        {

            if (!ModelState.IsValid)
            {
                return PartialView("_Create", task); // reuse same view
            }

            //if (task.Id != )
            //{
            //    await _projectTaskService.UpdateTaskAsync(task.Id, task);
            //}
            //else
            //{
            //}
             await _projectTaskService.CreateTaskAsync(task);
            //abhi kaliye mai is method ko use nahi kr rha mgr, in future is py kaam karna hai
          //  TempData["SwalMessage"] = result.Message;
            return Json(new { success = true });
  
        }
    public async Task<IActionResult> EditProjectTaskForm(Guid id)
        {
            var task = await _projectTaskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
           
            return PartialView("_Edit", task); // reuse same partial
        }
        [HttpPut]
        public async Task<IActionResult> Edit(TaskUpdateViewModel task)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", task); // reuse same view
            }
            await _projectTaskService.UpdateTaskAsync(task.Id, task);
            return Json(new { success = true });
        }
        //Delete
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var isDeleted = await _projectTaskService.DeleteTaskAsync(id);

            if (!isDeleted)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }
        [HttpGet]
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
