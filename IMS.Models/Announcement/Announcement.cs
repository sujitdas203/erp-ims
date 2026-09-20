using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.Announcement
{
    public class Announcement
    {
        public Guid ANN_Id { get; set; }
        public Guid ANN_TenantId { get; set; }
        public Guid? ANN_BranchId { get; set; }
        public string ANN_Title { get; set; } = "";
        public string ANN_Content { get; set; } = "";
        public DateTime? ANN_PublishedAt { get; set; }
        public DateTime? ANN_ExpiresAt { get; set; }
        public Guid ANN_CreatedBy { get; set; }
        public bool ANN_IsActive { get; set; }
        public DateTime ANN_CreatedAt { get; set; }
        public DateTime ANN_UpdatedAt { get; set; }
        public string ComputedStatus { get; set; } = "";
        public string? BranchName { get; set; }
    }
}
