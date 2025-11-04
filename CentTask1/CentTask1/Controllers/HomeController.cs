using System.Diagnostics;
using CentTask1.Enum;
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
            dashboardViewModel.CompletedTasks = _taskService.GetAllTasksAsync().Where(t => t.Status == ProjectTaskStatus.Completed).Count();
            dashboardViewModel.TasksInProgress = _taskService.GetAllTasksAsync().Where(t => t.Status == ProjectTaskStatus.NotStarted || t.Status == ProjectTaskStatus.InReview).Count();
            dashboardViewModel.PendingTasks = _taskService.GetAllTasksAsync().Where(t => t.Status == ProjectTaskStatus.NotStarted || t.Status == ProjectTaskStatus.Blocked).Count();


            dashboardViewModel.ProjectCount = _projectService.GetAllProjectsAsync().Result.Count();
            dashboardViewModel.CompletedProjects = _projectService.GetAllProjectsAsync().Result.Where(p => p.Status == Enum.ProjectStatus.Completed).Count();
            dashboardViewModel.PendingProjects = _projectService.GetAllProjectsAsync().Result.Where(p => p.Status == Enum.ProjectStatus.Pending).Count();
;           dashboardViewModel.ProjectsInProgress = _projectService.GetAllProjectsAsync().Result.Where(p => p.Status == Enum.ProjectStatus.InProgress || p.Status == ProjectStatus.OnHold).Count();

            return PartialView("_Index",dashboardViewModel);
        }

        public IActionResult MainIndex()
        {
            var dashboardViewModel = new DashboardViewModel
            {
            /* Total */        TaskCount = _taskService.GetAllTasksAsync().Count(),

            /* active */           TasksInProgress = _taskService.GetAllTasksAsync()
                                                      .Where(t => t.Status == ProjectTaskStatus.NotStarted || t.Status == ProjectTaskStatus.InReview).Count(),

            /* pending */          PendingTasks = _taskService.GetAllTasksAsync()
                                                    .Where(t => t.Status == ProjectTaskStatus.NotStarted || t.Status == ProjectTaskStatus.Blocked).Count(),

            /* completed */          CompletedTasks = _taskService.GetAllTasksAsync()
                                                        .Where(t => t.Status == ProjectTaskStatus.Completed).Count(),

                //projects status

                /* Total */     ProjectCount = _projectService.GetAllProjectsAsync().Result.Count(),

              /* completed */    CompletedProjects = _projectService.GetAllProjectsAsync().Result
                                                      .Where(p => p.Status == Enum.ProjectStatus.Completed).Count(),

               /* pending */   PendingProjects = _projectService.GetAllProjectsAsync().Result
                                                     .Where(p => p.Status == Enum.ProjectStatus.Pending).Count(),

               /* active */  ProjectsInProgress  = _projectService.GetAllProjectsAsync().Result
                                                      .Where(p => p.Status == Enum.ProjectStatus.InProgress || p.Status == ProjectStatus.OnHold).Count()
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
