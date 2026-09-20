using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Models.HomeTask
{
    public class HomeTask
    {
        public Guid HT_Id { get; set; }
        public Guid HT_TenantId { get; set; }
        public Guid HT_BatchId { get; set; }
        public string? BatchName { get; set; }
        public Guid HT_SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public Guid? HT_TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string HT_Title { get; set; } = string.Empty;
        public string HT_Description { get; set; } = string.Empty;
        public DateTime HT_AssignedDate { get; set; } = DateTime.Today;
        public DateTime HT_DueDate { get; set; } = DateTime.Today.AddDays(1);
        public string? HT_AttachmentUrl { get; set; }
        public decimal? HT_MaxMarks { get; set; }
        public string HT_Status { get; set; } = "Active"; // Active, Closed
        public bool HT_IsActive { get; set; } = true;
        public DateTime HT_CreatedAt { get; set; }
        public DateTime HT_UpdatedAt { get; set; }
        public int SubmissionCount { get; set; }
    }

    public class HomeTaskListItemViewModel
    {
        public Guid HT_Id { get; set; }
        public string BatchName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string? TeacherName { get; set; }
        public string HT_Title { get; set; } = string.Empty;
        public DateTime HT_AssignedDate { get; set; }
        public DateTime HT_DueDate { get; set; }
        public decimal? HT_MaxMarks { get; set; }
        public string HT_Status { get; set; } = "Active";
        public bool HT_IsActive { get; set; }
        public int SubmissionCount { get; set; }
    }

    public class HomeTaskIndexViewModel
    {
        public Guid? BatchFilter { get; set; }
        public Guid? SubjectFilter { get; set; }
        public string? StatusFilter { get; set; }
        public string? SearchTerm { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public List<SelectListItem> BatchOptions { get; set; } = new();
        public List<SelectListItem> SubjectOptions { get; set; } = new();
        public List<SelectListItem> StatusOptions { get; set; } = new();

        public List<HomeTaskListItemViewModel> Tasks { get; set; } = new();
    }

    public class HomeTaskFormViewModel
    {
        public Guid? HT_Id { get; set; }
        public Guid HT_BatchId { get; set; }
        public Guid HT_SubjectId { get; set; }
        public Guid? HT_TeacherId { get; set; }
        public string HT_Title { get; set; } = string.Empty;
        public string HT_Description { get; set; } = string.Empty;
        public DateTime HT_AssignedDate { get; set; } = DateTime.Today;
        public DateTime HT_DueDate { get; set; } = DateTime.Today.AddDays(2);
        public string? HT_AttachmentUrl { get; set; }
        public decimal? HT_MaxMarks { get; set; }
        public string HT_Status { get; set; } = "Active";

        public List<SelectListItem> BatchOptions { get; set; } = new();
        public List<SelectListItem> SubjectOptions { get; set; } = new();
        public List<SelectListItem> TeacherOptions { get; set; } = new();
        public List<SelectListItem> StatusOptions { get; set; } = new();
    }

    public class HomeTaskSubmissionItemViewModel
    {
        public Guid HTS_Id { get; set; }
        public Guid HTS_StudentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string AdmissionNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public DateTime SubmissionDate { get; set; }
        public string? Content { get; set; }
        public string? AttachmentUrl { get; set; }
        public decimal? MarksObtained { get; set; }
        public string? TeacherRemarks { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class HomeTaskDetailsViewModel
    {
        public HomeTask Task { get; set; } = new();
        public List<HomeTaskSubmissionItemViewModel> Submissions { get; set; } = new();
    }
}
