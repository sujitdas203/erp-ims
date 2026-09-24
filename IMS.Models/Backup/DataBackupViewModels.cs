using System;
using System.Collections.Generic;

namespace IMS.Models.Backup
{
    public class DataBackupFilterViewModel
    {
        public string Module { get; set; } = "Students";
        public Guid? AcademicYearId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? ProgramId { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? BatchId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }
        public string? SearchTerm { get; set; }
        public string Format { get; set; } = "csv"; // "csv", "excel"
    }

    public class DataBackupModuleInfo
    {
        public string Code { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "fa-database";
        public string Category { get; set; } = "General";
        public int EstimatedCount { get; set; }
        public List<string> SupportedFilters { get; set; } = new();
    }

    public class DataBackupIndexViewModel
    {
        public List<DataBackupModuleInfo> AvailableModules { get; set; } = new();
        public DataBackupFilterViewModel CurrentFilter { get; set; } = new();
        public int TotalSystemRecords { get; set; }
        public DateTime LastBackupDate { get; set; } = DateTime.UtcNow;
    }

    public class DataBackupPreviewResult
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public int TotalRecords { get; set; }
        public List<string> ColumnNames { get; set; } = new();
        public List<Dictionary<string, object>> SampleRows { get; set; } = new();
    }

    public class DataExportFileResult
    {
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "text/csv";
        public string FileName { get; set; } = "export.csv";
        public int RecordCount { get; set; }
    }
}
