using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMS.Models.HomeTask;

namespace IMS.DAL.Interfaces
{
    public interface IHomeTaskDAL
    {
        Task<(List<HomeTask> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, Guid? batchId, Guid? subjectId, string? status, string? search, int pageNumber, int pageSize);

        Task<HomeTaskDetailsViewModel?> GetDetailsByIdAsync(Guid id, Guid tenantId);

        Task<HomeTask?> GetByIdAsync(Guid id, Guid tenantId);

        Task<Guid> CreateAsync(HomeTask task);

        Task<bool> UpdateAsync(HomeTask task);

        Task<bool> DeleteAsync(Guid id, Guid tenantId);

        Task<bool> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive);
    }
}
