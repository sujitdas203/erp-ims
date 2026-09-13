using IMS.Models.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAL.Interfaces
{
    public interface IAnnouncementDAL
    {
        Task<Guid> CreateAsync(Announcement announcement);
        Task<bool> UpdateAsync(Announcement announcement);
        Task<bool> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive);
        Task<bool> DeleteAsync(Guid id, Guid tenantId);
        Task<Announcement?> GetByIdAsync(Guid id, Guid tenantId);

        Task<(List<Announcement> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, Guid? branchId, string? status, string? search, int pageNumber, int pageSize);
    }
}
