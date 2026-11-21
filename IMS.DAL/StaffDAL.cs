using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using IMS.DAL.Common;
using IMS.DAL.Interfaces;
using IMS.Models.Entities;

namespace IMS.DAL.Repositories
{
    public class StaffDAL : IStaffDAL
    {
        private readonly DBHelper _dbHelper;

        public StaffDAL(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<Staff> GetByIdAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Staff_ST", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetById";
            cmd.Parameters.Add("@ST_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@ST_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapStaff(reader) : null;
        }

        public async Task<(List<Staff> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, string searchTerm, Guid? branchId, string status,
            int pageNumber, int pageSize)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Staff_ST", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetPaged";
            cmd.Parameters.Add("@ST_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 255).Value = (object)searchTerm ?? DBNull.Value;
            cmd.Parameters.Add("@BranchId", SqlDbType.UniqueIdentifier).Value = (object)branchId ?? DBNull.Value;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = (object)status ?? DBNull.Value;
            cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            int totalCount = 0;
            if (await reader.ReadAsync())
                totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));

            var items = new List<Staff>();
            await reader.NextResultAsync();
            while (await reader.ReadAsync())
                items.Add(MapStaff(reader));

            return (items, totalCount);
        }

        public async Task<bool> IsEmployeeCodeTakenAsync(Guid tenantId, string code, Guid? excludeId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Staff_ST", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "ExistsByCode";
            cmd.Parameters.Add("@ST_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@ST_EmployeeCode", SqlDbType.NVarChar, 50).Value = code;
            cmd.Parameters.Add("@ExcludeId", SqlDbType.UniqueIdentifier).Value = (object)excludeId ?? DBNull.Value;

            await conn.OpenAsync();
            return (int)await cmd.ExecuteScalarAsync() == 1;
        }

