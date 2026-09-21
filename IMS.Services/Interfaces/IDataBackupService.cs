using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using IMS.Models.Backup;

namespace IMS.Services.Interfaces
{
    public interface IDataBackupService
    {
        Task<DataBackupIndexViewModel> GetDashboardModelAsync(Guid tenantId);
        Task<DataBackupPreviewResult> GetPreviewAsync(DataBackupFilterViewModel filter, Guid tenantId);
        Task<DataExportFileResult> ExportModuleAsync(DataBackupFilterViewModel filter, Guid tenantId);
        Task<DataExportFileResult> ExportCompleteBackupZipAsync(DataBackupFilterViewModel filter, Guid tenantId);
    }
}
