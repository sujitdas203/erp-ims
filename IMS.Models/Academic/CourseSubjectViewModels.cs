using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Models.ViewModels
{
    public class CourseSubjectListItemViewModel
    {
        public Guid CS_CourseId { get; set; }
        public Guid CS_SubjectId { get; set; }
        public string CourseName { get; set; } = "-";
        public string SubjectName { get; set; } = "-";
        public int CS_SequenceNo { get; set; }
        public bool CS_IsMandatory { get; set; }
        public decimal? CS_MaxMarks { get; set; }
        public decimal? CS_PassMarks { get; set; }
    }

    public class CourseSubjectIndexViewModel
    {
        public List<CourseSubjectListItemViewModel> Items { get; set; } = new();
        public Guid? CourseFilter { get; set; }
        public List<SelectListItem> CourseOptions { get; set; } = new();
    }

    public class CourseSubjectFormViewModel
    {
        [Required(ErrorMessage = "Please select a Course.")]
        [Display(Name = "Course / Class")]
        public Guid CS_CourseId { get; set; }

        [Required(ErrorMessage = "Please select a Subject.")]
        [Display(Name = "Subject")]
        public Guid CS_SubjectId { get; set; }

        [Display(Name = "Sequence Number")]
        [Range(1, 9999, ErrorMessage = "Sequence number must be at least 1.")]
        public int CS_SequenceNo { get; set; }

        [Display(Name = "Is Mandatory")]
        public bool CS_IsMandatory { get; set; } = true;

        [Display(Name = "Maximum Marks")]
        [Range(0, 10000, ErrorMessage = "Max Marks must be between 0 and 10,000.")]
        public decimal? CS_MaxMarks { get; set; }

        [Display(Name = "Passing Marks")]
        [Range(0, 10000, ErrorMessage = "Pass Marks must be between 0 and 10,000.")]
        public decimal? CS_PassMarks { get; set; }

        public List<SelectListItem> CourseOptions { get; set; } = new();
        public List<SelectListItem> SubjectOptions { get; set; } = new();
    }
}
