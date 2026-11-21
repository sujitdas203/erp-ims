using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMS.Models.ViewModels;

namespace IMS.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentIndexViewModel> GetListAsync(Guid tenantId, string? searchTerm, Guid? academicYearId,
            Guid? courseId, Guid? batchId, string? status, int page, int pageSize,
            Guid? branchId = null, Guid? classId = null, Guid? sectionId = null);
        Task<EnrollmentDetailsViewModel?> GetDetailsAsync(Guid id, Guid tenantId);
        Task<EnrollmentFormViewModel?> GetForEditAsync(Guid id, Guid tenantId);
        Task<ServiceResult> CreateAsync(EnrollmentFormViewModel model, Guid tenantId);
        Task<ServiceResult> UpdateAsync(EnrollmentFormViewModel model, Guid tenantId);
        Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId);

        void PopulateDropdowns(EnrollmentFormViewModel vm);

        // Promotion & History
        Task<StudentPromotionRequestViewModel?> GetPromotionFormForStudentAsync(Guid studentId, Guid tenantId);
        Task<ServiceResult> PromoteStudentAsync(StudentPromotionRequestViewModel model, Guid tenantId);
        Task<List<StudentEnrollmentHistoryViewModel>> GetEnrollmentHistoryAsync(Guid studentId, Guid tenantId);
        Task<BulkPromotionIndexViewModel> GetBulkPromotionViewModelAsync(Guid tenantId, Guid? sourceBranchId, Guid? sourceAcademicYearId, Guid? sourceClassId, Guid? sourceSectionId);
        Task<List<BulkPromotionStudentItemViewModel>> GetStudentsForPromotionAsync(Guid tenantId, Guid? branchId, Guid academicYearId, Guid classId, Guid? sectionId);
        Task<BulkPromotionResultViewModel> ProcessBulkPromotionAsync(BulkPromotionSubmitViewModel model, Guid tenantId);
    }
}
