using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using IMS.DAL.Common;
using IMS.DAL.Interfaces;
using IMS.Models.MockTest;

namespace IMS.DAL
{
    public class MockTestDAL : IMockTestDAL
    {
        private readonly DBHelper _dbHelper;

        public MockTestDAL(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<(List<MockTest> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, Guid? batchId, Guid? subjectId, string? status, string? search, int pageNumber, int pageSize)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_MockTests_GetPaged", conn);

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

            var items = new List<MockTest>();
            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                items.Add(new MockTest
                {
                    MT_Id = reader.GetGuid(reader.GetOrdinal("MT_Id")),
                    MT_TenantId = reader.GetGuid(reader.GetOrdinal("MT_TenantId")),
                    MT_BatchId = reader.GetGuid(reader.GetOrdinal("MT_BatchId")),
                    BatchName = reader["BatchName"] as string ?? "-",
                    MT_SubjectId = reader.GetGuid(reader.GetOrdinal("MT_SubjectId")),
                    SubjectName = reader["SubjectName"] as string ?? "-",
                    MT_Title = reader["MT_Title"] as string ?? "",
                    MT_Description = reader["MT_Description"] as string,
                    MT_TestDate = reader.GetDateTime(reader.GetOrdinal("MT_TestDate")),
                    MT_DurationMinutes = reader.GetInt32(reader.GetOrdinal("MT_DurationMinutes")),
                    MT_TotalMarks = reader.GetDecimal(reader.GetOrdinal("MT_TotalMarks")),
                    MT_PassMarks = reader.GetDecimal(reader.GetOrdinal("MT_PassMarks")),
                    MT_Status = reader["MT_Status"] as string ?? "Scheduled",
                    MT_IsActive = reader.GetBoolean(reader.GetOrdinal("MT_IsActive")),
                    MT_CreatedAt = reader.GetDateTime(reader.GetOrdinal("MT_CreatedAt")),
                    ResultCount = reader.GetInt32(reader.GetOrdinal("ResultCount")),
                    AvgScore = reader["AvgScore"] != DBNull.Value ? Convert.ToDecimal(reader["AvgScore"]) : null
                });
            }

            return (items, totalCount);
        }

        public async Task<MockTestDetailsViewModel?> GetDetailsByIdAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_MockTests_GetById", conn);
            cmd.Parameters.Add("@MT_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@MT_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;

            var vm = new MockTestDetailsViewModel
            {
                Test = new MockTest
                {
                    MT_Id = reader.GetGuid(reader.GetOrdinal("MT_Id")),
                    MT_TenantId = reader.GetGuid(reader.GetOrdinal("MT_TenantId")),
                    MT_BatchId = reader.GetGuid(reader.GetOrdinal("MT_BatchId")),
                    BatchName = reader["BatchName"] as string ?? "-",
                    MT_SubjectId = reader.GetGuid(reader.GetOrdinal("MT_SubjectId")),
                    SubjectName = reader["SubjectName"] as string ?? "-",
                    MT_Title = reader["MT_Title"] as string ?? "",
                    MT_Description = reader["MT_Description"] as string,
                    MT_TestDate = reader.GetDateTime(reader.GetOrdinal("MT_TestDate")),
                    MT_DurationMinutes = reader.GetInt32(reader.GetOrdinal("MT_DurationMinutes")),
                    MT_TotalMarks = reader.GetDecimal(reader.GetOrdinal("MT_TotalMarks")),
                    MT_PassMarks = reader.GetDecimal(reader.GetOrdinal("MT_PassMarks")),
                    MT_Status = reader["MT_Status"] as string ?? "Scheduled",
                    MT_IsActive = reader.GetBoolean(reader.GetOrdinal("MT_IsActive")),
                    MT_CreatedAt = reader.GetDateTime(reader.GetOrdinal("MT_CreatedAt"))
                }
            };

            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                vm.Results.Add(new MockTestStudentResultViewModel
                {
                    MTR_Id = reader.GetGuid(reader.GetOrdinal("MTR_Id")),
                    MTR_StudentId = reader.GetGuid(reader.GetOrdinal("MTR_StudentId")),
                    StudentCode = reader["S_StudentCode"] as string ?? "",
                    AdmissionNumber = reader["S_AdmissionNumber"] as string ?? "",
                    StudentName = reader["StudentName"] as string ?? "",
                    Score = reader.GetDecimal(reader.GetOrdinal("MTR_Score")),
                    Percentage = reader.GetDecimal(reader.GetOrdinal("MTR_Percentage")),
                    Grade = reader["MTR_Grade"] as string,
                    Status = reader["MTR_Status"] as string ?? "",
                    CompletedAt = reader.GetDateTime(reader.GetOrdinal("MTR_CompletedAt"))
                });
            }

