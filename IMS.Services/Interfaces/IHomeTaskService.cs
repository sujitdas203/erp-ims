using System;
using System.Threading.Tasks;
using IMS.Models.HomeTask;

namespace IMS.Services.Interfaces
{
    public interface IHomeTaskService
    {
        Task<HomeTaskIndexViewModel> GetListAsync(
            Guid tenantId, Guid? batchId, Guid? subjectId, string? status, string? search, int page, int pageSize);

        Task<HomeTaskFormViewModel> GetEmptyFormAsync(Guid tenantId);

        Task<HomeTaskFormViewModel?> GetForEditAsync(Guid id, Guid tenantId);

        Task<HomeTaskDetailsViewModel?> GetDetailsAsync(Guid id, Guid tenantId);

        Task<ServiceResult> CreateAsync(HomeTaskFormViewModel model, Guid tenantId);

        Task<ServiceResult> UpdateAsync(HomeTaskFormViewModel model, Guid tenantId);

        Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId);

        Task<ServiceResult> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive);

        void PopulateDropdowns(HomeTaskFormViewModel vm, Guid tenantId);
    }
}
