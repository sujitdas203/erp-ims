using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using IMS.DAL.Common;
using IMS.DAL.Interfaces;
using IMS.Models.HomeTask;

namespace IMS.DAL
{
    public class HomeTaskDAL : IHomeTaskDAL
    {
        private readonly DBHelper _dbHelper;

        public HomeTaskDAL(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<(List<HomeTask> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, Guid? batchId, Guid? subjectId, string? status, string? search, int pageNumber, int pageSize)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_HomeTasks_GetPaged", conn);

            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@BatchId", SqlDbType.UniqueIdentifier).Value = (object?)batchId ?? DBNull.Value;
            cmd.Parameters.Add("@SubjectId", SqlDbType.UniqueIdentifier).Value = (object?)subjectId ?? DBNull.Value;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = (object?)status ?? DBNull.Value;
            cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 200).Value = (object?)search ?? DBNull.Value;
            cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            int totalCount = 0;
            if (await reader.ReadAsync())
                totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));

            var items = new List<HomeTask>();
            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                items.Add(new HomeTask
                {
                    HT_Id = reader.GetGuid(reader.GetOrdinal("HT_Id")),
                    HT_TenantId = reader.GetGuid(reader.GetOrdinal("HT_TenantId")),
                    HT_BatchId = reader.GetGuid(reader.GetOrdinal("HT_BatchId")),
                    BatchName = reader["BatchName"] as string ?? "-",
                    HT_SubjectId = reader.GetGuid(reader.GetOrdinal("HT_SubjectId")),
                    SubjectName = reader["SubjectName"] as string ?? "-",
                    HT_TeacherId = reader["HT_TeacherId"] as Guid?,
                    TeacherName = reader["TeacherName"] as string,
                    HT_Title = reader["HT_Title"] as string ?? "",
                    HT_Description = reader["HT_Description"] as string ?? "",
                    HT_AssignedDate = reader.GetDateTime(reader.GetOrdinal("HT_AssignedDate")),
                    HT_DueDate = reader.GetDateTime(reader.GetOrdinal("HT_DueDate")),
                    HT_AttachmentUrl = reader["HT_AttachmentUrl"] as string,
                    HT_MaxMarks = reader["HT_MaxMarks"] != DBNull.Value ? reader.GetDecimal(reader.GetOrdinal("HT_MaxMarks")) : null,
                    HT_Status = reader["HT_Status"] as string ?? "Active",
                    HT_IsActive = reader.GetBoolean(reader.GetOrdinal("HT_IsActive")),
                    HT_CreatedAt = reader.GetDateTime(reader.GetOrdinal("HT_CreatedAt")),
                    HT_UpdatedAt = reader.GetDateTime(reader.GetOrdinal("HT_UpdatedAt")),
                    SubmissionCount = reader.GetInt32(reader.GetOrdinal("SubmissionCount"))
                });
            }

            return (items, totalCount);
        }

        public async Task<HomeTaskDetailsViewModel?> GetDetailsByIdAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_HomeTasks_GetById", conn);
            cmd.Parameters.Add("@HT_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@HT_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;

            var vm = new HomeTaskDetailsViewModel
            {
                Task = new HomeTask
                {
                    HT_Id = reader.GetGuid(reader.GetOrdinal("HT_Id")),
                    HT_TenantId = reader.GetGuid(reader.GetOrdinal("HT_TenantId")),
                    HT_BatchId = reader.GetGuid(reader.GetOrdinal("HT_BatchId")),
                    BatchName = reader["BatchName"] as string ?? "-",
                    HT_SubjectId = reader.GetGuid(reader.GetOrdinal("HT_SubjectId")),
                    SubjectName = reader["SubjectName"] as string ?? "-",
                    HT_TeacherId = reader["HT_TeacherId"] as Guid?,
                    TeacherName = reader["TeacherName"] as string,
                    HT_Title = reader["HT_Title"] as string ?? "",
                    HT_Description = reader["HT_Description"] as string ?? "",
                    HT_AssignedDate = reader.GetDateTime(reader.GetOrdinal("HT_AssignedDate")),
                    HT_DueDate = reader.GetDateTime(reader.GetOrdinal("HT_DueDate")),
                    HT_AttachmentUrl = reader["HT_AttachmentUrl"] as string,
                    HT_MaxMarks = reader["HT_MaxMarks"] != DBNull.Value ? reader.GetDecimal(reader.GetOrdinal("HT_MaxMarks")) : null,
                    HT_Status = reader["HT_Status"] as string ?? "Active",
                    HT_IsActive = reader.GetBoolean(reader.GetOrdinal("HT_IsActive")),
                    HT_CreatedAt = reader.GetDateTime(reader.GetOrdinal("HT_CreatedAt")),
                    HT_UpdatedAt = reader.GetDateTime(reader.GetOrdinal("HT_UpdatedAt"))
                }
            };

            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                vm.Submissions.Add(new HomeTaskSubmissionItemViewModel
                {
                    HTS_Id = reader.GetGuid(reader.GetOrdinal("HTS_Id")),
                    HTS_StudentId = reader.GetGuid(reader.GetOrdinal("HTS_StudentId")),
                    StudentCode = reader["S_StudentCode"] as string ?? "",
                    AdmissionNumber = reader["S_AdmissionNumber"] as string ?? "",
                    StudentName = reader["StudentName"] as string ?? "",
                    SubmissionDate = reader.GetDateTime(reader.GetOrdinal("HTS_SubmissionDate")),
                    Content = reader["HTS_Content"] as string,
                    AttachmentUrl = reader["HTS_AttachmentUrl"] as string,
                    MarksObtained = reader["HTS_MarksObtained"] != DBNull.Value ? reader.GetDecimal(reader.GetOrdinal("HTS_MarksObtained")) : null,
                    TeacherRemarks = reader["HTS_TeacherRemarks"] as string,
                    Status = reader["HTS_Status"] as string ?? ""
                });
            }

            return vm;
        }

        public async Task<HomeTask?> GetByIdAsync(Guid id, Guid tenantId)
        {
            var details = await GetDetailsByIdAsync(id, tenantId);
            return details?.Task;
        }

        public async Task<Guid> CreateAsync(HomeTask task)
        {
            task.HT_Id = task.HT_Id == Guid.Empty ? Guid.NewGuid() : task.HT_Id;

            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_HomeTasks_Create", conn);

            cmd.Parameters.Add("@HT_Id", SqlDbType.UniqueIdentifier).Value = task.HT_Id;
            cmd.Parameters.Add("@HT_TenantId", SqlDbType.UniqueIdentifier).Value = task.HT_TenantId;
            cmd.Parameters.Add("@HT_BatchId", SqlDbType.UniqueIdentifier).Value = task.HT_BatchId;
            cmd.Parameters.Add("@HT_SubjectId", SqlDbType.UniqueIdentifier).Value = task.HT_SubjectId;
            cmd.Parameters.Add("@HT_TeacherId", SqlDbType.UniqueIdentifier).Value = (object?)task.HT_TeacherId ?? DBNull.Value;
            cmd.Parameters.Add("@HT_Title", SqlDbType.NVarChar, 200).Value = task.HT_Title;
            cmd.Parameters.Add("@HT_Description", SqlDbType.NVarChar, -1).Value = task.HT_Description;
            cmd.Parameters.Add("@HT_AssignedDate", SqlDbType.Date).Value = task.HT_AssignedDate;
            cmd.Parameters.Add("@HT_DueDate", SqlDbType.Date).Value = task.HT_DueDate;
            cmd.Parameters.Add("@HT_AttachmentUrl", SqlDbType.NVarChar, 500).Value = (object?)task.HT_AttachmentUrl ?? DBNull.Value;
            cmd.Parameters.Add("@HT_MaxMarks", SqlDbType.Decimal).Value = (object?)task.HT_MaxMarks ?? DBNull.Value;
            cmd.Parameters.Add("@HT_Status", SqlDbType.NVarChar, 20).Value = task.HT_Status;

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return task.HT_Id;
        }

        public async Task<bool> UpdateAsync(HomeTask task)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_HomeTasks_Update", conn);

            cmd.Parameters.Add("@HT_Id", SqlDbType.UniqueIdentifier).Value = task.HT_Id;
            cmd.Parameters.Add("@HT_TenantId", SqlDbType.UniqueIdentifier).Value = task.HT_TenantId;
            cmd.Parameters.Add("@HT_BatchId", SqlDbType.UniqueIdentifier).Value = task.HT_BatchId;
            cmd.Parameters.Add("@HT_SubjectId", SqlDbType.UniqueIdentifier).Value = task.HT_SubjectId;
            cmd.Parameters.Add("@HT_TeacherId", SqlDbType.UniqueIdentifier).Value = (object?)task.HT_TeacherId ?? DBNull.Value;
            cmd.Parameters.Add("@HT_Title", SqlDbType.NVarChar, 200).Value = task.HT_Title;
            cmd.Parameters.Add("@HT_Description", SqlDbType.NVarChar, -1).Value = task.HT_Description;
            cmd.Parameters.Add("@HT_AssignedDate", SqlDbType.Date).Value = task.HT_AssignedDate;
            cmd.Parameters.Add("@HT_DueDate", SqlDbType.Date).Value = task.HT_DueDate;
            cmd.Parameters.Add("@HT_AttachmentUrl", SqlDbType.NVarChar, 500).Value = (object?)task.HT_AttachmentUrl ?? DBNull.Value;
            cmd.Parameters.Add("@HT_MaxMarks", SqlDbType.Decimal).Value = (object?)task.HT_MaxMarks ?? DBNull.Value;
            cmd.Parameters.Add("@HT_Status", SqlDbType.NVarChar, 20).Value = task.HT_Status;

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_HomeTasks_Delete", conn);
            cmd.Parameters.Add("@HT_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@HT_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }

        public async Task<bool> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_HomeTasks_ToggleActive", conn);
            cmd.Parameters.Add("@HT_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@HT_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }
    }
}
