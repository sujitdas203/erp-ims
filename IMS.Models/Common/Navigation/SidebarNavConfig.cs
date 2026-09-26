using System;
using System.Collections.Generic;

namespace IMS.Models.Common.Navigation
{
    /// <summary>
    /// Database entity model representing a row in SidebarMenuItems_SBM.
    /// </summary>
    public class SidebarMenuItemModel
    {
        public int Id { get; set; }
        public string SectionTitle { get; set; } = string.Empty;
        public int SectionOrder { get; set; } = 1;
        public string Title { get; set; } = string.Empty;
        public string? Controller { get; set; } = string.Empty;
        public string? Action { get; set; } = "Index";
        public string? Area { get; set; } = string.Empty;
        public string Icon { get; set; } = "fa-circle";
        public string? PermissionCode { get; set; }
        public int MenuOrder { get; set; } = 1;
        public string? BadgeText { get; set; }
        public string? BadgeClass { get; set; } = "IMS-soon-badge";
        public string? CustomUrl { get; set; }
        public bool IsCollapsible { get; set; }
        public string? CollapseTargetId { get; set; }
        public bool IsMasterManagement { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Grouped representation of a sidebar navigation section.
    /// </summary>
    public class NavSectionConfig
    {
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public string? Description { get; set; }
        public List<NavItemConfig> Items { get; set; } = new();
    }

    /// <summary>
    /// Individual item representation within a sidebar section.
    /// </summary>
    public class NavItemConfig
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string Action { get; set; } = "Index";
        public string Area { get; set; } = string.Empty;
        public string Icon { get; set; } = "fa-circle";
        public string? PermissionCode { get; set; }
        public int Order { get; set; }
        public string? BadgeText { get; set; }
        public string? BadgeClass { get; set; } = "IMS-soon-badge";
        public string? CustomUrl { get; set; }
        public bool IsCollapsible { get; set; }
        public string? CollapseTargetId { get; set; }
        public bool IsMasterManagement { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
