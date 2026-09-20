using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.SubjectSyllabus
{
    public class SubjectSyllabus
    {
        public Guid SS_Id { get; set; }
        public Guid SS_TenantId { get; set; }
        public Guid SS_CourseId { get; set; }
        public Guid SS_SubjectId { get; set; }
        public int SS_UnitNumber { get; set; }
        public string SS_UnitTitle { get; set; } = "";
        public string? SS_Description { get; set; }
        public int? SS_TotalHours { get; set; }
        public string? SS_FileUrl { get; set; }
        public bool SS_IsCompleted { get; set; }
        public bool SS_IsActive { get; set; }
        public DateTime SS_CreatedAt { get; set; }
        public string CourseName { get; set; } = "";
        public string SubjectName { get; set; } = "";
    }
}
