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
    public class EnrollmentDAL : IEnrollmentDAL
    {
        private readonly DBHelper _dbHelper;
        public EnrollmentDAL(DBHelper dbHelper) { _dbHelper = dbHelper; }

        public async Task<Enrollment?> GetByIdAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Enrollments_E", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetById";
            cmd.Parameters.Add("@E_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@E_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        public async Task<(List<Enrollment> Items, int TotalCount)> GetPagedAsync(Guid tenantId, string? searchTerm,
            Guid? academicYearId, Guid? courseId, Guid? batchId, string? status, int page, int pageSize,
            Guid? branchId = null, Guid? classId = null, Guid? sectionId = null)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Enrollments_E", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetPaged";
            cmd.Parameters.Add("@E_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 255).Value = (object?)searchTerm ?? DBNull.Value;
            cmd.Parameters.Add("@AcademicYearId", SqlDbType.UniqueIdentifier).Value = (object?)academicYearId ?? DBNull.Value;
            cmd.Parameters.Add("@BranchId", SqlDbType.UniqueIdentifier).Value = (object?)branchId ?? DBNull.Value;
            cmd.Parameters.Add("@ClassId", SqlDbType.UniqueIdentifier).Value = (object?)classId ?? DBNull.Value;
            cmd.Parameters.Add("@SectionId", SqlDbType.UniqueIdentifier).Value = (object?)sectionId ?? DBNull.Value;
            cmd.Parameters.Add("@CourseId", SqlDbType.UniqueIdentifier).Value = (object?)courseId ?? DBNull.Value;
            cmd.Parameters.Add("@BatchId", SqlDbType.UniqueIdentifier).Value = (object?)batchId ?? DBNull.Value;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = (object?)status ?? DBNull.Value;
            cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = page;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            int totalCount = 0;
            if (await reader.ReadAsync()) totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
            var items = new List<Enrollment>();
            await reader.NextResultAsync();
            while (await reader.ReadAsync()) items.Add(Map(reader));
            return (items, totalCount);
        }

        public async Task<bool> IsDuplicateAsync(Guid tenantId, Guid studentId, Guid academicYearId, Guid? classId, Guid? batchId, Guid? courseId, Guid? excludeId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Enrollments_E", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "IsDuplicate";
            cmd.Parameters.Add("@E_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@E_StudentId", SqlDbType.UniqueIdentifier).Value = studentId;
            cmd.Parameters.Add("@E_AcademicYearId", SqlDbType.UniqueIdentifier).Value = academicYearId;
            cmd.Parameters.Add("@E_ClassId", SqlDbType.UniqueIdentifier).Value = (object?)classId ?? DBNull.Value;
            cmd.Parameters.Add("@E_BatchId", SqlDbType.UniqueIdentifier).Value = (object?)batchId ?? DBNull.Value;
            cmd.Parameters.Add("@E_CourseId", SqlDbType.UniqueIdentifier).Value = (object?)courseId ?? DBNull.Value;
            cmd.Parameters.Add("@ExcludeId", SqlDbType.UniqueIdentifier).Value = (object?)excludeId ?? DBNull.Value;
            await conn.OpenAsync();
            var scalar = await cmd.ExecuteScalarAsync();
            return scalar != null && Convert.ToInt32(scalar) == 1;
        }

        public async Task<Guid> CreateAsync(Enrollment e)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Enrollments_E", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Insert";
            AddParams(cmd, e);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Guid.TryParse(result?.ToString(), out var id) ? id : e.E_Id;
        }

        public async Task<bool> UpdateAsync(Enrollment e)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Enrollments_E", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Update";
            AddParams(cmd, e);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != null && Convert.ToInt32(result) > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Enrollments_E", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Delete";
            cmd.Parameters.Add("@E_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@E_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != null && Convert.ToInt32(result) > 0;
        }

        private void AddParams(SqlCommand cmd, Enrollment e)
        {
            cmd.Parameters.Add("@E_Id", SqlDbType.UniqueIdentifier).Value = e.E_Id;
            cmd.Parameters.Add("@E_TenantId", SqlDbType.UniqueIdentifier).Value = e.E_TenantId;
            cmd.Parameters.Add("@E_BranchId", SqlDbType.UniqueIdentifier).Value = (object?)e.E_BranchId ?? DBNull.Value;
            cmd.Parameters.Add("@E_StudentId", SqlDbType.UniqueIdentifier).Value = e.E_StudentId;
            cmd.Parameters.Add("@E_AcademicYearId", SqlDbType.UniqueIdentifier).Value = e.E_AcademicYearId;
            cmd.Parameters.Add("@E_ClassId", SqlDbType.UniqueIdentifier).Value = (object?)e.E_ClassId ?? DBNull.Value;
            cmd.Parameters.Add("@E_SectionId", SqlDbType.UniqueIdentifier).Value = (object?)e.E_SectionId ?? DBNull.Value;
            cmd.Parameters.Add("@E_CourseId", SqlDbType.UniqueIdentifier).Value = (object?)e.E_CourseId ?? DBNull.Value;
            cmd.Parameters.Add("@E_BatchId", SqlDbType.UniqueIdentifier).Value = (object?)e.E_BatchId ?? DBNull.Value;
            cmd.Parameters.Add("@E_EnrollmentNumber", SqlDbType.NVarChar, 50).Value = (object?)e.E_EnrollmentNumber ?? DBNull.Value;
            cmd.Parameters.Add("@E_RollNumber", SqlDbType.NVarChar, 50).Value = (object?)e.E_RollNumber ?? DBNull.Value;
            cmd.Parameters.Add("@E_EnrollmentDate", SqlDbType.Date).Value = e.E_EnrollmentDate;
            cmd.Parameters.Add("@E_EnrollmentType", SqlDbType.NVarChar, 50).Value = (object?)e.E_EnrollmentType ?? "New Admission";
            cmd.Parameters.Add("@E_Status", SqlDbType.NVarChar, 20).Value = (object?)e.E_Status ?? "Active";
            cmd.Parameters.Add("@E_CompletionDate", SqlDbType.Date).Value = (object?)e.E_CompletionDate ?? DBNull.Value;
            cmd.Parameters.Add("@E_FeeStructureId", SqlDbType.UniqueIdentifier).Value = (object?)e.E_FeeStructureId ?? DBNull.Value;
            cmd.Parameters.Add("@E_Remarks", SqlDbType.NVarChar, -1).Value = (object?)e.E_Remarks ?? DBNull.Value;
        }

        private static Enrollment Map(SqlDataReader r)
        {
            var e = new Enrollment
            {
                E_Id = r.GetGuid(r.GetOrdinal("E_Id")),
                E_TenantId = r.GetGuid(r.GetOrdinal("E_TenantId")),
                E_StudentId = r.GetGuid(r.GetOrdinal("E_StudentId")),
                E_AcademicYearId = r.GetGuid(r.GetOrdinal("E_AcademicYearId")),
                E_EnrollmentNumber = HasColumn(r, "E_EnrollmentNumber") && !r.IsDBNull(r.GetOrdinal("E_EnrollmentNumber")) ? r.GetString(r.GetOrdinal("E_EnrollmentNumber")) : null,
                E_EnrollmentDate = r.GetDateTime(r.GetOrdinal("E_EnrollmentDate")),
                E_Status = HasColumn(r, "E_Status") && !r.IsDBNull(r.GetOrdinal("E_Status")) ? r.GetString(r.GetOrdinal("E_Status")) : "Active",
                E_CompletionDate = HasColumn(r, "E_CompletionDate") && !r.IsDBNull(r.GetOrdinal("E_CompletionDate")) ? r.GetDateTime(r.GetOrdinal("E_CompletionDate")) : null,
                E_CreatedAt = HasColumn(r, "E_CreatedAt") ? r.GetDateTime(r.GetOrdinal("E_CreatedAt")) : DateTime.UtcNow,
                E_UpdatedAt = HasColumn(r, "E_UpdatedAt") ? r.GetDateTime(r.GetOrdinal("E_UpdatedAt")) : DateTime.UtcNow
            };

            if (HasColumn(r, "E_BranchId") && !r.IsDBNull(r.GetOrdinal("E_BranchId")))
                e.E_BranchId = r.GetGuid(r.GetOrdinal("E_BranchId"));

            if (HasColumn(r, "E_ClassId") && !r.IsDBNull(r.GetOrdinal("E_ClassId")))
                e.E_ClassId = r.GetGuid(r.GetOrdinal("E_ClassId"));

            if (HasColumn(r, "E_SectionId") && !r.IsDBNull(r.GetOrdinal("E_SectionId")))
                e.E_SectionId = r.GetGuid(r.GetOrdinal("E_SectionId"));

            if (HasColumn(r, "E_CourseId") && !r.IsDBNull(r.GetOrdinal("E_CourseId")))
                e.E_CourseId = r.GetGuid(r.GetOrdinal("E_CourseId"));

            if (HasColumn(r, "E_BatchId") && !r.IsDBNull(r.GetOrdinal("E_BatchId")))
                e.E_BatchId = r.GetGuid(r.GetOrdinal("E_BatchId"));

            if (HasColumn(r, "E_RollNumber") && !r.IsDBNull(r.GetOrdinal("E_RollNumber")))
                e.E_RollNumber = r.GetString(r.GetOrdinal("E_RollNumber"));

            if (HasColumn(r, "E_EnrollmentType") && !r.IsDBNull(r.GetOrdinal("E_EnrollmentType")))
                e.E_EnrollmentType = r.GetString(r.GetOrdinal("E_EnrollmentType"));

            if (HasColumn(r, "E_FeeStructureId") && !r.IsDBNull(r.GetOrdinal("E_FeeStructureId")))
                e.E_FeeStructureId = r.GetGuid(r.GetOrdinal("E_FeeStructureId"));

            if (HasColumn(r, "E_Remarks") && !r.IsDBNull(r.GetOrdinal("E_Remarks")))
                e.E_Remarks = r.GetString(r.GetOrdinal("E_Remarks"));

            // Display names
            if (HasColumn(r, "StudentName") && !r.IsDBNull(r.GetOrdinal("StudentName")))
                e.StudentName = r.GetString(r.GetOrdinal("StudentName"));

            if (HasColumn(r, "StudentAdmissionNumber") && !r.IsDBNull(r.GetOrdinal("StudentAdmissionNumber")))
                e.StudentAdmissionNumber = r.GetString(r.GetOrdinal("StudentAdmissionNumber"));

            if (HasColumn(r, "StudentCode") && !r.IsDBNull(r.GetOrdinal("StudentCode")))
                e.StudentCode = r.GetString(r.GetOrdinal("StudentCode"));

            if (HasColumn(r, "StudentPhone") && !r.IsDBNull(r.GetOrdinal("StudentPhone")))
                e.StudentPhone = r.GetString(r.GetOrdinal("StudentPhone"));

            if (HasColumn(r, "StudentEmail") && !r.IsDBNull(r.GetOrdinal("StudentEmail")))
                e.StudentEmail = r.GetString(r.GetOrdinal("StudentEmail"));

            if (HasColumn(r, "BranchName") && !r.IsDBNull(r.GetOrdinal("BranchName")))
                e.BranchName = r.GetString(r.GetOrdinal("BranchName"));

            if (HasColumn(r, "CourseName") && !r.IsDBNull(r.GetOrdinal("CourseName")))
                e.CourseName = r.GetString(r.GetOrdinal("CourseName"));

            if (HasColumn(r, "BatchName") && !r.IsDBNull(r.GetOrdinal("BatchName")))
                e.BatchName = r.GetString(r.GetOrdinal("BatchName"));

            if (HasColumn(r, "AcademicYearName") && !r.IsDBNull(r.GetOrdinal("AcademicYearName")))
                e.AcademicYearName = r.GetString(r.GetOrdinal("AcademicYearName"));

            return e;
        }

        private static bool HasColumn(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
