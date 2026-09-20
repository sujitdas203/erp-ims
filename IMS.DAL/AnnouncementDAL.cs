using IMS.DAL.Common;
using IMS.DAL.Interfaces;
using IMS.Models.Announcement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAL
{
    public class AnnouncementDAL: IAnnouncementDAL
    {
        private readonly DBHelper _dbHelper;

        public AnnouncementDAL(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<Guid> CreateAsync(Announcement a)
        {
            a.ANN_Id = a.ANN_Id == Guid.Empty ? Guid.NewGuid() : a.ANN_Id;

            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_Announcements_Create", conn);

            cmd.Parameters.Add("@ANN_Id", SqlDbType.UniqueIdentifier).Value = a.ANN_Id;
            cmd.Parameters.Add("@ANN_TenantId", SqlDbType.UniqueIdentifier).Value = a.ANN_TenantId;
            cmd.Parameters.Add("@ANN_BranchId", SqlDbType.UniqueIdentifier).Value = (object?)a.ANN_BranchId ?? DBNull.Value;
            cmd.Parameters.Add("@ANN_Title", SqlDbType.NVarChar, 200).Value = a.ANN_Title;
            cmd.Parameters.Add("@ANN_Content", SqlDbType.NVarChar, -1).Value = a.ANN_Content;
            cmd.Parameters.Add("@ANN_PublishedAt", SqlDbType.DateTime2).Value = (object?)a.ANN_PublishedAt ?? DBNull.Value;
            cmd.Parameters.Add("@ANN_ExpiresAt", SqlDbType.DateTime2).Value = (object?)a.ANN_ExpiresAt ?? DBNull.Value;
            cmd.Parameters.Add("@ANN_CreatedBy", SqlDbType.UniqueIdentifier).Value = a.ANN_CreatedBy;

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return a.ANN_Id;
        }

        public async Task<bool> UpdateAsync(Announcement a)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_Announcements_Update", conn);

            cmd.Parameters.Add("@ANN_Id", SqlDbType.UniqueIdentifier).Value = a.ANN_Id;
            cmd.Parameters.Add("@ANN_TenantId", SqlDbType.UniqueIdentifier).Value = a.ANN_TenantId;
            cmd.Parameters.Add("@ANN_BranchId", SqlDbType.UniqueIdentifier).Value = (object?)a.ANN_BranchId ?? DBNull.Value;
            cmd.Parameters.Add("@ANN_Title", SqlDbType.NVarChar, 200).Value = a.ANN_Title;
            cmd.Parameters.Add("@ANN_Content", SqlDbType.NVarChar, -1).Value = a.ANN_Content;
            cmd.Parameters.Add("@ANN_PublishedAt", SqlDbType.DateTime2).Value = (object?)a.ANN_PublishedAt ?? DBNull.Value;
            cmd.Parameters.Add("@ANN_ExpiresAt", SqlDbType.DateTime2).Value = (object?)a.ANN_ExpiresAt ?? DBNull.Value;

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }

        public async Task<bool> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_Announcements_ToggleActive", conn);
            cmd.Parameters.Add("@ANN_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@ANN_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_Announcements_Delete", conn);
            cmd.Parameters.Add("@ANN_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@ANN_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }

        public async Task<Announcement?> GetByIdAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_Announcements_GetById", conn);
            cmd.Parameters.Add("@ANN_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@ANN_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        public async Task<(List<Announcement> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, Guid? branchId, string? status, string? search, int pageNumber, int pageSize)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_Announcements_GetPaged", conn);

            cmd.Parameters.Add("@ANN_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@BranchId", SqlDbType.UniqueIdentifier).Value = (object?)branchId ?? DBNull.Value;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = (object?)status ?? DBNull.Value;
            cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 200).Value = (object?)search ?? DBNull.Value;
            cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            int total = 0;
            if (await reader.ReadAsync()) total = reader.GetInt32(reader.GetOrdinal("TotalCount"));

            var items = new List<Announcement>();
            await reader.NextResultAsync();
            while (await reader.ReadAsync()) items.Add(Map(reader));

            return (items, total);
        }

        private static Announcement Map(SqlDataReader r) => new()
        {
            ANN_Id = r.GetGuid(r.GetOrdinal("ANN_Id")),
            ANN_TenantId = r.GetGuid(r.GetOrdinal("ANN_TenantId")),
            ANN_BranchId = r["ANN_BranchId"] as Guid?,
            ANN_Title = r["ANN_Title"] as string ?? "",
            ANN_Content = r["ANN_Content"] as string ?? "",
            ANN_PublishedAt = r["ANN_PublishedAt"] as DateTime?,
            ANN_ExpiresAt = r["ANN_ExpiresAt"] as DateTime?,
            ANN_CreatedBy = r.GetGuid(r.GetOrdinal("ANN_CreatedBy")),
            ANN_IsActive = r.GetBoolean(r.GetOrdinal("ANN_IsActive")),
            ANN_CreatedAt = r.GetDateTime(r.GetOrdinal("ANN_CreatedAt")),
            ANN_UpdatedAt = r.GetDateTime(r.GetOrdinal("ANN_UpdatedAt")),
            ComputedStatus = r["ComputedStatus"] as string ?? "",
            BranchName = r["BranchName"] as string
        };
    }
}
