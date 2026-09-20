using System;

namespace IMS.Models.Entities
{
    public class AdmissionApplication
    {
        public Guid AA_Id { get; set; }
        public Guid AA_TenantId { get; set; }
        public Guid AA_BranchId { get; set; }
        public string? AA_ApplicationNumber { get; set; }
        
        // Student Personal Details
        public string AA_FirstName { get; set; } = string.Empty;
        public string? AA_MiddleName { get; set; }
        public string AA_LastName { get; set; } = string.Empty;
        public DateTime? AA_DateOfBirth { get; set; }
        public string? AA_Gender { get; set; }
        public string? AA_BloodGroup { get; set; }
        public string? AA_Nationality { get; set; }
        public string? AA_Category { get; set; }
        public string? AA_Religion { get; set; }
        public string? AA_AadhaarNumber { get; set; }
        public string? AA_EmergencyContactName { get; set; }
        public string? AA_EmergencyContactPhone { get; set; }
        public string? AA_Email { get; set; }
        public string AA_Phone { get; set; } = string.Empty;

        // Parent / Guardian Details
        public string? AA_FatherName { get; set; }
        public string? AA_FatherPhone { get; set; }
        public string? AA_FatherOccupation { get; set; }
        public string? AA_FatherEmail { get; set; }

        public string? AA_MotherName { get; set; }
        public string? AA_MotherPhone { get; set; }
        public string? AA_MotherOccupation { get; set; }
        public string? AA_MotherEmail { get; set; }

        public string? AA_GuardianName { get; set; }
        public string? AA_GuardianPhone { get; set; }
        public string? AA_GuardianRelation { get; set; }
        public string? AA_GuardianEmail { get; set; }
        public string? AA_GuardianOccupation { get; set; }

        // Address Details
        public string? AA_AddressLine1 { get; set; }
        public string? AA_AddressLine2 { get; set; }
        public string? AA_City { get; set; }
        public string? AA_State { get; set; }
        public string? AA_PostalCode { get; set; }
        public string? AA_Country { get; set; }

        // Previous Academic Record
        public string? AA_PreviousSchool { get; set; }
        public string? AA_PreviousBoard { get; set; }
        public string? AA_PreviousGrade { get; set; }
        public string? AA_PreviousMarks { get; set; }
        public string? AA_TransferCertificateNumber { get; set; }

        // Academic Program Mapping
        public Guid? AA_ClassId { get; set; }
        public Guid? AA_CourseId { get; set; }
        public Guid AA_AcademicYearId { get; set; }

        // Application Workflow & Status
        public string? AA_Status { get; set; }
        public DateTime? AA_SubmittedAt { get; set; }
        public DateTime? AA_ReviewedAt { get; set; }
        public Guid? AA_ReviewedBy { get; set; }
        public string? AA_Notes { get; set; }

        // Document & Media Attachments
        public string? AA_StudentPhotoUrl { get; set; }
        public string? AA_BirthCertificateUrl { get; set; }
        public string? AA_TransferCertificateUrl { get; set; }
        public string? AA_MarksheetUrl { get; set; }
        public string? AA_NationalIdDocUrl { get; set; }

        // Health & Medical
        public string? AA_MedicalConditions { get; set; }

        // Transport & Facility Preferences
        public bool AA_RequiresTransport { get; set; }
        public string? AA_TransportPickupPoint { get; set; }
        public bool AA_RequiresHostel { get; set; }

        // Language & Sibling Preferences
        public string? AA_SecondLanguage { get; set; }
        public string? AA_MotherTongue { get; set; }
        public bool AA_HasSibling { get; set; }
        public string? AA_SiblingDetails { get; set; }

        // Financial & Conversion Bridge
        public Guid? AA_FeeStructureId { get; set; }
        public Guid? AA_AdmittedStudentId { get; set; }
        public Guid? AA_EnrollmentId { get; set; }

        public DateTime AA_CreatedAt { get; set; }
        public DateTime AA_UpdatedAt { get; set; }

        // Joined Display Properties
        public string? CourseName { get; set; }
        public string? AcademicYearName { get; set; }
        public string? ClassName { get; set; }
        public string? BranchName { get; set; }
        public string? AdmittedStudentCode { get; set; }
        public string? AdmittedStudentAdmissionNumber { get; set; }
    }
}
