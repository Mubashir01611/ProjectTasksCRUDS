using CentTask1.Interfaces;
using CentTask1.ViewModels.TaskViewModels;
using Microsoft.AspNetCore.Mvc;
using CentTask1.Extensions;
using Microsoft.EntityFrameworkCore;

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
       
        //get all task using queryable
        [HttpPost]
        public async Task<IActionResult> GetTasksDataTable()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var length = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "5");
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnIndex = Convert.ToInt32(Request.Form["order[0][column]"].FirstOrDefault() ?? "0");
            var sortColumn = Request.Form[$"columns[{sortColumnIndex}][data]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault() ?? "asc";

            var query =  _projectTaskService.GetAllTasksAsync();
            //var query = taskList.AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(p =>
                    p.TaskName.Contains(searchValue) ||
                    (p.Description != null && p.Description.Contains(searchValue)) ||
                    (p.TaskName != null && p.TaskName.Contains(searchValue)) ||
                    (p.ProjectName != null && p.ProjectName.Contains(searchValue))
                );
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = sortDirection == "asc"
                    ? query.OrderByDynamic(sortColumn, true)
                    : query.OrderByDynamic(sortColumn, false);
            }

            var recordsTotal = await query.CountAsync();
            var data = await query.Skip(start).Take(length).ToListAsync();

            var formattedData = data.Select(p => new {
                p.Id,
                p.TaskName,
                p.Description,
                startDate = p.StartDate.ToString("yyyy-MM-dd"),
                endDate = p.EndDate.ToString("yyyy-MM-dd"),
                p.Priority,
                p.EquipmentType,
                p.TWR,
                p.ProjectName,
                p.Status
            });

            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = formattedData
            });
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
            task.Status = Enum.ProjectTaskStatus.NotStarted;
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
        [HttpPost]
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
                : projects.Where(p => p.ProjectName.Contains(term, StringComparison.OrdinalIgnoreCase));

            var result = filtered.Select(p => new { id = p.Id, text = p.ProjectName }).ToList();
            return Json(new { results = result });
        }
    }
}
