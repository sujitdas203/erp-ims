using IMS.Models.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services.Interfaces
{
    public interface IAnnouncementService
    {
        Task<AnnouncementIndexViewModel> GetListAsync(
            Guid tenantId, Guid? branchId, string? status, string? search, int page, int pageSize);

        Task<AnnouncementFormViewModel?> GetForEditAsync(Guid id, Guid tenantId);

        Task<ServiceResult> CreateAsync(AnnouncementFormViewModel model, Guid tenantId, Guid createdBy);
        Task<ServiceResult> UpdateAsync(AnnouncementFormViewModel model, Guid tenantId);
        Task<ServiceResult> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive);
        Task<ServiceResult> DeleteAsync(Guid id, Guid tenantId);
    }
}