            return vm;
        }

        public async Task<MockTest?> GetByIdAsync(Guid id, Guid tenantId)
        {
            var details = await GetDetailsByIdAsync(id, tenantId);
            return details?.Test;
        }

        public async Task<Guid> CreateAsync(MockTest test)
        {
            test.MT_Id = test.MT_Id == Guid.Empty ? Guid.NewGuid() : test.MT_Id;

            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_MockTests_Create", conn);

            cmd.Parameters.Add("@MT_Id", SqlDbType.UniqueIdentifier).Value = test.MT_Id;
            cmd.Parameters.Add("@MT_TenantId", SqlDbType.UniqueIdentifier).Value = test.MT_TenantId;
            cmd.Parameters.Add("@MT_BatchId", SqlDbType.UniqueIdentifier).Value = test.MT_BatchId;
            cmd.Parameters.Add("@MT_SubjectId", SqlDbType.UniqueIdentifier).Value = test.MT_SubjectId;
            cmd.Parameters.Add("@MT_Title", SqlDbType.NVarChar, 200).Value = test.MT_Title;
            cmd.Parameters.Add("@MT_Description", SqlDbType.NVarChar, -1).Value = (object?)test.MT_Description ?? DBNull.Value;
            cmd.Parameters.Add("@MT_TestDate", SqlDbType.DateTime2).Value = test.MT_TestDate;
            cmd.Parameters.Add("@MT_DurationMinutes", SqlDbType.Int).Value = test.MT_DurationMinutes;
            cmd.Parameters.Add("@MT_TotalMarks", SqlDbType.Decimal).Value = test.MT_TotalMarks;
            cmd.Parameters.Add("@MT_PassMarks", SqlDbType.Decimal).Value = test.MT_PassMarks;
            cmd.Parameters.Add("@MT_Status", SqlDbType.NVarChar, 20).Value = test.MT_Status;

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return test.MT_Id;
        }

        public async Task<bool> UpdateAsync(MockTest test)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_MockTests_Update", conn);

            cmd.Parameters.Add("@MT_Id", SqlDbType.UniqueIdentifier).Value = test.MT_Id;
            cmd.Parameters.Add("@MT_TenantId", SqlDbType.UniqueIdentifier).Value = test.MT_TenantId;
            cmd.Parameters.Add("@MT_BatchId", SqlDbType.UniqueIdentifier).Value = test.MT_BatchId;
            cmd.Parameters.Add("@MT_SubjectId", SqlDbType.UniqueIdentifier).Value = test.MT_SubjectId;
            cmd.Parameters.Add("@MT_Title", SqlDbType.NVarChar, 200).Value = test.MT_Title;
            cmd.Parameters.Add("@MT_Description", SqlDbType.NVarChar, -1).Value = (object?)test.MT_Description ?? DBNull.Value;
            cmd.Parameters.Add("@MT_TestDate", SqlDbType.DateTime2).Value = test.MT_TestDate;
            cmd.Parameters.Add("@MT_DurationMinutes", SqlDbType.Int).Value = test.MT_DurationMinutes;
            cmd.Parameters.Add("@MT_TotalMarks", SqlDbType.Decimal).Value = test.MT_TotalMarks;
            cmd.Parameters.Add("@MT_PassMarks", SqlDbType.Decimal).Value = test.MT_PassMarks;
            cmd.Parameters.Add("@MT_Status", SqlDbType.NVarChar, 20).Value = test.MT_Status;

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_MockTests_Delete", conn);
            cmd.Parameters.Add("@MT_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@MT_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }

        public async Task<bool> ToggleActiveAsync(Guid id, Guid tenantId, bool isActive)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_MockTests_ToggleActive", conn);
            cmd.Parameters.Add("@MT_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@MT_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;

            await conn.OpenAsync();
            var rows = (int)await cmd.ExecuteScalarAsync();
            return rows > 0;
        }
    }
}
