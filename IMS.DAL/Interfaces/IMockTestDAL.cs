using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMS.Models.MockTest;

namespace IMS.DAL.Interfaces
{
    public interface IMockTestDAL
    {
        Task<(List<MockTest> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, Guid? batchId, Guid? subjectId, string? status, string? search, int pageNumber, int pageSize);

        Task<MockTestDetailsViewModel?> GetDetailsByIdAsync(Guid id, Guid tenantId);

        Task<MockTest?> GetByIdAsync(Guid id, Guid tenantId);

        Task<Guid> CreateAsync(MockTest test);

        Task<bool> UpdateAsync(MockTest test);

        Task<bool> DeleteAsync(Guid id, Guid tenantId);

        Task<bool> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive);
    }
}
