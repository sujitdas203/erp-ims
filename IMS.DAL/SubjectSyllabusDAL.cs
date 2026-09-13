using IMS.DAL.Common;
using IMS.DAL.Interfaces;
using IMS.Models.SubjectSyllabus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAL
{
    public class SubjectSyllabusDAL : ISubjectSyllabusDAL
    {
        private readonly DBHelper _dbHelper;

        public SubjectSyllabusDAL(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<Guid> CreateAsync(SubjectSyllabus s)
        {
            s.SS_Id = s.SS_Id == Guid.Empty ? Guid.NewGuid() : s.SS_Id;

            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SubjectSyllabus_Create", conn);
            AddCommonParameters(cmd, s);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return s.SS_Id;
        }

        public async Task<bool> UpdateAsync(SubjectSyllabus s)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SubjectSyllabus_Update", conn);
            AddCommonParameters(cmd, s);

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }

        public async Task<bool> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SubjectSyllabus_ToggleActive", conn);
            cmd.Parameters.Add("@SS_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@SS_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }

        public async Task<bool> ToggleCompletedAsync(Guid id, Guid tenantId, bool isCompleted)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SubjectSyllabus_ToggleCompleted", conn);
            cmd.Parameters.Add("@SS_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@SS_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@IsCompleted", SqlDbType.Bit).Value = isCompleted;

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SubjectSyllabus_Delete", conn);
            cmd.Parameters.Add("@SS_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@SS_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }

        public async Task<SubjectSyllabus?> GetByIdAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SubjectSyllabus_GetById", conn);
            cmd.Parameters.Add("@SS_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@SS_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        public async Task<(List<SubjectSyllabus> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, Guid? courseId, Guid? subjectId, string? status, string? search, int pageNumber, int pageSize)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SubjectSyllabus_GetPaged", conn);

            cmd.Parameters.Add("@SS_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@CourseId", SqlDbType.UniqueIdentifier).Value = (object?)courseId ?? DBNull.Value;
            cmd.Parameters.Add("@SubjectId", SqlDbType.UniqueIdentifier).Value = (object?)subjectId ?? DBNull.Value;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = (object?)status ?? DBNull.Value;
            cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 200).Value = (object?)search ?? DBNull.Value;
            cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            int total = 0;
            if (await reader.ReadAsync()) total = reader.GetInt32(reader.GetOrdinal("TotalCount"));

            var items = new List<SubjectSyllabus>();
            await reader.NextResultAsync();
            while (await reader.ReadAsync()) items.Add(Map(reader));

            return (items, total);
        }

        public async Task<List<(Guid Id, string Name)>> GetCourseOptionsAsync(Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_Courses_GetActiveList", conn);
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            var list = new List<(Guid, string)>();
            while (await reader.ReadAsync())
                list.Add((reader.GetGuid(reader.GetOrdinal("C_Id")), reader["C_Name"] as string ?? ""));
            return list;
        }

        public async Task<List<(Guid Id, string Name, string Code)>> GetSubjectOptionsAsync(Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_Subjects_GetActiveList", conn);
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            var list = new List<(Guid, string, string)>();
            while (await reader.ReadAsync())
                list.Add((
                    reader.GetGuid(reader.GetOrdinal("SB_Id")),
                    reader["SB_Name"] as string ?? "",
                    reader["SB_Code"] as string ?? ""));
            return list;
        }

        private static void AddCommonParameters(SqlCommand cmd, SubjectSyllabus s)
        {
            cmd.Parameters.Add("@SS_Id", SqlDbType.UniqueIdentifier).Value = s.SS_Id;
            cmd.Parameters.Add("@SS_TenantId", SqlDbType.UniqueIdentifier).Value = s.SS_TenantId;
            cmd.Parameters.Add("@SS_CourseId", SqlDbType.UniqueIdentifier).Value = s.SS_CourseId;
            cmd.Parameters.Add("@SS_SubjectId", SqlDbType.UniqueIdentifier).Value = s.SS_SubjectId;
            cmd.Parameters.Add("@SS_UnitNumber", SqlDbType.Int).Value = s.SS_UnitNumber;
            cmd.Parameters.Add("@SS_UnitTitle", SqlDbType.NVarChar, 200).Value = s.SS_UnitTitle;
            cmd.Parameters.Add("@SS_Description", SqlDbType.NVarChar, -1).Value = (object?)s.SS_Description ?? DBNull.Value;
            cmd.Parameters.Add("@SS_TotalHours", SqlDbType.Int).Value = (object?)s.SS_TotalHours ?? DBNull.Value;
            cmd.Parameters.Add("@SS_FileUrl", SqlDbType.NVarChar, 500).Value = (object?)s.SS_FileUrl ?? DBNull.Value;
        }

        private static SubjectSyllabus Map(SqlDataReader r) => new()
        {
            SS_Id = r.GetGuid(r.GetOrdinal("SS_Id")),
            SS_TenantId = r.GetGuid(r.GetOrdinal("SS_TenantId")),
            SS_CourseId = r.GetGuid(r.GetOrdinal("SS_CourseId")),
            SS_SubjectId = r.GetGuid(r.GetOrdinal("SS_SubjectId")),
            SS_UnitNumber = r.GetInt32(r.GetOrdinal("SS_UnitNumber")),
            SS_UnitTitle = r["SS_UnitTitle"] as string ?? "",
            SS_Description = r["SS_Description"] as string,
            SS_TotalHours = r["SS_TotalHours"] as int?,
            SS_FileUrl = r["SS_FileUrl"] as string,
            SS_IsCompleted = r.GetBoolean(r.GetOrdinal("SS_IsCompleted")),
            SS_IsActive = r.GetBoolean(r.GetOrdinal("SS_IsActive")),
            SS_CreatedAt = r.GetDateTime(r.GetOrdinal("SS_CreatedAt")),
            CourseName = r["CourseName"] as string ?? "",
            SubjectName = r["SubjectName"] as string ?? ""
        };
    }
}
