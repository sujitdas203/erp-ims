using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMS.Models.Entities;

namespace IMS.DAL.Interfaces
{
    /// <summary>
    /// One guardian entry as understood by the repository's composite
    /// create/update methods - either "link this existing guardian" or
    /// "create a new guardian row, then link it".
    /// </summary>
    public class GuardianLinkInput
    {
        public Guid? ExistingGuardianId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string Relation { get; set; }
        public bool IsPrimary { get; set; }
    }

    public interface IStudentDAL
    {
        Task<Student> GetByIdAsync(Guid id, Guid tenantId);

        Task<(List<Student> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, string searchTerm, string status, Guid? branchId, Guid? classId, int pageNumber, int pageSize);

        Task<bool> IsAdmissionNumberTakenAsync(Guid tenantId, string admissionNumber, Guid? excludeId);
        Task<bool> IsStudentCodeTakenAsync(Guid tenantId, string studentCode, Guid? excludeId);

        Task<List<StudentGuardianRecord>> GetGuardiansByStudentIdAsync(Guid studentId);

        /// <summary>Inserts the student row + all guardian rows/links in a single transaction.</summary>
        Task<Guid> CreateStudentWithGuardiansAsync(Student student, List<GuardianLinkInput> guardians);

        /// <summary>Updates the student row and replaces its guardian links in a single transaction.</summary>
        Task<bool> UpdateStudentWithGuardiansAsync(Student student, List<GuardianLinkInput> guardians);

        /// <summary>Updates only student entity fields directly (in-place or quick edit).</summary>
        Task<bool> QuickUpdateAsync(Student student);

        Task<(decimal TotalInvoiced, decimal TotalPaid, decimal BalanceAmount, List<IMS.Models.ViewModels.StudentInvoiceSummaryViewModel> RecentInvoices)> GetFeeSummaryAsync(Guid studentId, Guid tenantId);

        Task<(int TotalSessions, int PresentSessions, int AbsentSessions, double Percentage)> GetAttendanceSummaryAsync(Guid studentId, Guid tenantId);

        Task<(int TotalLeaves, List<IMS.Models.ViewModels.StudentLeaveSummaryViewModel> RecentLeaves)> GetLeaveSummaryAsync(Guid studentId, Guid tenantId);

        Task<List<IMS.Models.ViewModels.StudentEnrollmentHistoryViewModel>> GetEnrollmentHistoryAsync(Guid studentId, Guid tenantId);

        Task<bool> SoftDeleteAsync(Guid id, Guid tenantId);
    }
}
