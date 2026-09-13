using System;
using System.Threading.Tasks;
using IMS.Models.MockTest;

namespace IMS.Services.Interfaces
{
    public interface IMockTestService
    {
        Task<MockTestIndexViewModel> GetListAsync(
            Guid tenantId, Guid? batchId, Guid? subjectId, string? status, string? search, int page, int pageSize);

        Task<MockTestFormViewModel> GetEmptyFormAsync(Guid tenantId);

        Task<MockTestFormViewModel?> GetForEditAsync(Guid id, Guid tenantId);

        Task<MockTestDetailsViewModel?> GetDetailsAsync(Guid id, Guid tenantId);

        Task<ServiceResult> CreateAsync(MockTestFormViewModel model, Guid tenantId);

        Task<ServiceResult> UpdateAsync(MockTestFormViewModel model, Guid tenantId);

        Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId);

        Task<ServiceResult> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive);

        void PopulateDropdowns(MockTestFormViewModel vm, Guid tenantId);
    }
}
