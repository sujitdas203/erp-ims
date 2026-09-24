using System;
using System.Collections.Generic;
using System.Data;
using IMS.Models.Backup;

namespace IMS.DAL.Interfaces
{
    public interface IDataBackupDAL
    {
        DataTable GetModuleData(string module, DataBackupFilterViewModel filter, Guid tenantId);
        int GetModuleCount(string module, DataBackupFilterViewModel filter, Guid tenantId);
        Dictionary<string, int> GetSystemSummaryCounts(Guid tenantId);
        List<DataBackupModuleInfo> GetAvailableModules(Guid tenantId);
    }
}
