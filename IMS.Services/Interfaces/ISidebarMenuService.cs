using System.Collections.Generic;
using System.Threading.Tasks;
using IMS.Models.Common.Navigation;

namespace IMS.Services.Interfaces
{
    public interface ISidebarMenuService
    {
        Task<IReadOnlyList<NavSectionConfig>> GetSidebarMenuAsync();
        Task<List<SidebarMenuItemModel>> GetAllForAdminAsync();
        Task<SidebarMenuItemModel?> GetByIdAsync(int id);
        Task<int> CreateAsync(SidebarMenuItemModel item);
        Task<bool> UpdateAsync(SidebarMenuItemModel item);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleStatusAsync(int id);
        Task<bool> UpdateOrderAsync(int id, int sectionOrder, int menuOrder);
        void InvalidateCache();
    }
}