        public async Task<Guid> CreateAsync(Staff staff)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Staff_ST", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Insert";
            AddParameters(cmd, staff);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Guid.TryParse(result?.ToString(), out var id) ? id : staff.ST_Id;
        }

        public async Task<bool> UpdateAsync(Staff staff)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Staff_ST", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Update";
            AddParameters(cmd, staff);

            await conn.OpenAsync();
            return (int)await cmd.ExecuteScalarAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Staff_ST", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Delete";
            cmd.Parameters.Add("@ST_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@ST_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            return (int)await cmd.ExecuteScalarAsync() > 0;
        }

        private void AddParameters(SqlCommand cmd, Staff s)
        {
            cmd.Parameters.Add("@ST_Id", SqlDbType.UniqueIdentifier).Value = s.ST_Id;
            cmd.Parameters.Add("@ST_TenantId", SqlDbType.UniqueIdentifier).Value = s.ST_TenantId;
            cmd.Parameters.Add("@ST_BranchId", SqlDbType.UniqueIdentifier).Value = s.ST_BranchId;
            cmd.Parameters.Add("@ST_DepartmentId", SqlDbType.UniqueIdentifier).Value = (object)s.ST_DepartmentId ?? DBNull.Value;
            cmd.Parameters.Add("@ST_DesignationId", SqlDbType.UniqueIdentifier).Value = (object)s.ST_DesignationId ?? DBNull.Value;
            cmd.Parameters.Add("@ST_EmployeeCode", SqlDbType.NVarChar, 50).Value = s.ST_EmployeeCode;
            cmd.Parameters.Add("@ST_FirstName", SqlDbType.NVarChar, 100).Value = s.ST_FirstName;
            cmd.Parameters.Add("@ST_LastName", SqlDbType.NVarChar, 100).Value = s.ST_LastName;
            cmd.Parameters.Add("@ST_Gender", SqlDbType.NVarChar, 20).Value = (object)s.ST_Gender ?? DBNull.Value;
            cmd.Parameters.Add("@ST_DateOfBirth", SqlDbType.Date).Value = (object)s.ST_DateOfBirth ?? DBNull.Value;
            cmd.Parameters.Add("@ST_BloodGroup", SqlDbType.NVarChar, 10).Value = (object)s.ST_BloodGroup ?? DBNull.Value;
            cmd.Parameters.Add("@ST_Qualification", SqlDbType.NVarChar, 255).Value = (object)s.ST_Qualification ?? DBNull.Value;
            cmd.Parameters.Add("@ST_ExperienceYears", SqlDbType.Int).Value = (object)s.ST_ExperienceYears ?? DBNull.Value;
            cmd.Parameters.Add("@ST_Address", SqlDbType.NVarChar, 500).Value = (object)s.ST_Address ?? DBNull.Value;
            cmd.Parameters.Add("@ST_EmergencyContact", SqlDbType.NVarChar, 50).Value = (object)s.ST_EmergencyContact ?? DBNull.Value;
            cmd.Parameters.Add("@ST_BasicSalary", SqlDbType.Decimal).Value = (object)s.ST_BasicSalary ?? DBNull.Value;
            cmd.Parameters.Add("@ST_Email", SqlDbType.NVarChar, 255).Value = (object)s.ST_Email ?? DBNull.Value;
            cmd.Parameters.Add("@ST_Phone", SqlDbType.NVarChar, 30).Value = (object)s.ST_Phone ?? DBNull.Value;
            cmd.Parameters.Add("@ST_JoiningDate", SqlDbType.Date).Value = (object)s.ST_JoiningDate ?? DBNull.Value;
            cmd.Parameters.Add("@ST_Status", SqlDbType.NVarChar, 20).Value = s.ST_Status;
        }

        private static Staff MapStaff(SqlDataReader r)
        {
            var s = new Staff
            {
                ST_Id = r.GetGuid(r.GetOrdinal("ST_Id")),
                ST_TenantId = r.GetGuid(r.GetOrdinal("ST_TenantId")),
                ST_BranchId = r.GetGuid(r.GetOrdinal("ST_BranchId")),
                ST_UserId = r["ST_UserId"] as Guid?,
                ST_DepartmentId = r["ST_DepartmentId"] as Guid?,
                ST_DesignationId = r["ST_DesignationId"] as Guid?,
                ST_EmployeeCode = r["ST_EmployeeCode"] as string,
                ST_FirstName = r["ST_FirstName"] as string,
                ST_LastName = r["ST_LastName"] as string,
                ST_Gender = HasColumn(r, "ST_Gender") ? r["ST_Gender"] as string : null,
                ST_DateOfBirth = HasColumn(r, "ST_DateOfBirth") ? r["ST_DateOfBirth"] as DateTime? : null,
                ST_BloodGroup = HasColumn(r, "ST_BloodGroup") ? r["ST_BloodGroup"] as string : null,
                ST_Qualification = HasColumn(r, "ST_Qualification") ? r["ST_Qualification"] as string : null,
                ST_ExperienceYears = HasColumn(r, "ST_ExperienceYears") ? r["ST_ExperienceYears"] as int? : null,
                ST_Address = HasColumn(r, "ST_Address") ? r["ST_Address"] as string : null,
                ST_EmergencyContact = HasColumn(r, "ST_EmergencyContact") ? r["ST_EmergencyContact"] as string : null,
                ST_BasicSalary = HasColumn(r, "ST_BasicSalary") && r["ST_BasicSalary"] != DBNull.Value ? Convert.ToDecimal(r["ST_BasicSalary"]) : null,
                ST_Email = r["ST_Email"] as string,
                ST_Phone = r["ST_Phone"] as string,
                ST_JoiningDate = r["ST_JoiningDate"] as DateTime?,
                ST_Status = r["ST_Status"] as string,
                ST_CreatedAt = r.GetDateTime(r.GetOrdinal("ST_CreatedAt")),
                ST_UpdatedAt = r.GetDateTime(r.GetOrdinal("ST_UpdatedAt")),
                BranchName = HasColumn(r, "BranchName") ? r["BranchName"] as string : null,
                DepartmentName = HasColumn(r, "DepartmentName") ? r["DepartmentName"] as string : null,
                DesignationName = HasColumn(r, "DesignationName") ? r["DesignationName"] as string : null
            };
            return s;
        }

        private static bool HasColumn(SqlDataReader r, string columnName)
        {
            for (int i = 0; i < r.FieldCount; i++)
            {
                if (string.Equals(r.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
