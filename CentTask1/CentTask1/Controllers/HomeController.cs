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
        private readonly ILogger<HomeController> _logger;
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;

        public HomeController(ILogger<HomeController> logger,ITaskService taskService,IProjectService projectService)
        {
            _taskService = taskService;
            _projectService = projectService;
            _logger = logger;
        }

        public IActionResult Index(DashboardViewModel dashboardViewModel)
        {
          //  dashboardViewModel.ProjectCount = _projectService.GetAllProjectsAsync();
            dashboardViewModel.TaskCount = _taskService.GetAllTasksAsync().Count();
            dashboardViewModel.ProjectCount = _projectService.GetAllProjectsAsync().Result.Count();
            dashboardViewModel.InProgressTask = _taskService.GetAllTasksAsync().Where(t=> t.Status == Enum.ProjectTaskStatus.InProgress).Count();
            dashboardViewModel.CompletedProjects = _projectService.GetAllProjectsAsync().Result.Where(p => p.Status == Enum.ProjectStatus.Completed).Count();
            return PartialView("_Index",dashboardViewModel);
        }

        public IActionResult MainIndex()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
