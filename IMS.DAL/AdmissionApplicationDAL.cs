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

        public async Task<(List<AdmissionApplication> Items, int TotalCount)> GetPagedAsync(Guid tenantId, string searchTerm,
            Guid? branchId, Guid? classId, Guid? courseId, Guid? academicYearId, string status, int page, int pageSize)
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
            return (items, totalCount);
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
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_AdmissionApplications_AA", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Insert";
            AddParams(cmd, a);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Guid.TryParse(result?.ToString(), out var id) ? id : a.AA_Id;
        }

        public async Task<bool> UpdateAsync(AdmissionApplication a)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_AdmissionApplications_AA", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Update";
            AddParams(cmd, a);
            await conn.OpenAsync();
            return (int)await cmd.ExecuteScalarAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_AdmissionApplications_AA", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Delete";
            cmd.Parameters.Add("@AA_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@AA_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            await conn.OpenAsync();
            return (int)await cmd.ExecuteScalarAsync() > 0;
        }

        public async Task<bool> ReviewAsync(Guid id, string status, string notes, Guid tenantId, Guid reviewedBy)
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
            AA_CreatedAt = r.GetDateTime(r.GetOrdinal("AA_CreatedAt")),
            AA_UpdatedAt = r.GetDateTime(r.GetOrdinal("AA_UpdatedAt")),
            
            CourseName = HasColumn(r, "CourseName") ? r["CourseName"] as string : null,
            AcademicYearName = HasColumn(r, "AcademicYearName") ? r["AcademicYearName"] as string : null,
            ClassName = HasColumn(r, "ClassName") ? r["ClassName"] as string : null,
            BranchName = HasColumn(r, "BranchName") ? r["BranchName"] as string : null
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
