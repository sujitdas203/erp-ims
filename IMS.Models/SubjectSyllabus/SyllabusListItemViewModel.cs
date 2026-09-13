using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.SubjectSyllabus
{
    public class SyllabusListItemViewModel
    {
        public Guid SS_Id { get; set; }
        public string CourseName { get; set; } = "";
        public string SubjectName { get; set; } = "";
        public int UnitNumber { get; set; }
        public string UnitTitle { get; set; } = "";
        public int? TotalHours { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SyllabusIndexViewModel
    {
        public List<SyllabusListItemViewModel> Units { get; set; } = new();
        public Guid? CourseFilter { get; set; }
        public Guid? SubjectFilter { get; set; }
        public string? StatusFilter { get; set; }
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public List<SelectListItem> CourseOptions { get; set; } = new();
        public List<SelectListItem> SubjectOptions { get; set; } = new();
        public List<SelectListItem> StatusOptions { get; set; } = new();
    }

    public class SyllabusFormViewModel
    {
        public Guid? SS_Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid SubjectId { get; set; }
        public int UnitNumber { get; set; }
        public string UnitTitle { get; set; } = "";
        public string? Description { get; set; }
        public int? TotalHours { get; set; }
        public string? FileUrl { get; set; }

        public List<SelectListItem> CourseOptions { get; set; } = new();
        public List<SelectListItem> SubjectOptions { get; set; } = new();
    }
}
