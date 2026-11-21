using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMS.Models.Entities;
using IMS.Models.ViewModels;

namespace IMS.DAL.Interfaces
{
    public interface IEnrollmentDAL
    {
        Task<Enrollment?> GetByIdAsync(Guid id, Guid tenantId);
        Task<(List<Enrollment> Items, int TotalCount)> GetPagedAsync(Guid tenantId, string? searchTerm,
            Guid? academicYearId, Guid? courseId, Guid? batchId, string? status, int page, int pageSize,
            Guid? branchId = null, Guid? classId = null, Guid? sectionId = null);
        Task<bool> IsDuplicateAsync(Guid tenantId, Guid studentId, Guid academicYearId, Guid? classId, Guid? batchId, Guid? courseId, Guid? excludeId);
        Task<Guid> CreateAsync(Enrollment e);
        Task<bool> UpdateAsync(Enrollment e);
        Task<bool> DeleteAsync(Guid id, Guid tenantId);

        Task<List<StudentEnrollmentHistoryViewModel>> GetEnrollmentHistoryByStudentIdAsync(Guid studentId, Guid tenantId);
        Task<Enrollment?> GetActiveEnrollmentByStudentAsync(Guid studentId, Guid tenantId);
        Task<List<BulkPromotionStudentItemViewModel>> GetStudentsForPromotionAsync(Guid tenantId, Guid? branchId, Guid academicYearId, Guid classId, Guid? sectionId);
        Task<(bool Success, string Message, Guid? NewEnrollmentId)> PromoteSingleStudentAsync(
            Guid tenantId, Guid studentId, Guid currentAcademicYearId, Guid nextAcademicYearId,
            Guid nextClassId, Guid? nextSectionId, string? newRollNumber, string enrollmentType,
            Guid? nextFeeStructureId, string? remarks, Guid? branchId);
    }
}
