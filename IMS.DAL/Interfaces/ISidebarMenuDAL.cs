using System.Collections.Generic;
using System.Threading.Tasks;
using IMS.Models.Common.Navigation;

namespace IMS.DAL.Interfaces
{
    public interface ISidebarMenuDAL
    {
        Task EnsureTableAndSeedAsync();
        Task<List<SidebarMenuItemModel>> GetActiveMenuItemsAsync();
        Task<List<SidebarMenuItemModel>> GetAllMenuItemsAsync();
        Task<SidebarMenuItemModel?> GetByIdAsync(int id);
        Task<int> CreateAsync(SidebarMenuItemModel item);
        Task<bool> UpdateAsync(SidebarMenuItemModel item);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleStatusAsync(int id);
        Task<bool> UpdateOrderAsync(int id, int sectionOrder, int menuOrder);
    }
}
