using System;

namespace IMS.Models.Entities
{
    public class Enrollment
    {
        public Guid E_Id { get; set; }
        public Guid E_TenantId { get; set; }
        public Guid? E_BranchId { get; set; }
        public Guid E_StudentId { get; set; }
        public Guid E_AcademicYearId { get; set; }
        public Guid? E_ClassId { get; set; }
        public Guid? E_SectionId { get; set; }
        public Guid? E_CourseId { get; set; }
        public Guid? E_BatchId { get; set; }
        public string? E_EnrollmentNumber { get; set; }
        public string? E_RollNumber { get; set; }
        public DateTime E_EnrollmentDate { get; set; }
        public string? E_EnrollmentType { get; set; }
        public string E_Status { get; set; } = "Active";
        public DateTime? E_CompletionDate { get; set; }
        public Guid? E_FeeStructureId { get; set; }
        public string? E_Remarks { get; set; }
        public DateTime E_CreatedAt { get; set; }
        public DateTime E_UpdatedAt { get; set; }

        // Join / display fields
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
    }
}
