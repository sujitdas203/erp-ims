using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Models.ViewModels
{
    public class EnrollmentListItemViewModel
    {
        public Guid E_Id { get; set; }
        public string? E_EnrollmentNumber { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public string? BranchName { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? CourseName { get; set; }
        public string? BatchName { get; set; }
        public string? AcademicYearName { get; set; }
        public string? E_RollNumber { get; set; }
        public string? E_EnrollmentType { get; set; }
        public DateTime E_EnrollmentDate { get; set; }
        public string E_Status { get; set; } = "Active";
    }

    public class EnrollmentIndexViewModel
    {
        public List<EnrollmentListItemViewModel> Enrollments { get; set; } = new();
        public string? SearchTerm { get; set; }
        public Guid? AcademicYearFilter { get; set; }
        public Guid? BranchFilter { get; set; }
        public Guid? ClassFilter { get; set; }
        public Guid? SectionFilter { get; set; }
        public Guid? CourseFilter { get; set; }
        public Guid? BatchFilter { get; set; }
        public string? StatusFilter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public List<SelectListItem> AcademicYearOptions { get; set; } = new();
        public List<SelectListItem> BranchOptions { get; set; } = new();
        public List<SelectListItem> ClassOptions { get; set; } = new();
        public List<SelectListItem> SectionOptions { get; set; } = new();
        public List<SelectListItem> CourseOptions { get; set; } = new();
        public List<SelectListItem> BatchOptions { get; set; } = new();
        public List<SelectListItem> StatusOptions { get; set; } = new();
    }

    public class EnrollmentFormViewModel
    {
        public Guid? E_Id { get; set; }

        [Display(Name = "Branch / Campus")]
        public Guid? E_BranchId { get; set; }

        [Required(ErrorMessage = "Student is required.")]
        [Display(Name = "Student")]
        public Guid E_StudentId { get; set; }

        [Required(ErrorMessage = "Academic Year is required.")]
        [Display(Name = "Academic Year")]
        public Guid E_AcademicYearId { get; set; }

        [Display(Name = "Class / Grade (Nursery - 12th)")]
        public Guid? E_ClassId { get; set; }

        [Display(Name = "Section")]
        public Guid? E_SectionId { get; set; }

        [Display(Name = "Course / Stream / Program")]
        public Guid? E_CourseId { get; set; }

        [Display(Name = "Batch")]
        public Guid? E_BatchId { get; set; }

        [Display(Name = "Enrollment Number")]
        public string? E_EnrollmentNumber { get; set; }

        [Display(Name = "Roll Number")]
        public string? E_RollNumber { get; set; }

        [Required(ErrorMessage = "Enrollment Type is required.")]
        [Display(Name = "Enrollment Type")]
        public string E_EnrollmentType { get; set; } = "New Admission";

        [Required(ErrorMessage = "Enrollment Date is required.")]
        [Display(Name = "Enrollment Date")]
        public DateTime E_EnrollmentDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Status is required.")]
        [Display(Name = "Status")]
        public string E_Status { get; set; } = "Active";

        [Display(Name = "Completion Date")]
        public DateTime? E_CompletionDate { get; set; }

        [Display(Name = "Fee Structure")]
        public Guid? E_FeeStructureId { get; set; }

        [Display(Name = "Remarks / Notes")]
        public string? E_Remarks { get; set; }

        // Dropdown collections
        public List<SelectListItem> StudentOptions { get; set; } = new();
        public List<SelectListItem> BranchOptions { get; set; } = new();
        public List<SelectListItem> AcademicYearOptions { get; set; } = new();
        public List<SelectListItem> ClassOptions { get; set; } = new();
        public List<SelectListItem> SectionOptions { get; set; } = new();
        public List<SelectListItem> CourseOptions { get; set; } = new();
        public List<SelectListItem> BatchOptions { get; set; } = new();
        public List<SelectListItem> EnrollmentTypeOptions { get; set; } = new();
        public List<SelectListItem> StatusOptions { get; set; } = new();
        public List<SelectListItem> FeeStructureOptions { get; set; } = new();
    }

    public class EnrollmentDetailsViewModel
    {
        public Guid E_Id { get; set; }
        public string? E_EnrollmentNumber { get; set; }
        public string? StudentName { get; set; }
        public string? StudentAdmissionNumber { get; set; }
        public string? StudentCode { get; set; }
        public string? StudentPhone { get; set; }
        public string? StudentEmail { get; set; }
        public string? BranchName { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? CourseName { get; set; }
        public string? BatchName { get; set; }
        public string? AcademicYearName { get; set; }
        public string? E_RollNumber { get; set; }
        public string? E_EnrollmentType { get; set; }
        public DateTime E_EnrollmentDate { get; set; }
        public string E_Status { get; set; } = "Active";
        public DateTime? E_CompletionDate { get; set; }
        public string? E_Remarks { get; set; }
        public DateTime E_CreatedAt { get; set; }
        public DateTime E_UpdatedAt { get; set; }
    }

    public class StudentPromotionRequestViewModel
    {
        [Required(ErrorMessage = "Student is required.")]
        public Guid StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public string? AdmissionNumber { get; set; }

        public Guid? CurrentBranchId { get; set; }
        public string? CurrentBranchName { get; set; }

        public Guid? CurrentAcademicYearId { get; set; }
        public string? CurrentAcademicYearName { get; set; }

        public Guid? CurrentClassId { get; set; }
        public string? CurrentClassName { get; set; }

        public Guid? CurrentSectionId { get; set; }
        public string? CurrentSectionName { get; set; }

        public string? CurrentRollNumber { get; set; }

        [Required(ErrorMessage = "Destination academic year is required.")]
        [Display(Name = "Destination Academic Year")]
        public Guid NextAcademicYearId { get; set; }

        [Required(ErrorMessage = "Destination class is required.")]
        [Display(Name = "Destination Class / Grade")]
        public Guid NextClassId { get; set; }

        [Display(Name = "Destination Section")]
        public Guid? NextSectionId { get; set; }

        [Display(Name = "New Roll Number")]
        public string? NewRollNumber { get; set; }

        [Required(ErrorMessage = "Enrollment type is required.")]
        [Display(Name = "Enrollment Type")]
        public string EnrollmentType { get; set; } = "Promotion"; // Promotion, Repeat, Transfer

        [Display(Name = "Fee Structure")]
        public Guid? NextFeeStructureId { get; set; }

        [Display(Name = "Promotion Remarks / Performance Notes")]
        public string? Remarks { get; set; }

        // Dropdowns
        public List<SelectListItem> AcademicYearOptions { get; set; } = new();
        public List<SelectListItem> ClassOptions { get; set; } = new();
        public List<SelectListItem> SectionOptions { get; set; } = new();
        public List<SelectListItem> EnrollmentTypeOptions { get; set; } = new();
        public List<SelectListItem> FeeStructureOptions { get; set; } = new();
    }

    public class StudentEnrollmentHistoryViewModel
    {
        public Guid EnrollmentId { get; set; }
        public Guid AcademicYearId { get; set; }
        public string AcademicYearName { get; set; } = "-";
        public string? AcademicYearCode { get; set; }
        public DateTime? AcademicYearStartDate { get; set; }
        public DateTime? AcademicYearEndDate { get; set; }
        public bool IsCurrentYear { get; set; }

        public Guid? BranchId { get; set; }
        public string? BranchName { get; set; }

        public Guid? ClassId { get; set; }
        public string ClassName { get; set; } = "-";

        public Guid? SectionId { get; set; }
        public string SectionName { get; set; } = "-";

        public string? CourseName { get; set; }
        public string? BatchName { get; set; }

        public string? EnrollmentNumber { get; set; }
        public string? RollNumber { get; set; }
        public string EnrollmentType { get; set; } = "New Admission"; // Promotion, Repeat, New Admission, Transfer
        public string Status { get; set; } = "Active"; // Active, Promoted, Retained, Completed, Withdrawn, Alumni
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string? Remarks { get; set; }
    }

    public class BulkPromotionStudentItemViewModel
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; } = "";
        public string StudentCode { get; set; } = "";
        public string AdmissionNumber { get; set; } = "";
        public string? CurrentRollNumber { get; set; }
        public string CurrentStatus { get; set; } = "Active";
        public int TotalAttendanceSessions { get; set; }
        public int PresentSessions { get; set; }
        public double AttendancePercentage => TotalAttendanceSessions > 0
            ? Math.Round((double)PresentSessions / TotalAttendanceSessions * 100, 1)
            : 100.0;
        public decimal PendingFeeBalance { get; set; }
        public bool AlreadyEnrolledInNextYear { get; set; }

        // Promotion decision for this student
        public bool IsSelected { get; set; } = true;
        public string Action { get; set; } = "Promote"; // Promote, Repeat, Leave
        public Guid? TargetSectionId { get; set; }
        public string? NextRollNumber { get; set; }
    }

    public class BulkPromotionIndexViewModel
    {
        // Source Selection
        public Guid? SourceBranchId { get; set; }
        public Guid? SourceAcademicYearId { get; set; }
        public Guid? SourceClassId { get; set; }
        public Guid? SourceSectionId { get; set; }

        // Target Selection
        public Guid? TargetAcademicYearId { get; set; }
        public Guid? TargetClassId { get; set; }
        public Guid? TargetSectionId { get; set; }
        public Guid? TargetFeeStructureId { get; set; }
        public string DefaultEnrollmentType { get; set; } = "Promotion";

        // Students Grid
        public List<BulkPromotionStudentItemViewModel> Students { get; set; } = new();

        // Dropdown collections
        public List<SelectListItem> BranchOptions { get; set; } = new();
        public List<SelectListItem> AcademicYearOptions { get; set; } = new();
        public List<SelectListItem> ClassOptions { get; set; } = new();
        public List<SelectListItem> SectionOptions { get; set; } = new();
        public List<SelectListItem> FeeStructureOptions { get; set; } = new();
    }

    public class BulkPromotionSubmitViewModel
    {
        [Required(ErrorMessage = "Source Academic Year is required.")]
        public Guid SourceAcademicYearId { get; set; }

        [Required(ErrorMessage = "Target Academic Year is required.")]
        public Guid TargetAcademicYearId { get; set; }

        [Required(ErrorMessage = "Target Class is required.")]
        public Guid TargetClassId { get; set; }

        public Guid? TargetSectionId { get; set; }
        public Guid? TargetBranchId { get; set; }
        public Guid? TargetFeeStructureId { get; set; }

        public List<BulkPromotionStudentItemViewModel> Students { get; set; } = new();
    }

    public class BulkPromotionResultViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public int TotalCount { get; set; }
        public int PromotedCount { get; set; }
        public int RetainedCount { get; set; }
        public int SkippedCount { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
