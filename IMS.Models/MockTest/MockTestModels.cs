using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Models.MockTest
{
    public class MockTest
    {
        public Guid MT_Id { get; set; }
        public Guid MT_TenantId { get; set; }
        public Guid MT_BatchId { get; set; }
        public string? BatchName { get; set; }
        public Guid MT_SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public string MT_Title { get; set; } = string.Empty;
        public string? MT_Description { get; set; }
        public DateTime MT_TestDate { get; set; } = DateTime.Today.AddDays(1).AddHours(10);
        public int MT_DurationMinutes { get; set; } = 60;
        public decimal MT_TotalMarks { get; set; } = 100;
        public decimal MT_PassMarks { get; set; } = 40;
        public string MT_Status { get; set; } = "Scheduled"; // Scheduled, Ongoing, Completed, Cancelled
        public bool MT_IsActive { get; set; } = true;
        public DateTime MT_CreatedAt { get; set; }
        public int ResultCount { get; set; }
        public decimal? AvgScore { get; set; }
    }

    public class MockTestListItemViewModel
    {
        public Guid MT_Id { get; set; }
        public string BatchName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string MT_Title { get; set; } = string.Empty;
        public DateTime MT_TestDate { get; set; }
        public int MT_DurationMinutes { get; set; }
        public decimal MT_TotalMarks { get; set; }
        public decimal MT_PassMarks { get; set; }
        public string MT_Status { get; set; } = "Scheduled";
        public bool MT_IsActive { get; set; }
        public int ResultCount { get; set; }
        public decimal? AvgScore { get; set; }
    }

    public class MockTestIndexViewModel
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

        public List<MockTestListItemViewModel> Tests { get; set; } = new();
    }

    public class MockTestFormViewModel
    {
        public Guid? MT_Id { get; set; }
        public Guid MT_BatchId { get; set; }
        public Guid MT_SubjectId { get; set; }
        public string MT_Title { get; set; } = string.Empty;
        public string? MT_Description { get; set; }
        public DateTime MT_TestDate { get; set; } = DateTime.Today.AddDays(1).AddHours(10);
        public int MT_DurationMinutes { get; set; } = 60;
        public decimal MT_TotalMarks { get; set; } = 100;
        public decimal MT_PassMarks { get; set; } = 40;
        public string MT_Status { get; set; } = "Scheduled";

        public List<SelectListItem> BatchOptions { get; set; } = new();
        public List<SelectListItem> SubjectOptions { get; set; } = new();
        public List<SelectListItem> StatusOptions { get; set; } = new();
    }

    public class MockTestStudentResultViewModel
    {
        public Guid MTR_Id { get; set; }
        public Guid MTR_StudentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string AdmissionNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public decimal Percentage { get; set; }
        public string? Grade { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CompletedAt { get; set; }
    }

    public class MockTestDetailsViewModel
    {
        public MockTest Test { get; set; } = new();
        public List<MockTestStudentResultViewModel> Results { get; set; } = new();
    }
}
