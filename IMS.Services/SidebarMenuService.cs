using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using IMS.DAL.Interfaces;
using IMS.Models.Common.Navigation;
using IMS.Services.Interfaces;

namespace IMS.Services
{
    public class SidebarMenuService : ISidebarMenuService
    {
        private const string CacheKey = "IMS_SIDEBAR_NAV_MENU";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

        private readonly ISidebarMenuDAL _dal;
        private readonly IMemoryCache _cache;

        public SidebarMenuService(ISidebarMenuDAL dal, IMemoryCache cache)
        {
            _dal = dal;
            _cache = cache;
        }

        public async Task<IReadOnlyList<NavSectionConfig>> GetSidebarMenuAsync()
        {
            if (_cache.TryGetValue(CacheKey, out IReadOnlyList<NavSectionConfig>? cached) && cached != null)
            {
                return cached;
            }

            var items = await _dal.GetActiveMenuItemsAsync();

            // Group by SectionTitle and SectionOrder
            var grouped = items
                .GroupBy(i => new { i.SectionTitle, i.SectionOrder })
                .OrderBy(g => g.Key.SectionOrder)
                .Select(g => new NavSectionConfig
                {
                    Title = g.Key.SectionTitle,
                    Order = g.Key.SectionOrder,
                    Items = g.OrderBy(i => i.MenuOrder).Select(i => new NavItemConfig
                    {
                        Id = i.Id,
                        Title = i.Title,
                        Controller = i.Controller ?? "",
                        Action = i.Action ?? "Index",
                        Area = i.Area ?? "",
                        Icon = i.Icon,
                        PermissionCode = i.PermissionCode,
                        Order = i.MenuOrder,
                        BadgeText = i.BadgeText,
                        BadgeClass = i.BadgeClass,
                        CustomUrl = i.CustomUrl,
                        IsCollapsible = i.IsCollapsible,
                        CollapseTargetId = i.CollapseTargetId,
                        IsMasterManagement = i.IsMasterManagement,
                        IsActive = i.IsActive
                    }).ToList()
                })
                .ToList();

            _cache.Set(CacheKey, grouped, CacheDuration);
            return grouped;
        }

        public async Task<List<SidebarMenuItemModel>> GetAllForAdminAsync()
        {
            return await _dal.GetAllMenuItemsAsync();
        }

        public async Task<SidebarMenuItemModel?> GetByIdAsync(int id)
        {
            return await _dal.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(SidebarMenuItemModel item)
        {
            var id = await _dal.CreateAsync(item);
            InvalidateCache();
            return id;
        }

        public async Task<bool> UpdateAsync(SidebarMenuItemModel item)
        {
            var success = await _dal.UpdateAsync(item);
            if (success) InvalidateCache();
            return success;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var success = await _dal.DeleteAsync(id);
            if (success) InvalidateCache();
            return success;
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            var success = await _dal.ToggleStatusAsync(id);
            if (success) InvalidateCache();
            return success;
        }

        public async Task<bool> UpdateOrderAsync(int id, int sectionOrder, int menuOrder)
        {
            var success = await _dal.UpdateOrderAsync(id, sectionOrder, menuOrder);
            if (success) InvalidateCache();
            return success;
        }

        public void InvalidateCache()
        {
            _cache.Remove(CacheKey);
        }
    }
}
