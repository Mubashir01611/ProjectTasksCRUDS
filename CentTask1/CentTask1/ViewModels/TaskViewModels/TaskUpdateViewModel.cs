﻿using System.ComponentModel.DataAnnotations;
using CentTask1.Enum;
using CentTask1.ViewModels.BaseViewModels;

namespace CentTask1.ViewModels.TaskViewModels
{
    public class TaskUpdateViewModel: BaseViewModel
    {
        //public int Id { get; set; }
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        [Display(Name = "Equipment Type")]    
        public string? EquipmentType { get; set; }
        public string? TWR { get; set; }
        public string? ProjectId { get; set; }
        [Display(Name = "Project Name")]
        public string? ProjectName { get; set; }
        [Display(Name = "Task Status")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid status.")]
        [Required] public ProjectTaskStatus Status { get; set; }
    }
}
