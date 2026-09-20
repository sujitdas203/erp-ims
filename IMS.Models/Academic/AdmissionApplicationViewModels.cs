using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Models.ViewModels
{
    public class AdmissionApplicationListItemViewModel
    {
        public Guid AA_Id { get; set; }
        public string AA_ApplicationNumber { get; set; }
        public string FullName => string.Join(" ", new[] { AA_FirstName, AA_MiddleName, AA_LastName }.Where(p => !string.IsNullOrWhiteSpace(p)));
        public string AA_FirstName { get; set; }
        public string AA_MiddleName { get; set; }
        public string AA_LastName { get; set; }
        public string AA_Gender { get; set; }
        public string AA_Phone { get; set; }
        public string AA_Email { get; set; }
        public string BranchName { get; set; }
        public string ClassName { get; set; }
        public string CourseName { get; set; }
        public string AcademicYearName { get; set; }
        public string ParentName => !string.IsNullOrWhiteSpace(AA_FatherName) ? AA_FatherName : (!string.IsNullOrWhiteSpace(AA_GuardianName) ? AA_GuardianName : AA_MotherName);
        public string AA_FatherName { get; set; }
        public string AA_MotherName { get; set; }
        public string AA_GuardianName { get; set; }
        public string? AA_StudentPhotoUrl { get; set; }
        public bool AA_RequiresTransport { get; set; }
        public bool AA_RequiresHostel { get; set; }
        public DateTime? AA_SubmittedAt { get; set; }
        public string AA_Status { get; set; }
        public Guid? AA_AdmittedStudentId { get; set; }
        public string? AdmittedStudentCode { get; set; }
    }

    public class AdmissionApplicationIndexViewModel
    {
        public List<AdmissionApplicationListItemViewModel> Applications { get; set; } = new();
        public string SearchTerm { get; set; }
        public Guid? BranchFilter { get; set; }
        public Guid? ClassFilter { get; set; }
        public Guid? CourseFilter { get; set; }
        public Guid? AcademicYearFilter { get; set; }
        public string StatusFilter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public List<SelectListItem> BranchOptions { get; set; } = new();
        public List<SelectListItem> ClassOptions { get; set; } = new();
        public List<SelectListItem> CourseOptions { get; set; } = new();
        public List<SelectListItem> AcademicYearOptions { get; set; } = new();
        public List<SelectListItem> StatusOptions { get; set; } = new();
    }

    public class AdmissionApplicationFormViewModel
    {
        public Guid? AA_Id { get; set; }
        [Required(ErrorMessage = "Branch / Campus is required.")]
        [Display(Name = "Branch / Campus")]
        public Guid? AA_BranchId { get; set; }
        public string? AA_ApplicationNumber { get; set; }

        // Student Personal Details
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public string AA_FirstName { get; set; } = string.Empty;

        [Display(Name = "Middle Name")]
        public string? AA_MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public string AA_LastName { get; set; } = string.Empty;

        [Display(Name = "Date of Birth")]
        public DateTime? AA_DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        public string? AA_Gender { get; set; }

        [Display(Name = "Blood Group")]
        public string? AA_BloodGroup { get; set; }

        [Display(Name = "Nationality")]
        public string? AA_Nationality { get; set; } = "Indian";

        [Display(Name = "Category")]
        public string? AA_Category { get; set; }

        [Display(Name = "Religion")]
        public string? AA_Religion { get; set; }

        [Display(Name = "Aadhaar / National ID")]
        public string? AA_AadhaarNumber { get; set; }

        [Display(Name = "Emergency Contact Name")]
        public string? AA_EmergencyContactName { get; set; }

        [Display(Name = "Emergency Contact Mobile")]
        public string? AA_EmergencyContactPhone { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Student Email")]
        public string? AA_Email { get; set; }

        [Required(ErrorMessage = "Student phone / mobile number is required.")]
        [Display(Name = "Student Mobile")]
        public string AA_Phone { get; set; } = string.Empty;

        // Parent / Guardian Details
        [Display(Name = "Father's Full Name")]
        public string? AA_FatherName { get; set; }

        [Display(Name = "Father's Mobile")]
        public string? AA_FatherPhone { get; set; }

        [Display(Name = "Father's Occupation")]
        public string? AA_FatherOccupation { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address for father.")]
        [Display(Name = "Father's Email")]
        public string? AA_FatherEmail { get; set; }

        [Display(Name = "Mother's Full Name")]
        public string? AA_MotherName { get; set; }

        [Display(Name = "Mother's Mobile")]
        public string? AA_MotherPhone { get; set; }

        [Display(Name = "Mother's Occupation")]
        public string? AA_MotherOccupation { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address for mother.")]
        [Display(Name = "Mother's Email")]
        public string? AA_MotherEmail { get; set; }

        [Display(Name = "Guardian Name")]
        public string? AA_GuardianName { get; set; }

        [Display(Name = "Guardian Mobile")]
        public string? AA_GuardianPhone { get; set; }

        [Display(Name = "Guardian Relation")]
        public string? AA_GuardianRelation { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address for guardian.")]
        [Display(Name = "Guardian Email")]
        public string? AA_GuardianEmail { get; set; }

        [Display(Name = "Guardian Occupation")]
        public string? AA_GuardianOccupation { get; set; }

        // Address Details
        [Display(Name = "Address Line 1")]
        public string? AA_AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string? AA_AddressLine2 { get; set; }

        [Display(Name = "City")]
        public string? AA_City { get; set; }

        [Display(Name = "State")]
        public string? AA_State { get; set; }

        [Display(Name = "Postal Code")]
        public string? AA_PostalCode { get; set; }

        [Display(Name = "Country")]
        public string? AA_Country { get; set; } = "India";

        // Previous Academic Record
        [Display(Name = "Previous School")]
        public string? AA_PreviousSchool { get; set; }

        [Display(Name = "Previous Board")]
        public string? AA_PreviousBoard { get; set; }

        [Display(Name = "Previous Grade")]
        public string? AA_PreviousGrade { get; set; }

        [Display(Name = "Previous Marks / %")]
        public string? AA_PreviousMarks { get; set; }

        [Display(Name = "Transfer Certificate Number")]
        public string? AA_TransferCertificateNumber { get; set; }

        // Academic Program Mapping
        [Display(Name = "Class / Grade")]
        public Guid? AA_ClassId { get; set; }

        [Display(Name = "Course / Stream")]
        public Guid? AA_CourseId { get; set; }

        [Required(ErrorMessage = "Academic Year is required.")]
        [Display(Name = "Academic Year")]
        public Guid? AA_AcademicYearId { get; set; }

        // Document & Media URLs
        public string? AA_StudentPhotoUrl { get; set; }
        public string? AA_BirthCertificateUrl { get; set; }
        public string? AA_TransferCertificateUrl { get; set; }
        public string? AA_MarksheetUrl { get; set; }
        public string? AA_NationalIdDocUrl { get; set; }

        // IFormFile upload properties
        [Display(Name = "Student Photograph (Passport Size)")]
        public IFormFile? StudentPhoto { get; set; }

        [Display(Name = "Birth Certificate Document")]
        public IFormFile? BirthCertificateDoc { get; set; }

        [Display(Name = "Transfer Certificate Document")]
        public IFormFile? TransferCertificateDoc { get; set; }

        [Display(Name = "Previous Marksheet / Grade Sheet")]
        public IFormFile? MarksheetDoc { get; set; }

        [Display(Name = "Aadhaar / National ID Card Scan")]
        public IFormFile? NationalIdDoc { get; set; }

        // Health & Medical
        [Display(Name = "Medical Conditions / Allergies / Special Needs")]
        public string? AA_MedicalConditions { get; set; }

        // Facilities & Transport
        [Display(Name = "Requires School Transport")]
        public bool AA_RequiresTransport { get; set; }

        [Display(Name = "Preferred Pickup Stop / Route")]
        public string? AA_TransportPickupPoint { get; set; }

        [Display(Name = "Requires Hostel Facility")]
        public bool AA_RequiresHostel { get; set; }

        // Language & Sibling Preferences
        [Display(Name = "Second Language Preference")]
        public string? AA_SecondLanguage { get; set; }

        [Display(Name = "Mother Tongue")]
        public string? AA_MotherTongue { get; set; }

        [Display(Name = "Has Sibling in this Institute?")]
        public bool AA_HasSibling { get; set; }

        [Display(Name = "Sibling Details (Name, Class, Roll No.)")]
        public string? AA_SiblingDetails { get; set; }

        // Financial & Fee Structure
        [Display(Name = "Selected Fee Structure")]
        public Guid? AA_FeeStructureId { get; set; }

        // Workflow & Notes
        [Display(Name = "Status")]
        public string? AA_Status { get; set; } = "Submitted";

        [Display(Name = "Internal Notes / Remarks")]
        public string? AA_Notes { get; set; }

        // Dropdown options
        public List<SelectListItem> BranchOptions { get; set; } = new();
        public List<SelectListItem> ClassOptions { get; set; } = new();
        public List<SelectListItem> CourseOptions { get; set; } = new();
        public List<SelectListItem> AcademicYearOptions { get; set; } = new();
        public List<SelectListItem> FeeStructureOptions { get; set; } = new();
        public List<SelectListItem> GenderOptions { get; set; } = new();
        public List<SelectListItem> BloodGroupOptions { get; set; } = new();
        public List<SelectListItem> CategoryOptions { get; set; } = new();
        public List<SelectListItem> GuardianRelationOptions { get; set; } = new();
        public List<SelectListItem> StatusOptions { get; set; } = new();
    }

    public class AdmissionApplicationDetailsViewModel
    {
        public Guid AA_Id { get; set; }
        public Guid AA_TenantId { get; set; }
        public Guid AA_BranchId { get; set; }
        public string AA_ApplicationNumber { get; set; }

        // Student Personal Details
        public string FullName => string.Join(" ", new[] { AA_FirstName, AA_MiddleName, AA_LastName }.Where(p => !string.IsNullOrWhiteSpace(p)));
        public string AA_FirstName { get; set; }
        public string AA_MiddleName { get; set; }
        public string AA_LastName { get; set; }
        public DateTime? AA_DateOfBirth { get; set; }
        public string AA_Gender { get; set; }
        public string AA_BloodGroup { get; set; }
        public string AA_Nationality { get; set; }
        public string AA_Category { get; set; }
        public string AA_Religion { get; set; }
        public string AA_AadhaarNumber { get; set; }
        public string AA_EmergencyContactName { get; set; }
        public string AA_EmergencyContactPhone { get; set; }
        public string AA_Email { get; set; }
        public string AA_Phone { get; set; }

        // Parent / Guardian Details
        public string AA_FatherName { get; set; }
        public string AA_FatherPhone { get; set; }
        public string AA_FatherOccupation { get; set; }
        public string AA_FatherEmail { get; set; }

        public string AA_MotherName { get; set; }
        public string AA_MotherPhone { get; set; }
        public string AA_MotherOccupation { get; set; }
        public string AA_MotherEmail { get; set; }

        public string AA_GuardianName { get; set; }
        public string AA_GuardianPhone { get; set; }
        public string AA_GuardianRelation { get; set; }
        public string AA_GuardianEmail { get; set; }
        public string AA_GuardianOccupation { get; set; }

        // Address Details
        public string AA_AddressLine1 { get; set; }
        public string AA_AddressLine2 { get; set; }
        public string AA_City { get; set; }
        public string AA_State { get; set; }
        public string AA_PostalCode { get; set; }
        public string AA_Country { get; set; }
        public string FullAddress => string.Join(", ", new[] { AA_AddressLine1, AA_AddressLine2, AA_City, AA_State, AA_PostalCode, AA_Country }.Where(p => !string.IsNullOrWhiteSpace(p)));

        // Previous Academic Record
        public string AA_PreviousSchool { get; set; }
        public string AA_PreviousBoard { get; set; }
        public string AA_PreviousGrade { get; set; }
        public string AA_PreviousMarks { get; set; }
        public string AA_TransferCertificateNumber { get; set; }

        // Academic Program Mapping
        public Guid? AA_ClassId { get; set; }
        public Guid? AA_CourseId { get; set; }
        public Guid AA_AcademicYearId { get; set; }
        public string BranchName { get; set; }
        public string ClassName { get; set; }
        public string CourseName { get; set; }
        public string AcademicYearName { get; set; }

        // Document & Media URLs
        public string? AA_StudentPhotoUrl { get; set; }
        public string? AA_BirthCertificateUrl { get; set; }
        public string? AA_TransferCertificateUrl { get; set; }
        public string? AA_MarksheetUrl { get; set; }
        public string? AA_NationalIdDocUrl { get; set; }

        // Health & Medical
        public string? AA_MedicalConditions { get; set; }

        // Facilities & Transport
        public bool AA_RequiresTransport { get; set; }
        public string? AA_TransportPickupPoint { get; set; }
        public bool AA_RequiresHostel { get; set; }

        // Language & Sibling
        public string? AA_SecondLanguage { get; set; }
        public string? AA_MotherTongue { get; set; }
        public bool AA_HasSibling { get; set; }
        public string? AA_SiblingDetails { get; set; }

        // Financial & Conversion Bridge
        public Guid? AA_FeeStructureId { get; set; }
        public string? FeeStructureName { get; set; }
        public Guid? AA_AdmittedStudentId { get; set; }
        public string? AdmittedStudentCode { get; set; }
        public string? AdmittedStudentAdmissionNumber { get; set; }
        public Guid? AA_EnrollmentId { get; set; }

        // Application Workflow & Status
        public string AA_Status { get; set; }
        public DateTime? AA_SubmittedAt { get; set; }
        public DateTime? AA_ReviewedAt { get; set; }
        public Guid? AA_ReviewedBy { get; set; }
        public string AA_Notes { get; set; }
        public DateTime AA_CreatedAt { get; set; }
        public DateTime AA_UpdatedAt { get; set; }

        // Modal options for Quick Enroll
        public List<SelectListItem> ClassOptions { get; set; } = new();
        public List<SelectListItem> SectionOptions { get; set; } = new();
        public List<SelectListItem> BatchOptions { get; set; } = new();
        public List<SelectListItem> FeeStructureOptions { get; set; } = new();
    }

    public class AdmissionReviewViewModel
    {
        public Guid AA_Id { get; set; }
        public string AA_Status { get; set; }
        public string AA_Notes { get; set; }
    }

    public class AdmissionEnrollViewModel
    {
        public Guid AA_Id { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? AcademicYearId { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? SectionId { get; set; }
        public Guid? BatchId { get; set; }
        public string? AdmissionNumber { get; set; }
        public string? RollNumber { get; set; }
        public DateTime? AdmissionDate { get; set; } = DateTime.Today;
        public Guid? FeeStructureId { get; set; }
    }
}
