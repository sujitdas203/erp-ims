using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.Announcement
{
    public class AnnouncementListItemViewModel
    {
        public Guid ANN_Id { get; set; }
        public string Title { get; set; } = "";
        public string BranchName { get; set; } = "";  // "All Branches" if ANN_BranchId is null
        public string Status { get; set; } = "";
        public DateTime? PublishedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AnnouncementIndexViewModel
    {
        public List<AnnouncementListItemViewModel> Announcements { get; set; } = new();
        public Guid? BranchFilter { get; set; }
        public string? StatusFilter { get; set; }
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public List<SelectListItem> BranchOptions { get; set; } = new();
        public List<SelectListItem> StatusOptions { get; set; } = new();
    }
    public class AnnouncementFormViewModel
    {
        public Guid? ANN_Id { get; set; }
        public Guid? BranchId { get; set; }  // null = tenant-wide
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime? PublishedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;

        public List<SelectListItem> BranchOptions { get; set; } = new();
    }
}
