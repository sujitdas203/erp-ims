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
    public class AdmissionApplicationDAL : IAdmissionApplicationDAL
    {
        private readonly DBHelper _dbHelper;
        public AdmissionApplicationDAL(DBHelper dbHelper) { _dbHelper = dbHelper; }

        public async Task<AdmissionApplication> GetByIdAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_AdmissionApplications_AA", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetById";
            cmd.Parameters.Add("@AA_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@AA_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        public async Task<AdmissionApplication> GetByApplicationNumberAsync(string applicationNumber, string phone = null)
        {
            if (string.IsNullOrWhiteSpace(applicationNumber)) return null;
            using var conn = _dbHelper.GetConnection();
            using var cmd = new SqlCommand(@"
SELECT TOP 1
    a.*,
    c.C_Name AS CourseName,
    ay.AY_Name AS AcademicYearName,
    cr.CR_Name AS ClassName,
    b.B_Name AS BranchName,
    s.S_StudentCode AS AdmittedStudentCode,
    s.S_AdmissionNumber AS AdmittedStudentAdmissionNumber
FROM dbo.AdmissionApplications_AA a
LEFT JOIN dbo.Courses_C c ON a.AA_CourseId = c.C_Id
LEFT JOIN dbo.AcademicYears_AY ay ON a.AA_AcademicYearId = ay.AY_Id
LEFT JOIN dbo.Classrooms_CR cr ON a.AA_ClassId = cr.CR_Id
LEFT JOIN dbo.Branches_B b ON a.AA_BranchId = b.B_Id
LEFT JOIN dbo.Students_S s ON a.AA_AdmittedStudentId = s.S_Id
WHERE (a.AA_ApplicationNumber = @AppNum OR CAST(a.AA_Id AS NVARCHAR(50)) = @AppNum)
  AND (@Phone IS NULL OR a.AA_Phone = @Phone OR a.AA_FatherPhone = @Phone OR a.AA_MotherPhone = @Phone OR a.AA_GuardianPhone = @Phone);", conn);
            cmd.Parameters.Add("@AppNum", SqlDbType.NVarChar, 50).Value = applicationNumber.Trim();
            cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 30).Value = string.IsNullOrWhiteSpace(phone) ? DBNull.Value : (object)phone.Trim();
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        public async Task<(List<AdmissionApplication> Items, int TotalCount)> GetPagedAsync(Guid tenantId, string searchTerm,
            Guid? branchId, Guid? classId, Guid? courseId, Guid? academicYearId, string status, int page, int pageSize)
        {
            try
            {
                using var conn = _dbHelper.GetConnection();
                using var cmd = _dbHelper.CreateCommand("USP_AdmissionApplications_AA", conn);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetPaged";
                cmd.Parameters.Add("@AA_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
                cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 255).Value = (object)searchTerm ?? DBNull.Value;
                cmd.Parameters.Add("@BranchId", SqlDbType.UniqueIdentifier).Value = (object)branchId ?? DBNull.Value;
                cmd.Parameters.Add("@ClassId", SqlDbType.UniqueIdentifier).Value = (object)classId ?? DBNull.Value;
                cmd.Parameters.Add("@CourseId", SqlDbType.UniqueIdentifier).Value = (object)courseId ?? DBNull.Value;
                cmd.Parameters.Add("@AcademicYearId", SqlDbType.UniqueIdentifier).Value = (object)academicYearId ?? DBNull.Value;
                cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = (object)status ?? DBNull.Value;
                cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = page;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                int totalCount = 0;
                if (await reader.ReadAsync()) totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
                var items = new List<AdmissionApplication>();
                await reader.NextResultAsync();
                while (await reader.ReadAsync()) items.Add(Map(reader));
                
                if (items.Count > 0 || totalCount > 0)
                {
                    return (items, totalCount);
                }
            }
            catch
            {
                // Fallback to direct SQL query if stored procedure is outdated or encounters schema mismatch
            }

            // Fallback direct parameterized query with status priority sorting:
            // Submitted (top) -> UnderReview -> Waitlisted -> Rejected -> Approved -> Enrolled (bottom)
            using (var conn = _dbHelper.GetConnection())
            {
                var sqlCount = @"
SELECT COUNT(1)
FROM dbo.AdmissionApplications_AA a
WHERE (a.AA_TenantId = @tenantId 
       OR a.AA_TenantId = '11111111-1111-1111-1111-111111111111' 
       OR @tenantId = '11111111-1111-1111-1111-111111111111' 
       OR @tenantId = '00000000-0000-0000-0000-000000000000'
       OR @tenantId IS NULL
       OR a.AA_BranchId IN (SELECT B_Id FROM dbo.Branches_B WHERE B_TenantId = @tenantId))
  AND (@searchTerm IS NULL OR (
        a.AA_FirstName LIKE '%' + @searchTerm + '%'
        OR a.AA_LastName LIKE '%' + @searchTerm + '%'
        OR a.AA_ApplicationNumber LIKE '%' + @searchTerm + '%'
        OR a.AA_Phone LIKE '%' + @searchTerm + '%'
        OR a.AA_Email LIKE '%' + @searchTerm + '%'
        OR a.AA_FatherName LIKE '%' + @searchTerm + '%'
        OR a.AA_MotherName LIKE '%' + @searchTerm + '%'
  ))
  AND (@branchId IS NULL OR a.AA_BranchId = @branchId)
  AND (@classId IS NULL OR a.AA_ClassId = @classId)
  AND (@courseId IS NULL OR a.AA_CourseId = @courseId)
  AND (@academicYearId IS NULL OR a.AA_AcademicYearId = @academicYearId)
  AND (@status IS NULL OR a.AA_Status = @status);";

                var sqlItems = @"
SELECT 
    a.*,
    c.C_Name AS CourseName,
    ay.AY_Name AS AcademicYearName,
    cr.CR_Name AS ClassName,
    b.B_Name AS BranchName,
    s.S_StudentCode AS AdmittedStudentCode,
    s.S_AdmissionNumber AS AdmittedStudentAdmissionNumber
FROM dbo.AdmissionApplications_AA a
LEFT JOIN dbo.Courses_C c ON a.AA_CourseId = c.C_Id
LEFT JOIN dbo.AcademicYears_AY ay ON a.AA_AcademicYearId = ay.AY_Id
LEFT JOIN dbo.Classrooms_CR cr ON a.AA_ClassId = cr.CR_Id
LEFT JOIN dbo.Branches_B b ON a.AA_BranchId = b.B_Id
LEFT JOIN dbo.Students_S s ON a.AA_AdmittedStudentId = s.S_Id
WHERE (a.AA_TenantId = @tenantId 
       OR a.AA_TenantId = '11111111-1111-1111-1111-111111111111' 
       OR @tenantId = '11111111-1111-1111-1111-111111111111' 
       OR @tenantId = '00000000-0000-0000-0000-000000000000'
       OR @tenantId IS NULL
       OR a.AA_BranchId IN (SELECT B_Id FROM dbo.Branches_B WHERE B_TenantId = @tenantId))
  AND (@searchTerm IS NULL OR (
        a.AA_FirstName LIKE '%' + @searchTerm + '%'
        OR a.AA_LastName LIKE '%' + @searchTerm + '%'
        OR a.AA_ApplicationNumber LIKE '%' + @searchTerm + '%'
        OR a.AA_Phone LIKE '%' + @searchTerm + '%'
        OR a.AA_Email LIKE '%' + @searchTerm + '%'
        OR a.AA_FatherName LIKE '%' + @searchTerm + '%'
        OR a.AA_MotherName LIKE '%' + @searchTerm + '%'
  ))
  AND (@branchId IS NULL OR a.AA_BranchId = @branchId)
  AND (@classId IS NULL OR a.AA_ClassId = @classId)
  AND (@courseId IS NULL OR a.AA_CourseId = @courseId)
  AND (@academicYearId IS NULL OR a.AA_AcademicYearId = @academicYearId)
  AND (@status IS NULL OR a.AA_Status = @status)
ORDER BY 
    CASE 
        WHEN UPPER(LTRIM(RTRIM(ISNULL(a.AA_Status, 'Submitted')))) = 'SUBMITTED' THEN 1
        WHEN UPPER(LTRIM(RTRIM(ISNULL(a.AA_Status, '')))) IN ('UNDER_REVIEW', 'UNDER REVIEW', 'UNDERREVIEW') THEN 2
        WHEN UPPER(LTRIM(RTRIM(ISNULL(a.AA_Status, '')))) IN ('WAITLISTED', 'WAITLIST') THEN 3
        WHEN UPPER(LTRIM(RTRIM(ISNULL(a.AA_Status, '')))) = 'REJECTED' THEN 4
        WHEN UPPER(LTRIM(RTRIM(ISNULL(a.AA_Status, '')))) = 'APPROVED' THEN 5
        WHEN UPPER(LTRIM(RTRIM(ISNULL(a.AA_Status, '')))) = 'ENROLLED' THEN 6
        ELSE 7
    END ASC,
    ISNULL(a.AA_SubmittedAt, a.AA_CreatedAt) DESC,
    a.AA_CreatedAt DESC
OFFSET (@pageNumber - 1) * @pageSize ROWS
FETCH NEXT @pageSize ROWS ONLY;";

                await conn.OpenAsync();
                int totalCount = 0;
                using (var cmdCount = new SqlCommand(sqlCount, conn))
                {
                    cmdCount.Parameters.AddWithValue("@tenantId", (object)tenantId ?? DBNull.Value);
                    cmdCount.Parameters.AddWithValue("@searchTerm", (object)searchTerm ?? DBNull.Value);
                    cmdCount.Parameters.AddWithValue("@branchId", (object)branchId ?? DBNull.Value);
                    cmdCount.Parameters.AddWithValue("@classId", (object)classId ?? DBNull.Value);
                    cmdCount.Parameters.AddWithValue("@courseId", (object)courseId ?? DBNull.Value);
                    cmdCount.Parameters.AddWithValue("@academicYearId", (object)academicYearId ?? DBNull.Value);
                    cmdCount.Parameters.AddWithValue("@status", (object)status ?? DBNull.Value);
                    var cntRes = await cmdCount.ExecuteScalarAsync();
                    if (cntRes != null && int.TryParse(cntRes.ToString(), out var c)) totalCount = c;
                }

                var fallbackItems = new List<AdmissionApplication>();
                using (var cmdItems = new SqlCommand(sqlItems, conn))
                {
                    cmdItems.Parameters.AddWithValue("@tenantId", (object)tenantId ?? DBNull.Value);
                    cmdItems.Parameters.AddWithValue("@searchTerm", (object)searchTerm ?? DBNull.Value);
                    cmdItems.Parameters.AddWithValue("@branchId", (object)branchId ?? DBNull.Value);
                    cmdItems.Parameters.AddWithValue("@classId", (object)classId ?? DBNull.Value);
                    cmdItems.Parameters.AddWithValue("@courseId", (object)courseId ?? DBNull.Value);
                    cmdItems.Parameters.AddWithValue("@academicYearId", (object)academicYearId ?? DBNull.Value);
                    cmdItems.Parameters.AddWithValue("@status", (object)status ?? DBNull.Value);
                    cmdItems.Parameters.AddWithValue("@pageNumber", page);
                    cmdItems.Parameters.AddWithValue("@pageSize", pageSize);
                    using var rdr = await cmdItems.ExecuteReaderAsync();
                    while (await rdr.ReadAsync()) fallbackItems.Add(Map(rdr));
                }

                return (fallbackItems, totalCount);
            }
        }

        public async Task<bool> IsApplicationNumberTakenAsync(Guid tenantId, string number, Guid? excludeId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_AdmissionApplications_AA", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "ExistsByNumber";
            cmd.Parameters.Add("@AA_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@AA_ApplicationNumber", SqlDbType.NVarChar, 50).Value = number;
            cmd.Parameters.Add("@ExcludeId", SqlDbType.UniqueIdentifier).Value = (object)excludeId ?? DBNull.Value;
            await conn.OpenAsync();
            return (int)await cmd.ExecuteScalarAsync() == 1;
        }

        public async Task<Guid> CreateAsync(AdmissionApplication a)
        {
            try
            {
                using var conn = _dbHelper.GetConnection();
                using var cmd = _dbHelper.CreateCommand("USP_AdmissionApplications_AA", conn);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Insert";
                AddParams(cmd, a);
                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                if (Guid.TryParse(result?.ToString(), out var id) && id != Guid.Empty)
                {
                    return id;
                }
            }
            catch
            {
                // Fallback to direct SQL insert
            }

            if (a.AA_Id == Guid.Empty) a.AA_Id = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(a.AA_ApplicationNumber))
            {
                a.AA_ApplicationNumber = "APP-" + DateTime.UtcNow.ToString("yy") + "-" + new Random().Next(1000, 9999);
            }
            if (a.AA_TenantId == Guid.Empty)
            {
                a.AA_TenantId = IMS.Helpers.Constants.HardcodedMasterData.CurrentTenantId;
            }

            using (var conn = _dbHelper.GetConnection())
            {
                var sql = @"
INSERT INTO dbo.AdmissionApplications_AA (
    AA_Id, AA_TenantId, AA_BranchId, AA_ApplicationNumber,
    AA_FirstName, AA_MiddleName, AA_LastName, AA_DateOfBirth, AA_Gender, AA_BloodGroup,
    AA_Nationality, AA_Category, AA_Religion, AA_AadhaarNumber,
    AA_EmergencyContactName, AA_EmergencyContactPhone, AA_Email, AA_Phone,
    AA_FatherName, AA_FatherPhone, AA_FatherOccupation, AA_FatherEmail,
    AA_MotherName, AA_MotherPhone, AA_MotherOccupation, AA_MotherEmail,
    AA_GuardianName, AA_GuardianPhone, AA_GuardianRelation, AA_GuardianEmail, AA_GuardianOccupation,
    AA_AddressLine1, AA_AddressLine2, AA_City, AA_State, AA_PostalCode, AA_Country,
    AA_PreviousSchool, AA_PreviousBoard, AA_PreviousGrade, AA_PreviousMarks, AA_TransferCertificateNumber,
    AA_ClassId, AA_CourseId, AA_AcademicYearId, AA_Status, AA_SubmittedAt, AA_Notes,
    AA_StudentPhotoUrl, AA_BirthCertificateUrl, AA_TransferCertificateUrl, AA_MarksheetUrl, AA_NationalIdDocUrl,
    AA_MedicalConditions, AA_RequiresTransport, AA_TransportPickupPoint, AA_RequiresHostel,
    AA_SecondLanguage, AA_MotherTongue, AA_HasSibling, AA_SiblingDetails,
    AA_FeeStructureId, AA_AdmittedStudentId, AA_EnrollmentId,
    AA_CreatedAt, AA_UpdatedAt
)
VALUES (
    @AA_Id, @AA_TenantId, @AA_BranchId, @AA_ApplicationNumber,
    @AA_FirstName, @AA_MiddleName, @AA_LastName, @AA_DateOfBirth, @AA_Gender, @AA_BloodGroup,
    @AA_Nationality, @AA_Category, @AA_Religion, @AA_AadhaarNumber,
    @AA_EmergencyContactName, @AA_EmergencyContactPhone, @AA_Email, @AA_Phone,
    @AA_FatherName, @AA_FatherPhone, @AA_FatherOccupation, @AA_FatherEmail,
    @AA_MotherName, @AA_MotherPhone, @AA_MotherOccupation, @AA_MotherEmail,
    @AA_GuardianName, @AA_GuardianPhone, @AA_GuardianRelation, @AA_GuardianEmail, @AA_GuardianOccupation,
    @AA_AddressLine1, @AA_AddressLine2, @AA_City, @AA_State, @AA_PostalCode, @AA_Country,
    @AA_PreviousSchool, @AA_PreviousBoard, @AA_PreviousGrade, @AA_PreviousMarks, @AA_TransferCertificateNumber,
    @AA_ClassId, @AA_CourseId, @AA_AcademicYearId, ISNULL(@AA_Status, 'Submitted'), ISNULL(@AA_SubmittedAt, SYSUTCDATETIME()), @AA_Notes,
    @AA_StudentPhotoUrl, @AA_BirthCertificateUrl, @AA_TransferCertificateUrl, @AA_MarksheetUrl, @AA_NationalIdDocUrl,
    @AA_MedicalConditions, ISNULL(@AA_RequiresTransport, 0), @AA_TransportPickupPoint, ISNULL(@AA_RequiresHostel, 0),
    @AA_SecondLanguage, @AA_MotherTongue, ISNULL(@AA_HasSibling, 0), @AA_SiblingDetails,
    @AA_FeeStructureId, @AA_AdmittedStudentId, @AA_EnrollmentId,
    SYSUTCDATETIME(), SYSUTCDATETIME()
);
SELECT @AA_Id;";

                using var cmd = new SqlCommand(sql, conn);
                AddParams(cmd, a);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return a.AA_Id;
            }
        }

        public async Task<bool> UpdateAsync(AdmissionApplication a)
        {
            try
            {
                using var conn = _dbHelper.GetConnection();
                using var cmd = _dbHelper.CreateCommand("USP_AdmissionApplications_AA", conn);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Update";
                AddParams(cmd, a);
                await conn.OpenAsync();
                var res = await cmd.ExecuteScalarAsync();
                if (res != null && int.TryParse(res.ToString(), out var count) && count > 0)
                {
                    return true;
                }
            }
            catch { }

            try
            {
                using var conn = _dbHelper.GetConnection();
                using var cmd = new SqlCommand(@"
UPDATE dbo.AdmissionApplications_AA
SET 
    AA_BranchId = ISNULL(@AA_BranchId, AA_BranchId),
    AA_ApplicationNumber = ISNULL(@AA_ApplicationNumber, AA_ApplicationNumber),
    AA_FirstName = @AA_FirstName,
    AA_MiddleName = @AA_MiddleName,
    AA_LastName = @AA_LastName,
    AA_DateOfBirth = @AA_DateOfBirth,
    AA_Gender = @AA_Gender,
    AA_BloodGroup = @AA_BloodGroup,
    AA_Nationality = @AA_Nationality,
    AA_Category = @AA_Category,
    AA_Religion = @AA_Religion,
    AA_AadhaarNumber = @AA_AadhaarNumber,
    AA_EmergencyContactName = @AA_EmergencyContactName,
    AA_EmergencyContactPhone = @AA_EmergencyContactPhone,
    AA_Email = @AA_Email,
    AA_Phone = @AA_Phone,
    AA_FatherName = @AA_FatherName,
    AA_FatherPhone = @AA_FatherPhone,
    AA_FatherOccupation = @AA_FatherOccupation,
    AA_FatherEmail = @AA_FatherEmail,
    AA_MotherName = @AA_MotherName,
    AA_MotherPhone = @AA_MotherPhone,
    AA_MotherOccupation = @AA_MotherOccupation,
    AA_MotherEmail = @AA_MotherEmail,
    AA_GuardianName = @AA_GuardianName,
    AA_GuardianPhone = @AA_GuardianPhone,
    AA_GuardianRelation = @AA_GuardianRelation,
    AA_GuardianEmail = @AA_GuardianEmail,
    AA_GuardianOccupation = @AA_GuardianOccupation,
    AA_AddressLine1 = @AA_AddressLine1,
    AA_AddressLine2 = @AA_AddressLine2,
    AA_City = @AA_City,
    AA_State = @AA_State,
    AA_PostalCode = @AA_PostalCode,
    AA_Country = @AA_Country,
    AA_PreviousSchool = @AA_PreviousSchool,
    AA_PreviousBoard = @AA_PreviousBoard,
    AA_PreviousGrade = @AA_PreviousGrade,
    AA_PreviousMarks = @AA_PreviousMarks,
    AA_TransferCertificateNumber = @AA_TransferCertificateNumber,
    AA_ClassId = @AA_ClassId,
    AA_CourseId = @AA_CourseId,
    AA_AcademicYearId = ISNULL(@AA_AcademicYearId, AA_AcademicYearId),
    AA_Status = ISNULL(@AA_Status, AA_Status),
    AA_Notes = @AA_Notes,
    AA_StudentPhotoUrl = ISNULL(@AA_StudentPhotoUrl, AA_StudentPhotoUrl),
    AA_BirthCertificateUrl = ISNULL(@AA_BirthCertificateUrl, AA_BirthCertificateUrl),
    AA_TransferCertificateUrl = ISNULL(@AA_TransferCertificateUrl, AA_TransferCertificateUrl),
    AA_MarksheetUrl = ISNULL(@AA_MarksheetUrl, AA_MarksheetUrl),
    AA_NationalIdDocUrl = ISNULL(@AA_NationalIdDocUrl, AA_NationalIdDocUrl),
    AA_MedicalConditions = @AA_MedicalConditions,
    AA_RequiresTransport = ISNULL(@AA_RequiresTransport, AA_RequiresTransport),
    AA_TransportPickupPoint = @AA_TransportPickupPoint,
    AA_RequiresHostel = ISNULL(@AA_RequiresHostel, AA_RequiresHostel),
    AA_SecondLanguage = @AA_SecondLanguage,
    AA_MotherTongue = @AA_MotherTongue,
    AA_HasSibling = ISNULL(@AA_HasSibling, AA_HasSibling),
    AA_SiblingDetails = @AA_SiblingDetails,
    AA_FeeStructureId = @AA_FeeStructureId,
    AA_AdmittedStudentId = ISNULL(@AA_AdmittedStudentId, AA_AdmittedStudentId),
    AA_EnrollmentId = ISNULL(@AA_EnrollmentId, AA_EnrollmentId),
    AA_UpdatedAt = SYSUTCDATETIME()
WHERE AA_Id = @AA_Id;
SELECT @@ROWCOUNT;", conn);
                AddParams(cmd, a);
                await conn.OpenAsync();
                var count = await cmd.ExecuteScalarAsync();
                return count != null && int.TryParse(count.ToString(), out var rows) && rows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id, Guid tenantId)
        {
            try
            {
                using var conn = _dbHelper.GetConnection();
                using var cmd = _dbHelper.CreateCommand("USP_AdmissionApplications_AA", conn);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Delete";
                cmd.Parameters.Add("@AA_Id", SqlDbType.UniqueIdentifier).Value = id;
                cmd.Parameters.Add("@AA_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
                await conn.OpenAsync();
                var res = await cmd.ExecuteScalarAsync();
                if (res != null && int.TryParse(res.ToString(), out var count) && count > 0)
                {
                    return true;
                }
            }
            catch { }

            try
            {
                using var conn = _dbHelper.GetConnection();
                using var cmd = new SqlCommand("DELETE FROM dbo.AdmissionApplications_AA WHERE AA_Id = @AA_Id; SELECT @@ROWCOUNT;", conn);
                cmd.Parameters.Add("@AA_Id", SqlDbType.UniqueIdentifier).Value = id;
                await conn.OpenAsync();
                var count = await cmd.ExecuteScalarAsync();
                return count != null && int.TryParse(count.ToString(), out var rows) && rows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ReviewAsync(Guid id, string status, string notes, Guid tenantId, Guid reviewedBy)
        {
            try
            {
                using var conn = _dbHelper.GetConnection();
                using var cmd = _dbHelper.CreateCommand("USP_AdmissionApplications_AA", conn);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Review";
                cmd.Parameters.Add("@AA_Id", SqlDbType.UniqueIdentifier).Value = id;
                cmd.Parameters.Add("@AA_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
                cmd.Parameters.Add("@AA_Status", SqlDbType.NVarChar, 20).Value = status;
                cmd.Parameters.Add("@AA_Notes", SqlDbType.NVarChar, -1).Value = (object)notes ?? DBNull.Value;
                cmd.Parameters.Add("@AA_ReviewedBy", SqlDbType.UniqueIdentifier).Value = reviewedBy;
                await conn.OpenAsync();
                var res = await cmd.ExecuteScalarAsync();
                if (res != null && int.TryParse(res.ToString(), out var count) && count > 0)
                {
                    return true;
                }
            }
            catch { }

            try
            {
                using var conn = _dbHelper.GetConnection();
                using var cmd = new SqlCommand(@"
UPDATE dbo.AdmissionApplications_AA
SET 
    AA_Status = @AA_Status,
    AA_Notes = ISNULL(@AA_Notes, AA_Notes),
    AA_ReviewedAt = SYSUTCDATETIME(),
    AA_ReviewedBy = @AA_ReviewedBy,
    AA_UpdatedAt = SYSUTCDATETIME()
WHERE AA_Id = @AA_Id;
SELECT @@ROWCOUNT;", conn);
                cmd.Parameters.Add("@AA_Id", SqlDbType.UniqueIdentifier).Value = id;
                cmd.Parameters.Add("@AA_Status", SqlDbType.NVarChar, 20).Value = status;
                cmd.Parameters.Add("@AA_Notes", SqlDbType.NVarChar, -1).Value = (object)notes ?? DBNull.Value;
                cmd.Parameters.Add("@AA_ReviewedBy", SqlDbType.UniqueIdentifier).Value = reviewedBy;
                await conn.OpenAsync();
                var count = await cmd.ExecuteScalarAsync();
                return count != null && int.TryParse(count.ToString(), out var rows) && rows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LinkEnrolledStudentAsync(Guid id, Guid studentId, Guid enrollmentId, Guid tenantId, Guid currentUserId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_AdmissionApplications_AA", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Review";
            cmd.Parameters.Add("@AA_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@AA_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@AA_Status", SqlDbType.NVarChar, 20).Value = "Enrolled";
            cmd.Parameters.Add("@AA_AdmittedStudentId", SqlDbType.UniqueIdentifier).Value = studentId;
            cmd.Parameters.Add("@AA_EnrollmentId", SqlDbType.UniqueIdentifier).Value = enrollmentId;
            cmd.Parameters.Add("@AA_ReviewedBy", SqlDbType.UniqueIdentifier).Value = currentUserId;
            await conn.OpenAsync();
            return (int)await cmd.ExecuteScalarAsync() > 0;
        }

        private void AddParams(SqlCommand cmd, AdmissionApplication a)
        {
            cmd.Parameters.Add("@AA_Id", SqlDbType.UniqueIdentifier).Value = a.AA_Id != Guid.Empty ? a.AA_Id : Guid.NewGuid();
            cmd.Parameters.Add("@AA_TenantId", SqlDbType.UniqueIdentifier).Value = a.AA_TenantId != Guid.Empty ? (object)a.AA_TenantId : DBNull.Value;
            cmd.Parameters.Add("@AA_BranchId", SqlDbType.UniqueIdentifier).Value = a.AA_BranchId != Guid.Empty ? (object)a.AA_BranchId : DBNull.Value;
            cmd.Parameters.Add("@AA_ApplicationNumber", SqlDbType.NVarChar, 50).Value = (object)a.AA_ApplicationNumber ?? DBNull.Value;
            cmd.Parameters.Add("@AA_FirstName", SqlDbType.NVarChar, 100).Value = a.AA_FirstName ?? "";
            cmd.Parameters.Add("@AA_MiddleName", SqlDbType.NVarChar, 100).Value = (object)a.AA_MiddleName ?? DBNull.Value;
            cmd.Parameters.Add("@AA_LastName", SqlDbType.NVarChar, 100).Value = a.AA_LastName ?? "";
            cmd.Parameters.Add("@AA_DateOfBirth", SqlDbType.Date).Value = (object)a.AA_DateOfBirth ?? DBNull.Value;
            cmd.Parameters.Add("@AA_Gender", SqlDbType.NVarChar, 20).Value = (object)a.AA_Gender ?? DBNull.Value;
            cmd.Parameters.Add("@AA_BloodGroup", SqlDbType.NVarChar, 10).Value = (object)a.AA_BloodGroup ?? DBNull.Value;
            cmd.Parameters.Add("@AA_Nationality", SqlDbType.NVarChar, 50).Value = (object)a.AA_Nationality ?? DBNull.Value;
            cmd.Parameters.Add("@AA_Category", SqlDbType.NVarChar, 50).Value = (object)a.AA_Category ?? DBNull.Value;
            cmd.Parameters.Add("@AA_Religion", SqlDbType.NVarChar, 50).Value = (object)a.AA_Religion ?? DBNull.Value;
            cmd.Parameters.Add("@AA_AadhaarNumber", SqlDbType.NVarChar, 30).Value = (object)a.AA_AadhaarNumber ?? DBNull.Value;
            cmd.Parameters.Add("@AA_EmergencyContactName", SqlDbType.NVarChar, 100).Value = (object)a.AA_EmergencyContactName ?? DBNull.Value;
            cmd.Parameters.Add("@AA_EmergencyContactPhone", SqlDbType.NVarChar, 30).Value = (object)a.AA_EmergencyContactPhone ?? DBNull.Value;
            cmd.Parameters.Add("@AA_Email", SqlDbType.NVarChar, 255).Value = (object)a.AA_Email ?? DBNull.Value;
            cmd.Parameters.Add("@AA_Phone", SqlDbType.NVarChar, 30).Value = a.AA_Phone ?? "";
            
            // Parent details
            cmd.Parameters.Add("@AA_FatherName", SqlDbType.NVarChar, 100).Value = (object)a.AA_FatherName ?? DBNull.Value;
            cmd.Parameters.Add("@AA_FatherPhone", SqlDbType.NVarChar, 30).Value = (object)a.AA_FatherPhone ?? DBNull.Value;
            cmd.Parameters.Add("@AA_FatherOccupation", SqlDbType.NVarChar, 100).Value = (object)a.AA_FatherOccupation ?? DBNull.Value;
            cmd.Parameters.Add("@AA_FatherEmail", SqlDbType.NVarChar, 255).Value = (object)a.AA_FatherEmail ?? DBNull.Value;
            cmd.Parameters.Add("@AA_MotherName", SqlDbType.NVarChar, 100).Value = (object)a.AA_MotherName ?? DBNull.Value;
            cmd.Parameters.Add("@AA_MotherPhone", SqlDbType.NVarChar, 30).Value = (object)a.AA_MotherPhone ?? DBNull.Value;
            cmd.Parameters.Add("@AA_MotherOccupation", SqlDbType.NVarChar, 100).Value = (object)a.AA_MotherOccupation ?? DBNull.Value;
            cmd.Parameters.Add("@AA_MotherEmail", SqlDbType.NVarChar, 255).Value = (object)a.AA_MotherEmail ?? DBNull.Value;
            cmd.Parameters.Add("@AA_GuardianName", SqlDbType.NVarChar, 100).Value = (object)a.AA_GuardianName ?? DBNull.Value;
            cmd.Parameters.Add("@AA_GuardianPhone", SqlDbType.NVarChar, 30).Value = (object)a.AA_GuardianPhone ?? DBNull.Value;
            cmd.Parameters.Add("@AA_GuardianRelation", SqlDbType.NVarChar, 50).Value = (object)a.AA_GuardianRelation ?? DBNull.Value;
            cmd.Parameters.Add("@AA_GuardianEmail", SqlDbType.NVarChar, 255).Value = (object)a.AA_GuardianEmail ?? DBNull.Value;
            cmd.Parameters.Add("@AA_GuardianOccupation", SqlDbType.NVarChar, 100).Value = (object)a.AA_GuardianOccupation ?? DBNull.Value;

            // Address details
            cmd.Parameters.Add("@AA_AddressLine1", SqlDbType.NVarChar, 255).Value = (object)a.AA_AddressLine1 ?? DBNull.Value;
            cmd.Parameters.Add("@AA_AddressLine2", SqlDbType.NVarChar, 255).Value = (object)a.AA_AddressLine2 ?? DBNull.Value;
            cmd.Parameters.Add("@AA_City", SqlDbType.NVarChar, 100).Value = (object)a.AA_City ?? DBNull.Value;
            cmd.Parameters.Add("@AA_State", SqlDbType.NVarChar, 100).Value = (object)a.AA_State ?? DBNull.Value;
            cmd.Parameters.Add("@AA_PostalCode", SqlDbType.NVarChar, 20).Value = (object)a.AA_PostalCode ?? DBNull.Value;
            cmd.Parameters.Add("@AA_Country", SqlDbType.NVarChar, 100).Value = (object)a.AA_Country ?? DBNull.Value;

            // Previous Academic Record
            cmd.Parameters.Add("@AA_PreviousSchool", SqlDbType.NVarChar, 255).Value = (object)a.AA_PreviousSchool ?? DBNull.Value;
            cmd.Parameters.Add("@AA_PreviousBoard", SqlDbType.NVarChar, 100).Value = (object)a.AA_PreviousBoard ?? DBNull.Value;
            cmd.Parameters.Add("@AA_PreviousGrade", SqlDbType.NVarChar, 50).Value = (object)a.AA_PreviousGrade ?? DBNull.Value;
            cmd.Parameters.Add("@AA_PreviousMarks", SqlDbType.NVarChar, 50).Value = (object)a.AA_PreviousMarks ?? DBNull.Value;
            cmd.Parameters.Add("@AA_TransferCertificateNumber", SqlDbType.NVarChar, 100).Value = (object)a.AA_TransferCertificateNumber ?? DBNull.Value;

            // Academic Program Mapping
            cmd.Parameters.Add("@AA_ClassId", SqlDbType.UniqueIdentifier).Value = (object)a.AA_ClassId ?? DBNull.Value;
            cmd.Parameters.Add("@AA_CourseId", SqlDbType.UniqueIdentifier).Value = (object)a.AA_CourseId ?? DBNull.Value;
            cmd.Parameters.Add("@AA_AcademicYearId", SqlDbType.UniqueIdentifier).Value = a.AA_AcademicYearId != Guid.Empty ? (object)a.AA_AcademicYearId : DBNull.Value;
            cmd.Parameters.Add("@AA_Status", SqlDbType.NVarChar, 20).Value = a.AA_Status ?? "Submitted";
            cmd.Parameters.Add("@AA_SubmittedAt", SqlDbType.DateTime2).Value = (object)a.AA_SubmittedAt ?? DBNull.Value;
            cmd.Parameters.Add("@AA_Notes", SqlDbType.NVarChar, -1).Value = (object)a.AA_Notes ?? DBNull.Value;

            // Enhanced media & document fields
            cmd.Parameters.Add("@AA_StudentPhotoUrl", SqlDbType.NVarChar, 500).Value = (object)a.AA_StudentPhotoUrl ?? DBNull.Value;
            cmd.Parameters.Add("@AA_BirthCertificateUrl", SqlDbType.NVarChar, 500).Value = (object)a.AA_BirthCertificateUrl ?? DBNull.Value;
            cmd.Parameters.Add("@AA_TransferCertificateUrl", SqlDbType.NVarChar, 500).Value = (object)a.AA_TransferCertificateUrl ?? DBNull.Value;
            cmd.Parameters.Add("@AA_MarksheetUrl", SqlDbType.NVarChar, 500).Value = (object)a.AA_MarksheetUrl ?? DBNull.Value;
            cmd.Parameters.Add("@AA_NationalIdDocUrl", SqlDbType.NVarChar, 500).Value = (object)a.AA_NationalIdDocUrl ?? DBNull.Value;

            // Medical & Facilities
            cmd.Parameters.Add("@AA_MedicalConditions", SqlDbType.NVarChar, -1).Value = (object)a.AA_MedicalConditions ?? DBNull.Value;
            cmd.Parameters.Add("@AA_RequiresTransport", SqlDbType.Bit).Value = a.AA_RequiresTransport;
            cmd.Parameters.Add("@AA_TransportPickupPoint", SqlDbType.NVarChar, 200).Value = (object)a.AA_TransportPickupPoint ?? DBNull.Value;
            cmd.Parameters.Add("@AA_RequiresHostel", SqlDbType.Bit).Value = a.AA_RequiresHostel;

            // Language & Sibling
            cmd.Parameters.Add("@AA_SecondLanguage", SqlDbType.NVarChar, 100).Value = (object)a.AA_SecondLanguage ?? DBNull.Value;
            cmd.Parameters.Add("@AA_MotherTongue", SqlDbType.NVarChar, 100).Value = (object)a.AA_MotherTongue ?? DBNull.Value;
            cmd.Parameters.Add("@AA_HasSibling", SqlDbType.Bit).Value = a.AA_HasSibling;
            cmd.Parameters.Add("@AA_SiblingDetails", SqlDbType.NVarChar, 300).Value = (object)a.AA_SiblingDetails ?? DBNull.Value;

            // Financial & Linkages
            cmd.Parameters.Add("@AA_FeeStructureId", SqlDbType.UniqueIdentifier).Value = (object)a.AA_FeeStructureId ?? DBNull.Value;
            cmd.Parameters.Add("@AA_AdmittedStudentId", SqlDbType.UniqueIdentifier).Value = (object)a.AA_AdmittedStudentId ?? DBNull.Value;
            cmd.Parameters.Add("@AA_EnrollmentId", SqlDbType.UniqueIdentifier).Value = (object)a.AA_EnrollmentId ?? DBNull.Value;
        }

        private static AdmissionApplication Map(SqlDataReader r) => new()
        {
            AA_Id = r.GetGuid(r.GetOrdinal("AA_Id")),
            AA_TenantId = r.GetGuid(r.GetOrdinal("AA_TenantId")),
            AA_BranchId = r.GetGuid(r.GetOrdinal("AA_BranchId")),
            AA_ApplicationNumber = r["AA_ApplicationNumber"] as string,
            AA_FirstName = r["AA_FirstName"] as string,
            AA_MiddleName = HasColumn(r, "AA_MiddleName") ? r["AA_MiddleName"] as string : null,
            AA_LastName = r["AA_LastName"] as string,
            AA_DateOfBirth = r["AA_DateOfBirth"] as DateTime?,
            AA_Gender = r["AA_Gender"] as string,
            AA_BloodGroup = HasColumn(r, "AA_BloodGroup") ? r["AA_BloodGroup"] as string : null,
            AA_Nationality = HasColumn(r, "AA_Nationality") ? r["AA_Nationality"] as string : null,
            AA_Category = HasColumn(r, "AA_Category") ? r["AA_Category"] as string : null,
            AA_Religion = HasColumn(r, "AA_Religion") ? r["AA_Religion"] as string : null,
            AA_AadhaarNumber = HasColumn(r, "AA_AadhaarNumber") ? r["AA_AadhaarNumber"] as string : null,
            AA_EmergencyContactName = HasColumn(r, "AA_EmergencyContactName") ? r["AA_EmergencyContactName"] as string : null,
            AA_EmergencyContactPhone = HasColumn(r, "AA_EmergencyContactPhone") ? r["AA_EmergencyContactPhone"] as string : null,
            AA_Email = r["AA_Email"] as string,
            AA_Phone = r["AA_Phone"] as string,
            
            // Parent details
            AA_FatherName = HasColumn(r, "AA_FatherName") ? r["AA_FatherName"] as string : null,
            AA_FatherPhone = HasColumn(r, "AA_FatherPhone") ? r["AA_FatherPhone"] as string : null,
            AA_FatherOccupation = HasColumn(r, "AA_FatherOccupation") ? r["AA_FatherOccupation"] as string : null,
            AA_FatherEmail = HasColumn(r, "AA_FatherEmail") ? r["AA_FatherEmail"] as string : null,
            AA_MotherName = HasColumn(r, "AA_MotherName") ? r["AA_MotherName"] as string : null,
            AA_MotherPhone = HasColumn(r, "AA_MotherPhone") ? r["AA_MotherPhone"] as string : null,
            AA_MotherOccupation = HasColumn(r, "AA_MotherOccupation") ? r["AA_MotherOccupation"] as string : null,
            AA_MotherEmail = HasColumn(r, "AA_MotherEmail") ? r["AA_MotherEmail"] as string : null,
            AA_GuardianName = HasColumn(r, "AA_GuardianName") ? r["AA_GuardianName"] as string : null,
            AA_GuardianPhone = HasColumn(r, "AA_GuardianPhone") ? r["AA_GuardianPhone"] as string : null,
            AA_GuardianRelation = HasColumn(r, "AA_GuardianRelation") ? r["AA_GuardianRelation"] as string : null,
            AA_GuardianEmail = HasColumn(r, "AA_GuardianEmail") ? r["AA_GuardianEmail"] as string : null,
            AA_GuardianOccupation = HasColumn(r, "AA_GuardianOccupation") ? r["AA_GuardianOccupation"] as string : null,

            // Address details
            AA_AddressLine1 = HasColumn(r, "AA_AddressLine1") ? r["AA_AddressLine1"] as string : null,
            AA_AddressLine2 = HasColumn(r, "AA_AddressLine2") ? r["AA_AddressLine2"] as string : null,
            AA_City = HasColumn(r, "AA_City") ? r["AA_City"] as string : null,
            AA_State = HasColumn(r, "AA_State") ? r["AA_State"] as string : null,
            AA_PostalCode = HasColumn(r, "AA_PostalCode") ? r["AA_PostalCode"] as string : null,
            AA_Country = HasColumn(r, "AA_Country") ? r["AA_Country"] as string : null,

            // Previous Academic Record
            AA_PreviousSchool = HasColumn(r, "AA_PreviousSchool") ? r["AA_PreviousSchool"] as string : null,
            AA_PreviousBoard = HasColumn(r, "AA_PreviousBoard") ? r["AA_PreviousBoard"] as string : null,
            AA_PreviousGrade = HasColumn(r, "AA_PreviousGrade") ? r["AA_PreviousGrade"] as string : null,
            AA_PreviousMarks = HasColumn(r, "AA_PreviousMarks") ? r["AA_PreviousMarks"] as string : null,
            AA_TransferCertificateNumber = HasColumn(r, "AA_TransferCertificateNumber") ? r["AA_TransferCertificateNumber"] as string : null,

            // Academic Program Mapping
            AA_ClassId = HasColumn(r, "AA_ClassId") && r["AA_ClassId"] != DBNull.Value ? (Guid?)r["AA_ClassId"] : null,
            AA_CourseId = r["AA_CourseId"] as Guid?,
            AA_AcademicYearId = r.GetGuid(r.GetOrdinal("AA_AcademicYearId")),
            AA_Status = r["AA_Status"] as string,
            AA_SubmittedAt = r["AA_SubmittedAt"] as DateTime?,
            AA_ReviewedAt = r["AA_ReviewedAt"] as DateTime?,
            AA_ReviewedBy = r["AA_ReviewedBy"] as Guid?,
            AA_Notes = r["AA_Notes"] as string,

            // Enhanced media & document fields
            AA_StudentPhotoUrl = HasColumn(r, "AA_StudentPhotoUrl") ? r["AA_StudentPhotoUrl"] as string : null,
            AA_BirthCertificateUrl = HasColumn(r, "AA_BirthCertificateUrl") ? r["AA_BirthCertificateUrl"] as string : null,
            AA_TransferCertificateUrl = HasColumn(r, "AA_TransferCertificateUrl") ? r["AA_TransferCertificateUrl"] as string : null,
            AA_MarksheetUrl = HasColumn(r, "AA_MarksheetUrl") ? r["AA_MarksheetUrl"] as string : null,
            AA_NationalIdDocUrl = HasColumn(r, "AA_NationalIdDocUrl") ? r["AA_NationalIdDocUrl"] as string : null,

            // Medical & Facilities
            AA_MedicalConditions = HasColumn(r, "AA_MedicalConditions") ? r["AA_MedicalConditions"] as string : null,
            AA_RequiresTransport = HasColumn(r, "AA_RequiresTransport") && r["AA_RequiresTransport"] != DBNull.Value && (bool)r["AA_RequiresTransport"],
            AA_TransportPickupPoint = HasColumn(r, "AA_TransportPickupPoint") ? r["AA_TransportPickupPoint"] as string : null,
            AA_RequiresHostel = HasColumn(r, "AA_RequiresHostel") && r["AA_RequiresHostel"] != DBNull.Value && (bool)r["AA_RequiresHostel"],

            // Language & Sibling
            AA_SecondLanguage = HasColumn(r, "AA_SecondLanguage") ? r["AA_SecondLanguage"] as string : null,
            AA_MotherTongue = HasColumn(r, "AA_MotherTongue") ? r["AA_MotherTongue"] as string : null,
            AA_HasSibling = HasColumn(r, "AA_HasSibling") && r["AA_HasSibling"] != DBNull.Value && (bool)r["AA_HasSibling"],
            AA_SiblingDetails = HasColumn(r, "AA_SiblingDetails") ? r["AA_SiblingDetails"] as string : null,

            // Financial & Linkages
            AA_FeeStructureId = HasColumn(r, "AA_FeeStructureId") && r["AA_FeeStructureId"] != DBNull.Value ? (Guid?)r["AA_FeeStructureId"] : null,
            AA_AdmittedStudentId = HasColumn(r, "AA_AdmittedStudentId") && r["AA_AdmittedStudentId"] != DBNull.Value ? (Guid?)r["AA_AdmittedStudentId"] : null,
            AA_EnrollmentId = HasColumn(r, "AA_EnrollmentId") && r["AA_EnrollmentId"] != DBNull.Value ? (Guid?)r["AA_EnrollmentId"] : null,

            AA_CreatedAt = r.GetDateTime(r.GetOrdinal("AA_CreatedAt")),
            AA_UpdatedAt = r.GetDateTime(r.GetOrdinal("AA_UpdatedAt")),
            
            CourseName = HasColumn(r, "CourseName") ? r["CourseName"] as string : null,
            AcademicYearName = HasColumn(r, "AcademicYearName") ? r["AcademicYearName"] as string : null,
            ClassName = HasColumn(r, "ClassName") ? r["ClassName"] as string : null,
            BranchName = HasColumn(r, "BranchName") ? r["BranchName"] as string : null,
            AdmittedStudentCode = HasColumn(r, "AdmittedStudentCode") ? r["AdmittedStudentCode"] as string : null,
            AdmittedStudentAdmissionNumber = HasColumn(r, "AdmittedStudentAdmissionNumber") ? r["AdmittedStudentAdmissionNumber"] as string : null
        };

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
