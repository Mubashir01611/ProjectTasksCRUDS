using CentTask1.Extensions;
using CentTask1.Interfaces;
using CentTask1.Services;
using CentTask1.ViewModels.ProjectViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        public async Task<IActionResult> GetProjectsDataTable()
        {
            // Read DataTables parameters
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var lengthRaw = Request.Form["length"].FirstOrDefault();
            var length = lengthRaw == "-1" ? int.MaxValue : Convert.ToInt32(lengthRaw ?? "5");
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            var sortColumnIndex = Convert.ToInt32(Request.Form["order[0][column]"].FirstOrDefault() ?? "0");
            var sortColumn = Request.Form[$"columns[{sortColumnIndex}][data]"].FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault() ?? "asc";

            // Get all projects (you can optimize with IQueryable for large datasets)
            var query = _projectService.GetProjectsQueryable();
            if (length == -1)
            {
                length = int.MaxValue; // ✅ Fetch all rows
            }

            // Filtering
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(p =>
                    p.ProjectName.Contains(searchValue) ||
                    (p.Description != null && p.Description.Contains(searchValue)) ||
                    (p.ClientName != null && p.ClientName.Contains(searchValue)) ||
                    (p.Manager != null && p.Manager.Contains(searchValue))
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

            // Paging
            var data = await query.Skip(start).Take(length).ToListAsync();

            // Format dates for DataTables
            var formattedData = data.Select(p => new {
                p.Id,
                p.ProjectName,
                p.Description,
                startDate = p.StartDate.ToString("yyyy-MM-dd"),
                endDate = p.EndDate.ToString("yyyy-MM-dd"),
                p.Budget,
                p.ClientName,
                p.Manager,
                p.Status
            });

            // DataTables response
            return Json(new {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = formattedData
            });
        }
    }
}
