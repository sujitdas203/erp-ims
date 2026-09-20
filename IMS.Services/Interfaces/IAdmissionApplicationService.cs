using System;
using System.Threading.Tasks;
using IMS.Models.ViewModels;

namespace IMS.Services.Interfaces
{
    public interface IAdmissionApplicationService
    {
        Task<AdmissionApplicationIndexViewModel> GetListAsync(Guid tenantId, string searchTerm, Guid? branchId,
            Guid? classId, Guid? courseId, Guid? academicYearId, string status, int page, int pageSize);
        Task<AdmissionApplicationDetailsViewModel> GetDetailsAsync(Guid id, Guid tenantId);
        Task<AdmissionApplicationDetailsViewModel> GetDetailsByNumberAsync(string applicationNumber, string phone = null);
        Task<AdmissionApplicationFormViewModel> GetForEditAsync(Guid id, Guid tenantId);
        Task<ServiceResult> CreateAsync(AdmissionApplicationFormViewModel model, Guid tenantId);
        Task<ServiceResult> UpdateAsync(AdmissionApplicationFormViewModel model, Guid tenantId);
        Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId);
        Task<ServiceResult> ReviewAsync(AdmissionReviewViewModel model, Guid tenantId, Guid reviewedBy);
        Task<ServiceResult> EnrollStudentAsync(AdmissionEnrollViewModel model, Guid tenantId, Guid currentUserId);
        Task<ServiceResult> UploadAttachmentAsync(Guid id, string docType, Microsoft.AspNetCore.Http.IFormFile file, Guid tenantId);

        void PopulateDropdowns(AdmissionApplicationFormViewModel vm);
    }
}
