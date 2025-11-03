using System.Diagnostics;
using CentTask1.Interfaces;
using CentTask1.Models;
using CentTask1.Services;
using CentTask1.ViewModels.HomeViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CentTask1.Controllers
{
    public class HomeController : Controller
    {
         private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;

        public HomeController(ITaskService taskService,IProjectService projectService)
        {
            _taskService = taskService;
            _projectService = projectService;
         }

        public IActionResult Index(DashboardViewModel dashboardViewModel)
        { 
            dashboardViewModel.TaskCount = _taskService.GetAllTasksAsync().Count();
            dashboardViewModel.TasksInProgress = _taskService.GetAllTasksAsync().Where(t=> t.Status == Enum.ProjectTaskStatus.InProgress).Count();
            dashboardViewModel.PendingTasks = _taskService.GetAllTasksAsync().Where(t => t.Status == Enum.ProjectTaskStatus.Pending).Count();

            dashboardViewModel.ProjectCount = _projectService.GetAllProjectsAsync().Result.Count();
            dashboardViewModel.CompletedProjects = _projectService.GetAllProjectsAsync().Result.Where(p => p.Status == Enum.ProjectStatus.Completed).Count();
            dashboardViewModel.PendingProjects = _projectService.GetAllProjectsAsync().Result.Where(p => p.Status == Enum.ProjectStatus.Pending).Count();

            return PartialView("_Index",dashboardViewModel);
        }

        public IActionResult MainIndex()
        {
            var dashboardViewModel = new DashboardViewModel
            {
                TaskCount = _taskService.GetAllTasksAsync().Count(),
                TasksInProgress = _taskService.GetAllTasksAsync()
                                    .Where(t => t.Status == Enum.ProjectTaskStatus.InProgress).Count(),
                PendingTasks = _taskService.GetAllTasksAsync().Where(t => t.Status == Enum.ProjectTaskStatus.Pending).Count(),

                ProjectCount = _projectService.GetAllProjectsAsync().Result.Count(),
                CompletedProjects = _projectService.GetAllProjectsAsync().Result
                                    .Where(p => p.Status == Enum.ProjectStatus.Completed).Count(),
                PendingProjects = _projectService.GetAllProjectsAsync().Result.Where(p => p.Status == Enum.ProjectStatus.Pending).Count()
            };

            return View(dashboardViewModel); // send model to view
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
