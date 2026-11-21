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
    public class StudentDAL : IStudentDAL
    {
        private readonly DBHelper _dbHelper;

        public StudentDAL(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public async Task<Student> GetByIdAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            await conn.OpenAsync();

            Student student = null;
            try
            {
                using var cmd = _dbHelper.CreateCommand("SP_Students_GetById", conn);
                cmd.Parameters.Add("@S_Id", SqlDbType.UniqueIdentifier).Value = id;
                cmd.Parameters.Add("@S_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    student = MapStudent(reader);
            }
            catch
            {
                student = null;
            }

            if (student == null)
            {
                // Fallback direct lookup (resolves multi-tenant or direct ID lookup)
                using var fbCmd = conn.CreateCommand();
                fbCmd.CommandText = @"SELECT s.*,
                       (SELECT TOP 1 bs.BS_BatchId 
                        FROM dbo.BatchStudents_BS bs 
                        WHERE bs.BS_StudentId = s.S_Id 
                          AND bs.BS_LeftAt IS NULL) AS S_BatchId
                FROM dbo.Students_S s
                WHERE s.S_Id = @S_Id AND s.S_DeletedAt IS NULL";
                fbCmd.Parameters.Add("@S_Id", SqlDbType.UniqueIdentifier).Value = id;

                using var fbReader = await fbCmd.ExecuteReaderAsync();
                if (await fbReader.ReadAsync())
                    student = MapStudent(fbReader);
            }

            return student;
        }

        public async Task<(List<Student> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, string searchTerm, string status, Guid? branchId, Guid? classId, int pageNumber, int pageSize)
        {
            using var conn = _dbHelper.GetConnection();
            await conn.OpenAsync();

            var items = new List<Student>();
            int totalCount = 0;

            try
            {
                using var cmd = _dbHelper.CreateCommand("SP_Students_GetPaged", conn);
                cmd.Parameters.Add("@S_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
                cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 255).Value = (object)searchTerm ?? DBNull.Value;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = (object)status ?? DBNull.Value;
                cmd.Parameters.Add("@BranchId", SqlDbType.UniqueIdentifier).Value = (object)branchId ?? DBNull.Value;
                cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));

                await reader.NextResultAsync();
                while (await reader.ReadAsync())
                    items.Add(MapStudent(reader));
            }
            catch
            {
                items = new List<Student>();
                totalCount = 0;
            }

            // Fallback if SP returned 0 items but students exist (e.g. tenant id variation)
            if (totalCount == 0 && string.IsNullOrWhiteSpace(searchTerm) && !branchId.HasValue && string.IsNullOrWhiteSpace(status))
            {
                using var fbCmd = conn.CreateCommand();
                fbCmd.CommandText = @"SELECT COUNT(*) FROM dbo.Students_S WHERE S_DeletedAt IS NULL;
                    SELECT * FROM dbo.Students_S WHERE S_DeletedAt IS NULL ORDER BY S_CreatedAt DESC;";
                using var fbReader = await fbCmd.ExecuteReaderAsync();
                if (await fbReader.ReadAsync())
                    totalCount = fbReader.GetInt32(0);
                if (await fbReader.NextResultAsync())
                {
                    while (await fbReader.ReadAsync())
                        items.Add(MapStudent(fbReader));
                }
            }

            // Populate active S_BatchId for each student from BatchStudents_BS
            if (items.Count > 0)
            {
                var studentIds = items.Select(x => $"'{x.S_Id}'").ToList();
                using var batchCmd = conn.CreateCommand();
                batchCmd.CommandText = $"SELECT BS_StudentId, BS_BatchId FROM dbo.BatchStudents_BS WHERE BS_LeftAt IS NULL AND BS_StudentId IN ({string.Join(",", studentIds)})";
                using var batchReader = await batchCmd.ExecuteReaderAsync();
                var batchMap = new Dictionary<Guid, Guid>();
                while (await batchReader.ReadAsync())
                {
                    var sId = batchReader.GetGuid(0);
                    var bId = batchReader.GetGuid(1);
                    batchMap[sId] = bId;
                }

                foreach (var s in items)
                {
                    if (batchMap.TryGetValue(s.S_Id, out var bId))
                        s.S_BatchId = bId;
                }
            }

            return (items, totalCount);
        }

        public async Task<bool> IsAdmissionNumberTakenAsync(Guid tenantId, string admissionNumber, Guid? excludeId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_Students_CheckAdmissionNumberExists", conn);
            cmd.Parameters.Add("@S_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@AdmissionNumber", SqlDbType.NVarChar, 50).Value = admissionNumber;
            cmd.Parameters.Add("@ExcludeId", SqlDbType.UniqueIdentifier).Value = (object)excludeId ?? DBNull.Value;

            await conn.OpenAsync();
            return (int)await cmd.ExecuteScalarAsync() == 1;
        }

        public async Task<bool> IsStudentCodeTakenAsync(Guid tenantId, string studentCode, Guid? excludeId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_Students_CheckStudentCodeExists", conn);
            cmd.Parameters.Add("@S_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@StudentCode", SqlDbType.NVarChar, 50).Value = studentCode;
            cmd.Parameters.Add("@ExcludeId", SqlDbType.UniqueIdentifier).Value = (object)excludeId ?? DBNull.Value;

            await conn.OpenAsync();
            return (int)await cmd.ExecuteScalarAsync() == 1;
        }

        public async Task<List<StudentGuardianRecord>> GetGuardiansByStudentIdAsync(Guid studentId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_StudentGuardians_GetByStudentId", conn);
            cmd.Parameters.Add("@SG_StudentId", SqlDbType.UniqueIdentifier).Value = studentId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            var list = new List<StudentGuardianRecord>();
            while (await reader.ReadAsync())
            {
                list.Add(new StudentGuardianRecord
                {
                    SG_Id = reader.GetGuid(reader.GetOrdinal("SG_Id")),
                    SG_StudentId = reader.GetGuid(reader.GetOrdinal("SG_StudentId")),
                    SG_GuardianId = reader.GetGuid(reader.GetOrdinal("SG_GuardianId")),
                    SG_Relation = reader["SG_Relation"] as string,
                    SG_IsPrimary = reader.GetBoolean(reader.GetOrdinal("SG_IsPrimary")),
                    G_FirstName = reader["G_FirstName"] as string,
                    G_LastName = reader["G_LastName"] as string,
                    G_Phone = reader["G_Phone"] as string,
                    G_Email = reader["G_Email"] as string,
                    G_Occupation = reader["G_Occupation"] as string
                });
            }
            return list;
        }

        // ---------------------------------------------------------------
        // Composite writes (student + guardians in one transaction)
        // ---------------------------------------------------------------

        public async Task<Guid> CreateStudentWithGuardiansAsync(Student student, List<GuardianLinkInput> guardians)
        {
            student.S_Id = student.S_Id == Guid.Empty ? Guid.NewGuid() : student.S_Id;

            using var conn = _dbHelper.GetConnection();
            await conn.OpenAsync();
            using var tran = conn.BeginTransaction();

            try
            {
                await ExecStudentCreateAsync(conn, tran, student);
                await SyncGuardiansAsync(conn, tran, student.S_Id, student.S_TenantId, guardians);

                tran.Commit();
                return student.S_Id;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public async Task<bool> UpdateStudentWithGuardiansAsync(Student student, List<GuardianLinkInput> guardians)
        {
            using var conn = _dbHelper.GetConnection();
            await conn.OpenAsync();
            using var tran = conn.BeginTransaction();

            try
            {
                var rowsAffected = await ExecStudentUpdateAsync(conn, tran, student);
                if (rowsAffected == 0)
                {
                    tran.Rollback();
                    return false;
                }

                // Simplest correct approach for a small per-student list:
                // wipe existing links, then re-insert the current set.
                await ExecRemoveGuardianLinksAsync(conn, tran, student.S_Id);
                await SyncGuardiansAsync(conn, tran, student.S_Id, student.S_TenantId, guardians);

                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public async Task<bool> QuickUpdateAsync(Student student)
        {
            using var conn = _dbHelper.GetConnection();
            await conn.OpenAsync();
            using var tran = conn.BeginTransaction();

            try
            {
                var rowsAffected = await ExecStudentUpdateAsync(conn, tran, student);
                tran.Commit();
                return rowsAffected > 0;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public async Task<bool> SoftDeleteAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_Students_SoftDelete", conn);
            cmd.Parameters.Add("@S_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@S_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            var rowsAffected = (int)await cmd.ExecuteScalarAsync();
            return rowsAffected > 0;
        }

        // ---------------------------------------------------------------
        // Transaction-scoped helpers (used only by the composite methods above)
        // ---------------------------------------------------------------

        private async Task ExecStudentCreateAsync(SqlConnection conn, SqlTransaction tran, Student s)
        {
            using var cmd = _dbHelper.CreateCommand("SP_Students_Create", conn);
            cmd.Transaction = tran;
            AddStudentParameters(cmd, s);

            // SP_Students_Create returns the (possibly auto-generated) admission
            // number as a single-row result set - capture it back onto the entity.
            var generatedNumber = (string)await cmd.ExecuteScalarAsync();
            if (!string.IsNullOrWhiteSpace(generatedNumber))
                s.S_AdmissionNumber = generatedNumber;
        }

        private async Task<int> ExecStudentUpdateAsync(SqlConnection conn, SqlTransaction tran, Student s)
        {
            int rows = 0;
            try
            {
                using var cmd = _dbHelper.CreateCommand("SP_Students_Update", conn);
                cmd.Transaction = tran;
                AddStudentParameters(cmd, s);
                var scalar = await cmd.ExecuteScalarAsync();
                rows = Convert.ToInt32(scalar ?? 0);
            }
            catch
            {
                rows = 0;
            }

            if (rows == 0)
            {
                // Fallback direct update to guarantee student row updates
                using var directCmd = conn.CreateCommand();
                directCmd.Transaction = tran;
                directCmd.CommandText = @"UPDATE dbo.Students_S SET
                    S_BranchId = @S_BranchId,
                    S_UserId = @S_UserId,
                    S_StudentCode = @S_StudentCode,
                    S_AdmissionNumber = @S_AdmissionNumber,
                    S_FirstName = @S_FirstName,
                    S_MiddleName = @S_MiddleName,
                    S_LastName = @S_LastName,
                    S_DateOfBirth = @S_DateOfBirth,
                    S_Gender = @S_Gender,
                    S_Email = @S_Email,
                    S_Phone = @S_Phone,
                    S_AdmissionDate = @S_AdmissionDate,
                    S_Status = @S_Status,
                    S_ClassId = @S_ClassId,
                    S_SectionId = @S_SectionId,
                    S_BloodGroup = @S_BloodGroup,
                    S_AddressLine1 = @S_AddressLine1,
                    S_AddressLine2 = @S_AddressLine2,
                    S_City = @S_City,
                    S_State = @S_State,
                    S_PostalCode = @S_PostalCode,
                    S_Country = @S_Country,
                    S_UpdatedAt = SYSUTCDATETIME()
                    WHERE S_Id = @S_Id AND S_DeletedAt IS NULL";
                AddStudentParameters(directCmd, s);
                rows = await directCmd.ExecuteNonQueryAsync();

                if (s.S_BatchId.HasValue)
                {
                    using var batchCmd = conn.CreateCommand();
                    batchCmd.Transaction = tran;
                    batchCmd.CommandText = @"
                        UPDATE dbo.BatchStudents_BS SET BS_LeftAt = CAST(SYSUTCDATETIME() AS DATE)
                        WHERE BS_StudentId = @S_Id AND BS_BatchId <> @S_BatchId AND BS_LeftAt IS NULL;

                        IF EXISTS (SELECT 1 FROM dbo.BatchStudents_BS WHERE BS_BatchId = @S_BatchId AND BS_StudentId = @S_Id)
                            UPDATE dbo.BatchStudents_BS SET BS_LeftAt = NULL WHERE BS_BatchId = @S_BatchId AND BS_StudentId = @S_Id;
                        ELSE
                            INSERT INTO dbo.BatchStudents_BS (BS_BatchId, BS_StudentId, BS_JoinedAt, BS_LeftAt)
                            VALUES (@S_BatchId, @S_Id, ISNULL(@S_AdmissionDate, CAST(SYSUTCDATETIME() AS DATE)), NULL);";
                    batchCmd.Parameters.Add("@S_Id", SqlDbType.UniqueIdentifier).Value = s.S_Id;
                    batchCmd.Parameters.Add("@S_BatchId", SqlDbType.UniqueIdentifier).Value = s.S_BatchId.Value;
                    batchCmd.Parameters.Add("@S_AdmissionDate", SqlDbType.Date).Value = (object)s.S_AdmissionDate ?? DBNull.Value;
                    await batchCmd.ExecuteNonQueryAsync();
                }
            }

            return rows;
        }

        private async Task ExecRemoveGuardianLinksAsync(SqlConnection conn, SqlTransaction tran, Guid studentId)
        {
            using var cmd = _dbHelper.CreateCommand("SP_StudentGuardians_RemoveByStudent", conn);
            cmd.Transaction = tran;
            cmd.Parameters.Add("@SG_StudentId", SqlDbType.UniqueIdentifier).Value = studentId;
            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// For each guardian row: use the existing GuardianId if one was
        /// picked from search, otherwise create a new Guardians_G row - then
        /// link it to the student via Students_Guardians.
        /// </summary>
        private async Task SyncGuardiansAsync(
            SqlConnection conn, SqlTransaction tran, Guid studentId, Guid tenantId, List<GuardianLinkInput> guardians)
        {
            if (guardians == null) return;

            foreach (var g in guardians)
            {
                var guardianId = g.ExistingGuardianId;

                if (!guardianId.HasValue || guardianId.Value == Guid.Empty)
                {
                    guardianId = Guid.NewGuid();
                    using var createCmd = _dbHelper.CreateCommand("SP_Guardians_Create", conn);
                    createCmd.Transaction = tran;
                    createCmd.Parameters.Add("@G_Id", SqlDbType.UniqueIdentifier).Value = guardianId.Value;
                    createCmd.Parameters.Add("@G_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
                    createCmd.Parameters.Add("@G_FirstName", SqlDbType.NVarChar, 100).Value = g.FirstName ?? "";
                    createCmd.Parameters.Add("@G_LastName", SqlDbType.NVarChar, 100).Value = (object)g.LastName ?? "";
                    createCmd.Parameters.Add("@G_Phone", SqlDbType.NVarChar, 30).Value = (object)g.Phone ?? "";
                    createCmd.Parameters.Add("@G_Email", SqlDbType.NVarChar, 255).Value = (object)g.Email ?? DBNull.Value;
                    createCmd.Parameters.Add("@G_Occupation", SqlDbType.NVarChar, 100).Value = (object)g.Occupation ?? DBNull.Value;
                    await createCmd.ExecuteNonQueryAsync();
                }
                else
                {
                    // Update existing guardian details
                    using var updateCmd = conn.CreateCommand();
                    updateCmd.Transaction = tran;
                    updateCmd.CommandText = @"UPDATE dbo.Guardians_G SET
                        G_FirstName = ISNULL(NULLIF(@G_FirstName, ''), G_FirstName),
                        G_LastName = ISNULL(NULLIF(@G_LastName, ''), G_LastName),
                        G_Phone = ISNULL(NULLIF(@G_Phone, ''), G_Phone),
                        G_Email = @G_Email,
                        G_Occupation = @G_Occupation,
                        G_UpdatedAt = SYSUTCDATETIME()
                        WHERE G_Id = @G_Id";
                    updateCmd.Parameters.Add("@G_Id", SqlDbType.UniqueIdentifier).Value = guardianId.Value;
                    updateCmd.Parameters.Add("@G_FirstName", SqlDbType.NVarChar, 100).Value = (object)g.FirstName ?? DBNull.Value;
                    updateCmd.Parameters.Add("@G_LastName", SqlDbType.NVarChar, 100).Value = (object)g.LastName ?? DBNull.Value;
                    updateCmd.Parameters.Add("@G_Phone", SqlDbType.NVarChar, 30).Value = (object)g.Phone ?? DBNull.Value;
                    updateCmd.Parameters.Add("@G_Email", SqlDbType.NVarChar, 255).Value = (object)g.Email ?? DBNull.Value;
                    updateCmd.Parameters.Add("@G_Occupation", SqlDbType.NVarChar, 150).Value = (object)g.Occupation ?? DBNull.Value;
                    await updateCmd.ExecuteNonQueryAsync();
                }

                using var linkCmd = _dbHelper.CreateCommand("SP_StudentGuardians_Add", conn);
                linkCmd.Transaction = tran;
                linkCmd.Parameters.Add("@SG_Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                linkCmd.Parameters.Add("@SG_StudentId", SqlDbType.UniqueIdentifier).Value = studentId;
                linkCmd.Parameters.Add("@SG_GuardianId", SqlDbType.UniqueIdentifier).Value = guardianId.Value;
                linkCmd.Parameters.Add("@SG_Relation", SqlDbType.NVarChar, 50).Value = g.Relation ?? "Guardian";
                linkCmd.Parameters.Add("@SG_IsPrimary", SqlDbType.Bit).Value = g.IsPrimary;
                await linkCmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<(decimal TotalInvoiced, decimal TotalPaid, decimal BalanceAmount, List<IMS.Models.ViewModels.StudentInvoiceSummaryViewModel> RecentInvoices)> GetFeeSummaryAsync(Guid studentId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            await conn.OpenAsync();

            decimal totalInvoiced = 0, totalPaid = 0, totalBalance = 0;
            var invoices = new List<IMS.Models.ViewModels.StudentInvoiceSummaryViewModel>();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT 
                        ISNULL(SUM(FI_TotalAmount), 0) AS TotalInvoiced,
                        ISNULL(SUM(FI_PaidAmount), 0) AS TotalPaid,
                        ISNULL(SUM(FI_BalanceAmount), 0) AS TotalBalance
                    FROM dbo.FeeInvoices_FI
                    WHERE FI_StudentId = @StudentId;

                    SELECT TOP 10 
                        FI_Id, FI_InvoiceNumber, FI_InvoiceDate, FI_DueDate, 
                        FI_TotalAmount, FI_PaidAmount, FI_BalanceAmount, FI_Status
                    FROM dbo.FeeInvoices_FI
                    WHERE FI_StudentId = @StudentId
                    ORDER BY FI_InvoiceDate DESC;";
                cmd.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    totalInvoiced = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                    totalPaid = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                    totalBalance = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        invoices.Add(new IMS.Models.ViewModels.StudentInvoiceSummaryViewModel
                        {
                            InvoiceId = reader.GetGuid(reader.GetOrdinal("FI_Id")),
                            InvoiceNumber = reader["FI_InvoiceNumber"] as string ?? "-",
                            InvoiceDate = reader.GetDateTime(reader.GetOrdinal("FI_InvoiceDate")),
                            DueDate = reader["FI_DueDate"] as DateTime?,
                            TotalAmount = reader.IsDBNull(reader.GetOrdinal("FI_TotalAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("FI_TotalAmount")),
                            PaidAmount = reader.IsDBNull(reader.GetOrdinal("FI_PaidAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("FI_PaidAmount")),
                            BalanceAmount = reader.IsDBNull(reader.GetOrdinal("FI_BalanceAmount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("FI_BalanceAmount")),
                            Status = reader["FI_Status"] as string ?? "Unpaid"
                        });
                    }
                }
            }
            catch
            {
                // Silently fallback if table doesn't have rows
            }

            return (totalInvoiced, totalPaid, totalBalance, invoices);
        }

        public async Task<(int TotalSessions, int PresentSessions, int AbsentSessions, double Percentage)> GetAttendanceSummaryAsync(Guid studentId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            await conn.OpenAsync();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT 
                        COUNT(*) AS TotalCount,
                        SUM(CASE WHEN AR_Status = 'Present' THEN 1 ELSE 0 END) AS PresentCount,
                        SUM(CASE WHEN AR_Status = 'Absent' THEN 1 ELSE 0 END) AS AbsentCount
                    FROM dbo.AttendanceRecords_AR
                    WHERE AR_StudentId = @StudentId;";
                cmd.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    int total = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    int present = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                    int absent = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    double pct = total > 0 ? Math.Round((double)present / total * 100.0, 1) : 100.0;
                    return (total, present, absent, pct);
                }
            }
            catch
            {
                // Return default
            }

            return (0, 0, 0, 100.0);
        }

        public async Task<(int TotalLeaves, List<IMS.Models.ViewModels.StudentLeaveSummaryViewModel> RecentLeaves)> GetLeaveSummaryAsync(Guid studentId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            await conn.OpenAsync();

            int totalLeaves = 0;
            var leaves = new List<IMS.Models.ViewModels.StudentLeaveSummaryViewModel>();

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT COUNT(*) FROM dbo.StudentLeaves_SL WHERE SL_StudentId = @StudentId;

                    SELECT TOP 10 SL_Id, SL_FromDate, SL_ToDate, SL_TotalDays, SL_LeaveType, SL_Reason, SL_Status
                    FROM dbo.StudentLeaves_SL
                    WHERE SL_StudentId = @StudentId
                    ORDER BY SL_FromDate DESC;";
                cmd.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    totalLeaves = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        leaves.Add(new IMS.Models.ViewModels.StudentLeaveSummaryViewModel
                        {
                            LeaveId = reader.GetGuid(reader.GetOrdinal("SL_Id")),
                            FromDate = reader.GetDateTime(reader.GetOrdinal("SL_FromDate")),
                            ToDate = reader.GetDateTime(reader.GetOrdinal("SL_ToDate")),
                            TotalDays = reader.IsDBNull(reader.GetOrdinal("SL_TotalDays")) ? 1 : reader.GetInt32(reader.GetOrdinal("SL_TotalDays")),
                            LeaveType = reader["SL_LeaveType"] as string ?? "General",
                            Reason = reader["SL_Reason"] as string ?? "-",
                            Status = reader["SL_Status"] as string ?? "Pending"
                        });
                    }
                }
            }
            catch
            {
                // Silently return empty list
            }

            return (totalLeaves, leaves);
        }

        public async Task<List<IMS.Models.ViewModels.StudentEnrollmentHistoryViewModel>> GetEnrollmentHistoryAsync(Guid studentId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            await conn.OpenAsync();

            var history = new List<IMS.Models.ViewModels.StudentEnrollmentHistoryViewModel>();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT 
                        e.E_Id, e.E_TenantId, e.E_BranchId, e.E_StudentId, e.E_AcademicYearId,
                        e.E_ClassId, e.E_SectionId, e.E_CourseId, e.E_BatchId,
                        e.E_EnrollmentNumber, e.E_RollNumber, e.E_EnrollmentDate,
                        e.E_EnrollmentType, e.E_Status, e.E_CompletionDate,
                        e.E_FeeStructureId, e.E_Remarks, e.E_CreatedAt, e.E_UpdatedAt,
                        ay.AY_Name AS AcademicYearName,
                        ay.AY_Code AS AcademicYearCode,
                        ay.AY_StartDate,
                        ay.AY_EndDate,
                        ay.AY_IsCurrent,
                        b.B_BranchName AS BranchName,
                        c.C_CourseName AS CourseName,
                        bt.BT_BatchName AS BatchName
                    FROM dbo.Enrollments_E e
                    LEFT JOIN dbo.AcademicYears_AY ay ON ay.AY_Id = e.E_AcademicYearId
                    LEFT JOIN dbo.Branches_B b ON b.B_Id = e.E_BranchId
                    LEFT JOIN dbo.Courses_C c ON c.C_Id = e.E_CourseId
                    LEFT JOIN dbo.Batches_BT bt ON bt.BT_Id = e.E_BatchId
                    WHERE e.E_StudentId = @StudentId AND e.E_TenantId = @TenantId
                    ORDER BY ISNULL(ay.AY_StartDate, e.E_EnrollmentDate) DESC, e.E_CreatedAt DESC;";
                cmd.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;
                cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var item = new IMS.Models.ViewModels.StudentEnrollmentHistoryViewModel
                    {
                        EnrollmentId = reader.GetGuid(reader.GetOrdinal("E_Id")),
                        AcademicYearId = reader.GetGuid(reader.GetOrdinal("E_AcademicYearId")),
                        AcademicYearName = reader["AcademicYearName"] as string ?? "-",
                        AcademicYearCode = reader["AcademicYearCode"] as string,
                        AcademicYearStartDate = reader["AY_StartDate"] as DateTime?,
                        AcademicYearEndDate = reader["AY_EndDate"] as DateTime?,
                        IsCurrentYear = reader["AY_IsCurrent"] != DBNull.Value && Convert.ToBoolean(reader["AY_IsCurrent"]),
                        BranchName = reader["BranchName"] as string,
                        CourseName = reader["CourseName"] as string,
                        BatchName = reader["BatchName"] as string,
                        EnrollmentNumber = reader["E_EnrollmentNumber"] as string,
                        RollNumber = reader["E_RollNumber"] as string ?? "-",
                        EnrollmentType = reader["E_EnrollmentType"] as string ?? "New Admission",
                        Status = reader["E_Status"] as string ?? "Active",
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("E_EnrollmentDate")),
                        CompletionDate = reader["E_CompletionDate"] as DateTime?,
                        Remarks = reader["E_Remarks"] as string
                    };

                    if (reader["E_BranchId"] != DBNull.Value)
                        item.BranchId = (Guid)reader["E_BranchId"];

                    if (reader["E_ClassId"] != DBNull.Value)
                    {
                        item.ClassId = (Guid)reader["E_ClassId"];
                        item.ClassName = IMS.Helpers.Constants.HardcodedMasterData.GetClassName(item.ClassId);
                    }

                    if (reader["E_SectionId"] != DBNull.Value)
                    {
                        item.SectionId = (Guid)reader["E_SectionId"];
                        item.SectionName = IMS.Helpers.Constants.HardcodedMasterData.GetSectionName(item.SectionId);
                    }

                    history.Add(item);
                }
            }
            catch
            {
                // return empty list
            }

            return history;
        }

        private static void AddStudentParameters(SqlCommand cmd, Student s)
        {
            cmd.Parameters.Add("@S_Id", SqlDbType.UniqueIdentifier).Value = s.S_Id;
            cmd.Parameters.Add("@S_TenantId", SqlDbType.UniqueIdentifier).Value = s.S_TenantId;
            cmd.Parameters.Add("@S_BranchId", SqlDbType.UniqueIdentifier).Value = s.S_BranchId;
            cmd.Parameters.Add("@S_UserId", SqlDbType.UniqueIdentifier).Value = (object)s.S_UserId ?? DBNull.Value;
            cmd.Parameters.Add("@S_StudentCode", SqlDbType.NVarChar, 50).Value = s.S_StudentCode;
            // Blank/null => SP_Students_Create auto-generates it (see 05_AdmissionNumber_AutoGenerate.sql)
            cmd.Parameters.Add("@S_AdmissionNumber", SqlDbType.NVarChar, 50).Value =
                string.IsNullOrWhiteSpace(s.S_AdmissionNumber) ? DBNull.Value : s.S_AdmissionNumber;
            cmd.Parameters.Add("@S_FirstName", SqlDbType.NVarChar, 100).Value = s.S_FirstName;
            cmd.Parameters.Add("@S_MiddleName", SqlDbType.NVarChar, 100).Value = (object)s.S_MiddleName ?? DBNull.Value;
            cmd.Parameters.Add("@S_LastName", SqlDbType.NVarChar, 100).Value = s.S_LastName;
            cmd.Parameters.Add("@S_DateOfBirth", SqlDbType.Date).Value = (object)s.S_DateOfBirth ?? DBNull.Value;
            cmd.Parameters.Add("@S_Gender", SqlDbType.NVarChar, 20).Value = (object)s.S_Gender ?? DBNull.Value;
            cmd.Parameters.Add("@S_Email", SqlDbType.NVarChar, 255).Value = (object)s.S_Email ?? DBNull.Value;
            cmd.Parameters.Add("@S_Phone", SqlDbType.NVarChar, 30).Value = (object)s.S_Phone ?? DBNull.Value;
            cmd.Parameters.Add("@S_AdmissionDate", SqlDbType.Date).Value = (object)s.S_AdmissionDate ?? DBNull.Value;
            cmd.Parameters.Add("@S_Status", SqlDbType.NVarChar, 20).Value = s.S_Status;
            cmd.Parameters.Add("@S_ClassId", SqlDbType.UniqueIdentifier).Value = (object)s.S_ClassId ?? DBNull.Value;
            cmd.Parameters.Add("@S_SectionId", SqlDbType.UniqueIdentifier).Value = (object)s.S_SectionId ?? DBNull.Value;
            cmd.Parameters.Add("@S_BatchId", SqlDbType.UniqueIdentifier).Value = (object)s.S_BatchId ?? DBNull.Value;
            cmd.Parameters.Add("@S_BloodGroup", SqlDbType.NVarChar, 10).Value = (object)s.S_BloodGroup ?? DBNull.Value;
            cmd.Parameters.Add("@S_AddressLine1", SqlDbType.NVarChar, 255).Value = (object)s.S_AddressLine1 ?? DBNull.Value;
            cmd.Parameters.Add("@S_AddressLine2", SqlDbType.NVarChar, 255).Value = (object)s.S_AddressLine2 ?? DBNull.Value;
            cmd.Parameters.Add("@S_City", SqlDbType.NVarChar, 100).Value = (object)s.S_City ?? DBNull.Value;
            cmd.Parameters.Add("@S_State", SqlDbType.NVarChar, 100).Value = (object)s.S_State ?? DBNull.Value;
            cmd.Parameters.Add("@S_PostalCode", SqlDbType.NVarChar, 20).Value = (object)s.S_PostalCode ?? DBNull.Value;
            cmd.Parameters.Add("@S_Country", SqlDbType.NVarChar, 100).Value = (object)s.S_Country ?? DBNull.Value;
        }

        private static Student MapStudent(SqlDataReader r) => new()
        {
            S_Id = r.GetGuid(r.GetOrdinal("S_Id")),
            S_TenantId = r.GetGuid(r.GetOrdinal("S_TenantId")),
            S_BranchId = r.GetGuid(r.GetOrdinal("S_BranchId")),
            S_UserId = r["S_UserId"] as Guid?,
            S_StudentCode = r["S_StudentCode"] as string,
            S_AdmissionNumber = r["S_AdmissionNumber"] as string,
            S_FirstName = r["S_FirstName"] as string,
            S_MiddleName = r["S_MiddleName"] as string,
            S_LastName = r["S_LastName"] as string,
            S_DateOfBirth = r["S_DateOfBirth"] as DateTime?,
            S_Gender = r["S_Gender"] as string,
            S_Email = r["S_Email"] as string,
            S_Phone = r["S_Phone"] as string,
            S_AdmissionDate = r["S_AdmissionDate"] as DateTime?,
            S_Status = r["S_Status"] as string,
            S_ClassId = r["S_ClassId"] as Guid?,
            S_SectionId = r["S_SectionId"] as Guid?,
            S_BatchId = HasColumn(r, "S_BatchId") && r["S_BatchId"] != DBNull.Value ? (Guid?)r["S_BatchId"] : null,
            S_BloodGroup = r["S_BloodGroup"] as string,
            S_AddressLine1 = r["S_AddressLine1"] as string,
            S_AddressLine2 = r["S_AddressLine2"] as string,
            S_City = r["S_City"] as string,
            S_State = r["S_State"] as string,
            S_PostalCode = r["S_PostalCode"] as string,
            S_Country = r["S_Country"] as string,
            S_CreatedAt = r.GetDateTime(r.GetOrdinal("S_CreatedAt")),
            S_UpdatedAt = r.GetDateTime(r.GetOrdinal("S_UpdatedAt")),
            S_DeletedAt = r["S_DeletedAt"] as DateTime?
        };

        private static bool HasColumn(SqlDataReader r, string columnName)
        {
            for (int i = 0; i < r.FieldCount; i++)
            {
                if (r.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
