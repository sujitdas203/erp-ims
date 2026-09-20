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
    public class CourseSubjectDAL : ICourseSubjectDAL
    {
        private readonly DBHelper _dbHelper;

        public CourseSubjectDAL(DBHelper dbHelper) { _dbHelper = dbHelper; }

        public async Task<List<CourseSubject>> GetAllAsync(Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_CourseSubjects_CS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetAll";
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            var list = new List<CourseSubject>();
            while (await reader.ReadAsync()) list.Add(Map(reader));
            return list;
        }

        public async Task<List<CourseSubject>> GetByCourseIdAsync(Guid courseId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_CourseSubjects_CS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetByCourseId";
            cmd.Parameters.Add("@CS_CourseId", SqlDbType.UniqueIdentifier).Value = courseId;
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            var list = new List<CourseSubject>();
            while (await reader.ReadAsync()) list.Add(Map(reader));
            return list;
        }

        public async Task<CourseSubject?> GetByIdAsync(Guid courseId, Guid subjectId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_CourseSubjects_CS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetById";
            cmd.Parameters.Add("@CS_CourseId", SqlDbType.UniqueIdentifier).Value = courseId;
            cmd.Parameters.Add("@CS_SubjectId", SqlDbType.UniqueIdentifier).Value = subjectId;
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        public async Task<bool> ExistsAsync(Guid courseId, Guid subjectId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_CourseSubjects_CS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Exists";
            cmd.Parameters.Add("@CS_CourseId", SqlDbType.UniqueIdentifier).Value = courseId;
            cmd.Parameters.Add("@CS_SubjectId", SqlDbType.UniqueIdentifier).Value = subjectId;
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            var res = await cmd.ExecuteScalarAsync();
            return res != null && Convert.ToInt32(res) == 1;
        }

        public async Task<bool> SequenceExistsAsync(Guid courseId, int sequenceNo, Guid? excludeSubjectId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_CourseSubjects_CS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "SequenceExists";
            cmd.Parameters.Add("@CS_CourseId", SqlDbType.UniqueIdentifier).Value = courseId;
            cmd.Parameters.Add("@CS_SequenceNo", SqlDbType.Int).Value = sequenceNo;
            cmd.Parameters.Add("@ExcludeSubjectId", SqlDbType.UniqueIdentifier).Value = (object?)excludeSubjectId ?? DBNull.Value;
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            try
            {
                var res = await cmd.ExecuteScalarAsync();
                return res != null && Convert.ToInt32(res) == 1;
            }
            catch
            {
                // Direct query fallback if SP has not yet been executed
                using var fallbackCmd = conn.CreateCommand();
                fallbackCmd.CommandText = "SELECT CASE WHEN EXISTS (SELECT 1 FROM CourseSubjects_CS WHERE CS_CourseId = @cId AND CS_SequenceNo = @seq AND (@exId IS NULL OR CS_SubjectId <> @exId)) THEN 1 ELSE 0 END";
                fallbackCmd.Parameters.Add("@cId", SqlDbType.UniqueIdentifier).Value = courseId;
                fallbackCmd.Parameters.Add("@seq", SqlDbType.Int).Value = sequenceNo;
                fallbackCmd.Parameters.Add("@exId", SqlDbType.UniqueIdentifier).Value = (object?)excludeSubjectId ?? DBNull.Value;
                var res2 = await fallbackCmd.ExecuteScalarAsync();
                return res2 != null && Convert.ToInt32(res2) == 1;
            }
        }

        public async Task<int> GetNextSequenceNoAsync(Guid courseId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_CourseSubjects_CS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetNextSequence";
            cmd.Parameters.Add("@CS_CourseId", SqlDbType.UniqueIdentifier).Value = courseId;
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            try
            {
                var res = await cmd.ExecuteScalarAsync();
                return (res != null && res != DBNull.Value) ? Convert.ToInt32(res) : 1;
            }
            catch
            {
                // Fallback direct query
                using var fallbackCmd = conn.CreateCommand();
                fallbackCmd.CommandText = "SELECT ISNULL(MAX(CS_SequenceNo), 0) + 1 FROM CourseSubjects_CS WHERE CS_CourseId = @cId";
                fallbackCmd.Parameters.Add("@cId", SqlDbType.UniqueIdentifier).Value = courseId;
                var res2 = await fallbackCmd.ExecuteScalarAsync();
                return (res2 != null && res2 != DBNull.Value) ? Convert.ToInt32(res2) : 1;
            }
        }

        public async Task<bool> CreateAsync(CourseSubject cs)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_CourseSubjects_CS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Insert";
            AddParams(cmd, cs);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(CourseSubject cs, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_CourseSubjects_CS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Update";
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            AddParams(cmd, cs);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid courseId, Guid subjectId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_CourseSubjects_CS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Delete";
            cmd.Parameters.Add("@CS_CourseId", SqlDbType.UniqueIdentifier).Value = courseId;
            cmd.Parameters.Add("@CS_SubjectId", SqlDbType.UniqueIdentifier).Value = subjectId;
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        private void AddParams(SqlCommand cmd, CourseSubject cs)
        {
            cmd.Parameters.Add("@CS_CourseId", SqlDbType.UniqueIdentifier).Value = cs.CS_CourseId;
            cmd.Parameters.Add("@CS_SubjectId", SqlDbType.UniqueIdentifier).Value = cs.CS_SubjectId;
            cmd.Parameters.Add("@CS_SequenceNo", SqlDbType.Int).Value = cs.CS_SequenceNo;
            cmd.Parameters.Add("@CS_IsMandatory", SqlDbType.Bit).Value = cs.CS_IsMandatory;
            cmd.Parameters.Add("@CS_MaxMarks", SqlDbType.Decimal).Value = (object?)cs.CS_MaxMarks ?? DBNull.Value;
            cmd.Parameters.Add("@CS_PassMarks", SqlDbType.Decimal).Value = (object?)cs.CS_PassMarks ?? DBNull.Value;
        }

        private static CourseSubject Map(SqlDataReader r) => new()
        {
            CS_CourseId = r.GetGuid(r.GetOrdinal("CS_CourseId")),
            CS_SubjectId = r.GetGuid(r.GetOrdinal("CS_SubjectId")),
            CS_SequenceNo = r.GetInt32(r.GetOrdinal("CS_SequenceNo")),
            CS_IsMandatory = r.GetBoolean(r.GetOrdinal("CS_IsMandatory")),
            CS_MaxMarks = r["CS_MaxMarks"] as decimal?,
            CS_PassMarks = r["CS_PassMarks"] as decimal?,
            CourseName = r["CourseName"] as string ?? "-",
            SubjectName = r["SubjectName"] as string ?? "-"
        };
    }
}
