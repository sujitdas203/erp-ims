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
}
