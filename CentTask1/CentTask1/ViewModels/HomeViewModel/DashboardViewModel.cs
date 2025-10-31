namespace CentTask1.ViewModels.HomeViewModel
{
    public class DashboardViewModel
    {
        public int ProjectCount { get; set; }
        public int ProjectsInProgress { get; set; }
        public int CompletedProjects { get; set; }
        public int PendingProjects { get; set; }

        //tasks
        public int TaskCount { get; set; }
        public int TasksInProgress { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }

    }
}
