using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using IMS.DAL.Common;
using IMS.DAL.Interfaces;
using IMS.Models.Entities;
using IMS.Models.ViewModels;

namespace IMS.DAL.Repositories
{
    public class FeeStructureDAL : IFeeStructureDAL
    {
        private readonly DBHelper _dbHelper;
        public FeeStructureDAL(DBHelper dbHelper) { _dbHelper = dbHelper; }

        public async Task<FeeStructure?> GetByIdAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeStructures_FS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetById";
            cmd.Parameters.Add("@FS_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@FS_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapFeeStructure(reader) : null;
        }

        public async Task<(List<FeeStructure> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, string? searchTerm, Guid? academicYearId, int pageNumber, int pageSize)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeStructures_FS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetPaged";
            cmd.Parameters.Add("@FS_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 255).Value = (object?)searchTerm ?? DBNull.Value;
            cmd.Parameters.Add("@AcademicYearId", SqlDbType.UniqueIdentifier).Value = (object?)academicYearId ?? DBNull.Value;
            cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            int totalCount = 0;
            if (await reader.ReadAsync()) totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
            var items = new List<FeeStructure>();
            await reader.NextResultAsync();
            while (await reader.ReadAsync()) items.Add(MapFeeStructure(reader));
            return (items, totalCount);
        }

        public async Task<bool> IsCodeTakenAsync(Guid tenantId, string code, Guid? excludeId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeStructures_FS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "ExistsByCode";
            cmd.Parameters.Add("@FS_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@FS_Code", SqlDbType.NVarChar, 50).Value = code;
            cmd.Parameters.Add("@ExcludeId", SqlDbType.UniqueIdentifier).Value = (object?)excludeId ?? DBNull.Value;
            await conn.OpenAsync();
            var res = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(res) == 1;
        }

        public async Task<Guid> CreateAsync(FeeStructure fs)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeStructures_FS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Insert";
            AddParameters(cmd, fs);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Guid.TryParse(result?.ToString(), out var id) ? id : fs.FS_Id;
        }

        public async Task<bool> UpdateAsync(FeeStructure fs)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeStructures_FS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Update";
            AddParameters(cmd, fs);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeStructures_FS", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Delete";
            cmd.Parameters.Add("@FS_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@FS_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        private void AddParameters(SqlCommand cmd, FeeStructure fs)
        {
            cmd.Parameters.Add("@FS_Id", SqlDbType.UniqueIdentifier).Value = fs.FS_Id;
            cmd.Parameters.Add("@FS_TenantId", SqlDbType.UniqueIdentifier).Value = fs.FS_TenantId;
            cmd.Parameters.Add("@FS_Name", SqlDbType.NVarChar, 150).Value = fs.FS_Name;
            cmd.Parameters.Add("@FS_Code", SqlDbType.NVarChar, 50).Value = fs.FS_Code;
            cmd.Parameters.Add("@FS_CourseId", SqlDbType.UniqueIdentifier).Value = (object?)fs.FS_CourseId ?? DBNull.Value;
            cmd.Parameters.Add("@FS_BatchId", SqlDbType.UniqueIdentifier).Value = (object?)fs.FS_BatchId ?? DBNull.Value;
            cmd.Parameters.Add("@FS_AcademicYearId", SqlDbType.UniqueIdentifier).Value = fs.FS_AcademicYearId;
            cmd.Parameters.Add("@FS_Description", SqlDbType.NVarChar, -1).Value = (object?)fs.FS_Description ?? DBNull.Value;
            cmd.Parameters.Add("@FS_IsActive", SqlDbType.Bit).Value = fs.FS_IsActive;
        }

        private static FeeStructure MapFeeStructure(SqlDataReader r) => new()
        {
            FS_Id = r.GetGuid(r.GetOrdinal("FS_Id")),
            FS_TenantId = r.GetGuid(r.GetOrdinal("FS_TenantId")),
            FS_Name = r["FS_Name"] as string ?? string.Empty,
            FS_Code = r["FS_Code"] as string ?? string.Empty,
            FS_CourseId = r["FS_CourseId"] as Guid?,
            FS_BatchId = r["FS_BatchId"] as Guid?,
            FS_AcademicYearId = r.GetGuid(r.GetOrdinal("FS_AcademicYearId")),
            FS_Description = r["FS_Description"] as string,
            FS_IsActive = r.GetBoolean(r.GetOrdinal("FS_IsActive")),
            FS_CreatedAt = r.GetDateTime(r.GetOrdinal("FS_CreatedAt")),
            FS_UpdatedAt = r.GetDateTime(r.GetOrdinal("FS_UpdatedAt")),
            CourseName = r["CourseName"] as string,
            BatchName = r["BatchName"] as string,
            AcademicYearName = r["AcademicYearName"] as string
        };
    }

    public class FeeStructureItemDAL : IFeeStructureItemDAL
    {
        private readonly DBHelper _dbHelper;
        public FeeStructureItemDAL(DBHelper dbHelper) { _dbHelper = dbHelper; }

        public async Task<List<FeeStructureItem>> GetByFeeStructureIdAsync(Guid feeStructureId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeStructureItems_FSI", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetByFeeStructure";
            cmd.Parameters.Add("@FSI_FeeStructureId", SqlDbType.UniqueIdentifier).Value = feeStructureId;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            var items = new List<FeeStructureItem>();
            while (await reader.ReadAsync())
            {
                items.Add(new FeeStructureItem
                {
                    FSI_Id = reader.GetGuid(reader.GetOrdinal("FSI_Id")),
                    FSI_FeeStructureId = reader.GetGuid(reader.GetOrdinal("FSI_FeeStructureId")),
                    FSI_FeeCategoryId = reader.GetGuid(reader.GetOrdinal("FSI_FeeCategoryId")),
                    FSI_Amount = reader.GetDecimal(reader.GetOrdinal("FSI_Amount")),
                    FSI_DueDays = reader["FSI_DueDays"] as int?,
                    FSI_IsMandatory = reader.GetBoolean(reader.GetOrdinal("FSI_IsMandatory")),
                    FeeCategoryName = reader["FeeCategoryName"] as string
                });
            }
            return items;
        }

        public async Task<bool> SaveItemsAsync(Guid feeStructureId, List<FeeStructureItem> items)
        {
            using var conn = _dbHelper.GetConnection();
            await conn.OpenAsync();
            using var deleteCmd = _dbHelper.CreateCommand("USP_FeeStructureItems_FSI", conn);
            deleteCmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "DeleteByFeeStructure";
            deleteCmd.Parameters.Add("@FSI_FeeStructureId", SqlDbType.UniqueIdentifier).Value = feeStructureId;
            await deleteCmd.ExecuteNonQueryAsync();

            foreach (var item in items)
            {
                using var cmd = _dbHelper.CreateCommand("USP_FeeStructureItems_FSI", conn);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Insert";
                cmd.Parameters.Add("@FSI_Id", SqlDbType.UniqueIdentifier).Value = item.FSI_Id;
                cmd.Parameters.Add("@FSI_FeeStructureId", SqlDbType.UniqueIdentifier).Value = feeStructureId;
                cmd.Parameters.Add("@FSI_FeeCategoryId", SqlDbType.UniqueIdentifier).Value = item.FSI_FeeCategoryId;
                cmd.Parameters.Add("@FSI_Amount", SqlDbType.Decimal).Value = item.FSI_Amount;
                cmd.Parameters.Add("@FSI_DueDays", SqlDbType.Int).Value = (object?)item.FSI_DueDays ?? DBNull.Value;
                cmd.Parameters.Add("@FSI_IsMandatory", SqlDbType.Bit).Value = item.FSI_IsMandatory;
                await cmd.ExecuteNonQueryAsync();
            }
            return true;
        }
    }

    public class FeeInvoiceDAL : IFeeInvoiceDAL
    {
        private readonly DBHelper _dbHelper;
        public FeeInvoiceDAL(DBHelper dbHelper) { _dbHelper = dbHelper; }

        public async Task<FeeInvoice?> GetByIdAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeInvoices_FI", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetById";
            cmd.Parameters.Add("@FI_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@FI_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapInvoice(reader) : null;
        }

        public async Task<(List<FeeInvoice> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, string? searchTerm, string? status, int pageNumber, int pageSize)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeInvoices_FI", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetPaged";
            cmd.Parameters.Add("@FI_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 255).Value = (object?)searchTerm ?? DBNull.Value;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = (object?)status ?? DBNull.Value;
            cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            int totalCount = 0;
            if (await reader.ReadAsync()) totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
            var items = new List<FeeInvoice>();
            await reader.NextResultAsync();
            while (await reader.ReadAsync()) items.Add(MapInvoice(reader));
            return (items, totalCount);
        }

        public async Task<List<FeeInvoice>> GetPendingByStudentAsync(Guid studentId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            const string sql = @"
                SELECT fi.*, s.S_FirstName + ' ' + s.S_LastName AS StudentName, s.S_StudentCode AS StudentCode,
                (
                    SELECT STRING_AGG(fc.FC_Name + ' (₹' + CAST(CAST(fii.FII_TotalAmount AS INT) AS NVARCHAR) + ')', ', ')
                    FROM FeeInvoiceItems_FII fii
                    INNER JOIN FeeCategories_FC fc ON fii.FII_FeeCategoryId = fc.FC_Id
                    WHERE fii.FII_InvoiceId = fi.FI_Id
                ) AS FeeHeadsSummary
                FROM FeeInvoices_FI fi
                INNER JOIN Students_S s ON fi.FI_StudentId = s.S_Id
                WHERE fi.FI_TenantId = @TenantId 
                  AND fi.FI_StudentId = @StudentId 
                  AND fi.FI_BalanceAmount > 0
                  AND fi.FI_Status NOT IN ('cancelled', 'Cancelled')
                ORDER BY fi.FI_DueDate ASC, fi.FI_InvoiceDate ASC;";

            using var cmd = _dbHelper.CreateCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            var list = new List<FeeInvoice>();
            while (await reader.ReadAsync())
            {
                var inv = MapInvoice(reader);
                inv.StudentName = reader["FeeHeadsSummary"] as string ?? inv.StudentName; // summary container
                list.Add(inv);
            }
            return list;
        }

        public async Task<string> GetNextInvoiceNumberAsync(Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeInvoices_FI", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetNextNumber";
            cmd.Parameters.Add("@FI_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result?.ToString() ?? "INV-00001";
        }

        public async Task<Guid> CreateAsync(FeeInvoice invoice)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeInvoices_FI", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Insert";
            AddParameters(cmd, invoice);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Guid.TryParse(result?.ToString(), out var id) ? id : invoice.FI_Id;
        }

        public async Task<bool> UpdateAsync(FeeInvoice invoice)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeInvoices_FI", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Update";
            AddParameters(cmd, invoice);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdatePaidAmountAsync(Guid invoiceId, decimal paidAmount)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeInvoices_FI", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "UpdatePaidAmount";
            cmd.Parameters.Add("@FI_Id", SqlDbType.UniqueIdentifier).Value = invoiceId;
            cmd.Parameters.Add("@FI_PaidAmount", SqlDbType.Decimal).Value = paidAmount;
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeInvoices_FI", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Delete";
            cmd.Parameters.Add("@FI_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@FI_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<decimal> GetTotalDueByStudentAsync(Guid studentId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            const string sql = "SELECT ISNULL(SUM(FI_BalanceAmount), 0) FROM FeeInvoices_FI WHERE FI_StudentId = @StudentId AND FI_TenantId = @TenantId AND FI_BalanceAmount > 0 AND FI_Status NOT IN ('cancelled', 'Cancelled')";
            using var cmd = _dbHelper.CreateCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
        }

        public async Task<(Guid InvoiceId, string InvoiceNumber)> CreateAdHocChargeAsync(
            Guid tenantId, Guid studentId, Guid feeCategoryId, decimal amount, 
            string description, DateTime? dueDate, Guid createdBy)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_CreateAdHocStudentCharge", conn);
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;
            cmd.Parameters.Add("@FeeCategoryId", SqlDbType.UniqueIdentifier).Value = feeCategoryId;
            cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 255).Value = description;
            cmd.Parameters.Add("@DueDate", SqlDbType.Date).Value = (object?)dueDate ?? DBNull.Value;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = createdBy;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var invId = reader.GetGuid(reader.GetOrdinal("InvoiceId"));
                var invNum = reader["InvoiceNumber"]?.ToString() ?? string.Empty;
                return (invId, invNum);
            }
            return (Guid.Empty, string.Empty);
        }

        public async Task<(int GeneratedCount, decimal TotalAmount)> GenerateBatchInvoicesAsync(
            Guid tenantId, Guid academicYearId, Guid courseId, Guid? batchId, 
            Guid feeStructureId, string billingPeriod, DateTime invoiceDate, DateTime dueDate, Guid createdBy)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_GenerateBatchFeeInvoices", conn);
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@AcademicYearId", SqlDbType.UniqueIdentifier).Value = academicYearId;
            cmd.Parameters.Add("@CourseId", SqlDbType.UniqueIdentifier).Value = courseId;
            cmd.Parameters.Add("@BatchId", SqlDbType.UniqueIdentifier).Value = (object?)batchId ?? DBNull.Value;
            cmd.Parameters.Add("@FeeStructureId", SqlDbType.UniqueIdentifier).Value = feeStructureId;
            cmd.Parameters.Add("@BillingPeriod", SqlDbType.NVarChar, 50).Value = billingPeriod;
            cmd.Parameters.Add("@InvoiceDate", SqlDbType.Date).Value = invoiceDate;
            cmd.Parameters.Add("@DueDate", SqlDbType.Date).Value = dueDate;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = createdBy;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                int count = Convert.ToInt32(reader["InvoicesGenerated"]);
                decimal total = Convert.ToDecimal(reader["TotalAmountBilled"]);
                return (count, total);
            }
            return (0, 0m);
        }

        public async Task<List<FeeInvoiceItem>> GetItemsByInvoiceIdAsync(Guid invoiceId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_FeeInvoiceItems_FII", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetByInvoiceId";
            cmd.Parameters.Add("@FII_InvoiceId", SqlDbType.UniqueIdentifier).Value = invoiceId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            var list = new List<FeeInvoiceItem>();
            while (await reader.ReadAsync())
            {
                list.Add(new FeeInvoiceItem
                {
                    FII_Id = reader.GetGuid(reader.GetOrdinal("FII_Id")),
                    FII_InvoiceId = reader.GetGuid(reader.GetOrdinal("FII_InvoiceId")),
                    FII_FeeCategoryId = reader.GetGuid(reader.GetOrdinal("FII_FeeCategoryId")),
                    FII_Description = reader["FII_Description"]?.ToString() ?? string.Empty,
                    FII_Quantity = reader.GetDecimal(reader.GetOrdinal("FII_Quantity")),
                    FII_UnitAmount = reader.GetDecimal(reader.GetOrdinal("FII_UnitAmount")),
                    FII_DiscountAmount = reader.GetDecimal(reader.GetOrdinal("FII_DiscountAmount")),
                    FII_TaxAmount = reader.GetDecimal(reader.GetOrdinal("FII_TaxAmount")),
                    FII_TotalAmount = reader.GetDecimal(reader.GetOrdinal("FII_TotalAmount")),
                    FeeCategoryName = reader["FeeCategoryName"] as string,
                    FeeCategoryCode = reader["FeeCategoryCode"] as string
                });
            }
            return list;
        }

        public async Task<bool> SaveInvoiceItemsAsync(Guid invoiceId, List<FeeInvoiceItem> items)
        {
            using var conn = _dbHelper.GetConnection();
            await conn.OpenAsync();

            using var deleteCmd = _dbHelper.CreateCommand("USP_FeeInvoiceItems_FII", conn);
            deleteCmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "DeleteByInvoice";
            deleteCmd.Parameters.Add("@FII_InvoiceId", SqlDbType.UniqueIdentifier).Value = invoiceId;
            await deleteCmd.ExecuteNonQueryAsync();

            foreach (var item in items)
            {
                using var cmd = _dbHelper.CreateCommand("USP_FeeInvoiceItems_FII", conn);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Insert";
                cmd.Parameters.Add("@FII_Id", SqlDbType.UniqueIdentifier).Value = item.FII_Id == Guid.Empty ? Guid.NewGuid() : item.FII_Id;
                cmd.Parameters.Add("@FII_InvoiceId", SqlDbType.UniqueIdentifier).Value = invoiceId;
                cmd.Parameters.Add("@FII_FeeCategoryId", SqlDbType.UniqueIdentifier).Value = item.FII_FeeCategoryId;
                cmd.Parameters.Add("@FII_Description", SqlDbType.NVarChar, 255).Value = item.FII_Description ?? string.Empty;
                cmd.Parameters.Add("@FII_Quantity", SqlDbType.Decimal).Value = item.FII_Quantity;
                cmd.Parameters.Add("@FII_UnitAmount", SqlDbType.Decimal).Value = item.FII_UnitAmount;
                cmd.Parameters.Add("@FII_DiscountAmount", SqlDbType.Decimal).Value = item.FII_DiscountAmount;
                cmd.Parameters.Add("@FII_TaxAmount", SqlDbType.Decimal).Value = item.FII_TaxAmount;
                cmd.Parameters.Add("@FII_TotalAmount", SqlDbType.Decimal).Value = item.FII_TotalAmount;
                await cmd.ExecuteNonQueryAsync();
            }
            return true;
        }

        private void AddParameters(SqlCommand cmd, FeeInvoice fi)
        {
            cmd.Parameters.Add("@FI_Id", SqlDbType.UniqueIdentifier).Value = fi.FI_Id;
            cmd.Parameters.Add("@FI_TenantId", SqlDbType.UniqueIdentifier).Value = fi.FI_TenantId;
            cmd.Parameters.Add("@FI_StudentId", SqlDbType.UniqueIdentifier).Value = fi.FI_StudentId;
            cmd.Parameters.Add("@FI_InvoiceNumber", SqlDbType.NVarChar, 50).Value = fi.FI_InvoiceNumber;
            cmd.Parameters.Add("@FI_InvoiceDate", SqlDbType.Date).Value = fi.FI_InvoiceDate;
            cmd.Parameters.Add("@FI_DueDate", SqlDbType.Date).Value = fi.FI_DueDate;
            cmd.Parameters.Add("@FI_Subtotal", SqlDbType.Decimal).Value = fi.FI_Subtotal;
            cmd.Parameters.Add("@FI_DiscountAmount", SqlDbType.Decimal).Value = fi.FI_DiscountAmount;
            cmd.Parameters.Add("@FI_TaxAmount", SqlDbType.Decimal).Value = fi.FI_TaxAmount;
            cmd.Parameters.Add("@FI_TotalAmount", SqlDbType.Decimal).Value = fi.FI_TotalAmount;
            cmd.Parameters.Add("@FI_PaidAmount", SqlDbType.Decimal).Value = fi.FI_PaidAmount;
            cmd.Parameters.Add("@FI_BalanceAmount", SqlDbType.Decimal).Value = fi.FI_BalanceAmount;
            cmd.Parameters.Add("@FI_Status", SqlDbType.NVarChar, 20).Value = fi.FI_Status;
            cmd.Parameters.Add("@FI_Notes", SqlDbType.NVarChar, -1).Value = (object?)fi.FI_Notes ?? DBNull.Value;
        }

        private static FeeInvoice MapInvoice(SqlDataReader r) => new()
        {
            FI_Id = r.GetGuid(r.GetOrdinal("FI_Id")),
            FI_TenantId = r.GetGuid(r.GetOrdinal("FI_TenantId")),
            FI_StudentId = r.GetGuid(r.GetOrdinal("FI_StudentId")),
            FI_InvoiceNumber = r["FI_InvoiceNumber"] as string ?? string.Empty,
            FI_InvoiceDate = r.GetDateTime(r.GetOrdinal("FI_InvoiceDate")),
            FI_DueDate = r.GetDateTime(r.GetOrdinal("FI_DueDate")),
            FI_Subtotal = r.GetDecimal(r.GetOrdinal("FI_Subtotal")),
            FI_DiscountAmount = r.GetDecimal(r.GetOrdinal("FI_DiscountAmount")),
            FI_TaxAmount = r.GetDecimal(r.GetOrdinal("FI_TaxAmount")),
            FI_TotalAmount = r.GetDecimal(r.GetOrdinal("FI_TotalAmount")),
            FI_PaidAmount = r.GetDecimal(r.GetOrdinal("FI_PaidAmount")),
            FI_BalanceAmount = r.GetDecimal(r.GetOrdinal("FI_BalanceAmount")),
            FI_Status = r["FI_Status"] as string ?? string.Empty,
            FI_Notes = r["FI_Notes"] as string,
            FI_CreatedAt = r.GetDateTime(r.GetOrdinal("FI_CreatedAt")),
            FI_UpdatedAt = r.GetDateTime(r.GetOrdinal("FI_UpdatedAt")),
            StudentName = r["StudentName"] as string,
            StudentCode = r["StudentCode"] as string
        };
    }

    public class PaymentDAL : IPaymentDAL
    {
        private readonly DBHelper _dbHelper;
        public PaymentDAL(DBHelper dbHelper) { _dbHelper = dbHelper; }

        public async Task<Payment?> GetByIdAsync(Guid id, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Payments_PAY", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetById";
            cmd.Parameters.Add("@PAY_Id", SqlDbType.UniqueIdentifier).Value = id;
            cmd.Parameters.Add("@PAY_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapPayment(reader) : null;
        }

        public async Task<(List<Payment> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, string? searchTerm, string? status, int pageNumber, int pageSize)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Payments_PAY", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetPaged";
            cmd.Parameters.Add("@PAY_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 255).Value = (object?)searchTerm ?? DBNull.Value;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = (object?)status ?? DBNull.Value;
            cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            int totalCount = 0;
            if (await reader.ReadAsync()) totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
            var items = new List<Payment>();
            await reader.NextResultAsync();
            while (await reader.ReadAsync()) items.Add(MapPayment(reader));
            return (items, totalCount);
        }

        public async Task<string> GetNextPaymentNumberAsync(Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Payments_PAY", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "GetNextNumber";
            cmd.Parameters.Add("@PAY_TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result?.ToString() ?? "REC/" + DateTime.Today.Year + "/00001";
        }

        public async Task<Guid> CreateAsync(Payment payment)
        {
            var res = await ProcessPaymentAsync(
                payment.PAY_TenantId, payment.PAY_StudentId, payment.PAY_PaymentDate,
                payment.PAY_Amount, payment.PAY_PaymentMethodId, payment.PAY_TransactionReference,
                payment.PAY_ChequeNumber, payment.PAY_BankName, payment.PAY_ChequeDate,
                payment.PAY_Notes, payment.PAY_CreatedBy, null);
            return res.PaymentId;
        }

        public async Task<(Guid PaymentId, string ReceiptNumber, decimal TotalAllocated, decimal AdvanceCredited)> ProcessPaymentAsync(
            Guid tenantId, Guid studentId, DateTime paymentDate, decimal amount, 
            Guid paymentMethodId, string? transactionRef, string? chequeNo, string? bankName, 
            DateTime? chequeDate, string? notes, Guid createdBy, string? selectedInvoiceIds)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_ProcessStudentPayment", conn);
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;
            cmd.Parameters.Add("@PaymentDate", SqlDbType.DateTime2).Value = paymentDate;
            cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
            cmd.Parameters.Add("@PaymentMethodId", SqlDbType.UniqueIdentifier).Value = paymentMethodId;
            cmd.Parameters.Add("@TransactionReference", SqlDbType.NVarChar, 150).Value = (object?)transactionRef ?? DBNull.Value;
            cmd.Parameters.Add("@ChequeNumber", SqlDbType.NVarChar, 50).Value = (object?)chequeNo ?? DBNull.Value;
            cmd.Parameters.Add("@BankName", SqlDbType.NVarChar, 100).Value = (object?)bankName ?? DBNull.Value;
            cmd.Parameters.Add("@ChequeDate", SqlDbType.Date).Value = (object?)chequeDate ?? DBNull.Value;
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, -1).Value = (object?)notes ?? DBNull.Value;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = createdBy;
            cmd.Parameters.Add("@SelectedInvoiceIds", SqlDbType.NVarChar, -1).Value = (object?)selectedInvoiceIds ?? DBNull.Value;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var payId = reader.GetGuid(reader.GetOrdinal("PaymentId"));
                var recNo = reader["ReceiptNumber"]?.ToString() ?? string.Empty;
                var totalAlloc = Convert.ToDecimal(reader["TotalAllocated"]);
                var advCred = Convert.ToDecimal(reader["AdvanceCredited"]);
                return (payId, recNo, totalAlloc, advCred);
            }
            return (Guid.Empty, string.Empty, 0m, 0m);
        }

        public async Task<(bool Success, string Message)> CancelPaymentAsync(Guid tenantId, Guid paymentId, string reason, Guid cancelledBy)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_CancelOrRefundPayment", conn);
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@PaymentId", SqlDbType.UniqueIdentifier).Value = paymentId;
            cmd.Parameters.Add("@Reason", SqlDbType.NVarChar, 500).Value = reason;
            cmd.Parameters.Add("@CancelledBy", SqlDbType.UniqueIdentifier).Value = cancelledBy;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var success = Convert.ToInt32(reader["Success"]) == 1;
                var msg = reader["Message"]?.ToString() ?? "Processed";
                return (success, msg);
            }
            return (false, "Payment cancellation failed.");
        }

        public async Task<StudentFeeLedgerViewModel?> GetStudentFeeLedgerAsync(Guid studentId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_GetStudentFeeLedger", conn);
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@StudentId", SqlDbType.UniqueIdentifier).Value = studentId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;

            var vm = new StudentFeeLedgerViewModel
            {
                StudentId = reader.GetGuid(reader.GetOrdinal("StudentId")),
                StudentCode = reader["StudentCode"]?.ToString() ?? string.Empty,
                AdmissionNumber = reader["AdmissionNumber"]?.ToString() ?? string.Empty,
                StudentName = reader["StudentName"]?.ToString() ?? string.Empty,
                Gender = reader["Gender"]?.ToString(),
                Phone = reader["Phone"]?.ToString(),
                Email = reader["Email"]?.ToString(),
                CourseName = reader["CourseName"]?.ToString(),
                BatchName = reader["BatchName"]?.ToString(),
                AcademicYearName = reader["AcademicYearName"]?.ToString(),
                BranchName = reader["BranchName"]?.ToString(),
                GuardianName = reader["GuardianName"]?.ToString(),
                TotalBilled = Convert.ToDecimal(reader["TotalBilled"]),
                TotalPaid = Convert.ToDecimal(reader["TotalPaid"]),
                OutstandingDue = Convert.ToDecimal(reader["OutstandingDue"]),
                AdvanceWalletBalance = Convert.ToDecimal(reader["AdvanceWalletBalance"])
            };

            // Set 2: Invoices
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    vm.Invoices.Add(new FeeInvoiceListItemViewModel
                    {
                        FI_Id = reader.GetGuid(reader.GetOrdinal("InvoiceId")),
                        FI_InvoiceNumber = reader["InvoiceNumber"]?.ToString() ?? string.Empty,
                        FI_InvoiceDate = Convert.ToDateTime(reader["InvoiceDate"]),
                        FI_DueDate = Convert.ToDateTime(reader["DueDate"]),
                        FI_InvoiceType = reader["InvoiceType"]?.ToString(),
                        FI_BillingPeriod = reader["BillingPeriod"]?.ToString(),
                        FI_TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        FI_PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
                        FI_BalanceAmount = Convert.ToDecimal(reader["BalanceAmount"]),
                        FI_Status = reader["Status"]?.ToString() ?? string.Empty,
                        FeeHeadsSummary = reader["FeeHeadsSummary"]?.ToString()
                    });
                }
            }

            // Set 3: Payments
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    vm.Payments.Add(new PaymentListItemViewModel
                    {
                        PAY_Id = reader.GetGuid(reader.GetOrdinal("PaymentId")),
                        PAY_PaymentNumber = reader["ReceiptNumber"]?.ToString() ?? string.Empty,
                        PAY_PaymentDate = Convert.ToDateTime(reader["PaymentDate"]),
                        PAY_Amount = Convert.ToDecimal(reader["Amount"]),
                        PaymentMethodName = reader["PaymentMethod"]?.ToString() ?? string.Empty,
                        TransactionRef = reader["TransactionRef"]?.ToString(),
                        PAY_Status = reader["Status"]?.ToString() ?? string.Empty,
                        CashierName = reader["CashierName"]?.ToString()
                    });
                }
            }

            // Set 4: Advances
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    vm.Advances.Add(new StudentAdvanceItemViewModel
                    {
                        AdvanceId = reader.GetGuid(reader.GetOrdinal("AdvanceId")),
                        PaymentId = reader.GetGuid(reader.GetOrdinal("PaymentId")),
                        OriginatingReceiptNumber = reader["OriginatingReceiptNumber"]?.ToString() ?? string.Empty,
                        OriginalAmount = Convert.ToDecimal(reader["OriginalAmount"]),
                        AvailableBalance = Convert.ToDecimal(reader["AvailableBalance"]),
                        Status = reader["Status"]?.ToString() ?? string.Empty,
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    });
                }
            }

            return vm;
        }

        public async Task<DailyCollectionReportViewModel> GetDailyCollectionReportAsync(
            Guid tenantId, DateTime fromDate, DateTime toDate, Guid? paymentMethodId, Guid? cashierId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_GetDailyCollectionReport", conn);
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;
            cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
            cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;
            cmd.Parameters.Add("@PaymentMethodId", SqlDbType.UniqueIdentifier).Value = (object?)paymentMethodId ?? DBNull.Value;
            cmd.Parameters.Add("@CashierId", SqlDbType.UniqueIdentifier).Value = (object?)cashierId ?? DBNull.Value;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            var vm = new DailyCollectionReportViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                PaymentMethodId = paymentMethodId,
                CashierId = cashierId
            };

            // Set 1: Summaries
            while (await reader.ReadAsync())
            {
                string method = reader["PaymentMethodName"]?.ToString() ?? string.Empty;
                decimal amount = Convert.ToDecimal(reader["TotalCollected"]);
                vm.GrandTotal += amount;

                if (method.IndexOf("Cash", StringComparison.OrdinalIgnoreCase) >= 0)
                    vm.CashTotal += amount;
                else if (method.IndexOf("UPI", StringComparison.OrdinalIgnoreCase) >= 0 || method.IndexOf("QR", StringComparison.OrdinalIgnoreCase) >= 0)
                    vm.UpiTotal += amount;
                else if (method.IndexOf("Cheque", StringComparison.OrdinalIgnoreCase) >= 0 || method.IndexOf("Draft", StringComparison.OrdinalIgnoreCase) >= 0)
                    vm.ChequeTotal += amount;
                else
                    vm.BankTotal += amount;
            }

            // Set 2: Itemized Rows
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    vm.Items.Add(new DailyCollectionItemViewModel
                    {
                        PaymentId = reader.GetGuid(reader.GetOrdinal("PaymentId")),
                        ReceiptNumber = reader["ReceiptNumber"]?.ToString() ?? string.Empty,
                        PaymentDate = Convert.ToDateTime(reader["PaymentDate"]),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        PaymentMethod = reader["PaymentMethod"]?.ToString() ?? string.Empty,
                        TransactionRef = reader["TransactionRef"]?.ToString(),
                        ChequeNumber = reader["ChequeNumber"]?.ToString(),
                        Status = reader["Status"]?.ToString() ?? string.Empty,
                        StudentCode = reader["StudentCode"]?.ToString() ?? string.Empty,
                        StudentName = reader["StudentName"]?.ToString() ?? string.Empty,
                        CourseName = reader["CourseName"]?.ToString(),
                        BatchName = reader["BatchName"]?.ToString(),
                        CashierName = reader["CashierName"]?.ToString()
                    });
                }
            }

            return vm;
        }

        public async Task<FeeReceiptPrintViewModel?> GetReceiptForPrintAsync(Guid paymentId, Guid tenantId)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_Portal_GetReceiptDetails", conn);
            cmd.Parameters.Add("@PaymentId", SqlDbType.UniqueIdentifier).Value = paymentId;
            cmd.Parameters.Add("@TenantId", SqlDbType.UniqueIdentifier).Value = tenantId;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;

            var vm = new FeeReceiptPrintViewModel
            {
                PaymentId = (Guid)reader["PaymentId"],
                ReceiptNumber = reader["ReceiptNumber"]?.ToString() ?? string.Empty,
                PaymentDate = Convert.ToDateTime(reader["PaymentDate"]),
                PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
                TransactionRef = reader["TransactionRef"]?.ToString(),
                PaymentMode = reader["PaymentMode"]?.ToString() ?? "Cash",
                StudentName = reader["StudentName"]?.ToString() ?? string.Empty,
                StudentCode = reader["StudentCode"]?.ToString() ?? string.Empty,
                AdmissionNumber = reader["AdmissionNumber"]?.ToString() ?? string.Empty,
                CourseName = reader["CourseName"]?.ToString(),
                BatchName = reader["BatchName"]?.ToString(),
                BranchName = reader["BranchName"]?.ToString(),
                BranchAddress = reader["BranchAddress"]?.ToString(),
                BranchPhone = reader["BranchPhone"]?.ToString(),
                OrganizationName = reader["OrganizationName"]?.ToString() ?? "INSTITUTE MANAGEMENT SYSTEM",
                OrganizationLogoUrl = reader["OrganizationLogoUrl"]?.ToString()
            };

            // Result Set 2: Allocated Invoices
            if (await reader.NextResultAsync())
            {
                int sl = 1;
                while (await reader.ReadAsync())
                {
                    string invNo = reader["InvoiceNumber"]?.ToString() ?? string.Empty;
                    decimal amt = Convert.ToDecimal(reader["AmountPaid"]);

                    vm.AllocatedInvoices.Add(new ReceiptAllocationItemDto
                    {
                        InvoiceNumber = invNo,
                        AmountPaid = amt
                    });

                    vm.FeeHeads.Add(new ReceiptFeeHeadItemDto
                    {
                        SlNo = sl++,
                        FeeHeadName = "Fee Settlement against Inv #" + invNo,
                        Period = "Academic Session",
                        Amount = amt,
                        Concession = 0,
                        NetAmount = amt
                    });
                }
            }

            vm.SubTotal = vm.PaidAmount;
            vm.AmountInWords = ConvertNumberToWords(vm.PaidAmount);

            return vm;
        }

        public async Task<bool> UpdateAsync(Payment payment)
        {
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("USP_Payments_PAY", conn);
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "Update";
            cmd.Parameters.Add("@PAY_Id", SqlDbType.UniqueIdentifier).Value = payment.PAY_Id;
            cmd.Parameters.Add("@PAY_TenantId", SqlDbType.UniqueIdentifier).Value = payment.PAY_TenantId;
            cmd.Parameters.Add("@PAY_PaymentNumber", SqlDbType.NVarChar, 50).Value = payment.PAY_PaymentNumber;
            cmd.Parameters.Add("@PAY_PaymentDate", SqlDbType.DateTime2).Value = payment.PAY_PaymentDate;
            cmd.Parameters.Add("@PAY_Amount", SqlDbType.Decimal).Value = payment.PAY_Amount;
            cmd.Parameters.Add("@PAY_PaymentMethodId", SqlDbType.UniqueIdentifier).Value = payment.PAY_PaymentMethodId;
            cmd.Parameters.Add("@PAY_Status", SqlDbType.NVarChar, 20).Value = payment.PAY_Status;
            cmd.Parameters.Add("@PAY_TransactionReference", SqlDbType.NVarChar, 150).Value = (object?)payment.PAY_TransactionReference ?? DBNull.Value;
            cmd.Parameters.Add("@PAY_Notes", SqlDbType.NVarChar, -1).Value = (object?)payment.PAY_Notes ?? DBNull.Value;

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid tenantId)
        {
            var res = await CancelPaymentAsync(tenantId, id, "Deleted by user", Guid.Empty);
            return res.Success;
        }

        private static Payment MapPayment(SqlDataReader r) => new()
        {
            PAY_Id = r.GetGuid(r.GetOrdinal("PAY_Id")),
            PAY_TenantId = r.GetGuid(r.GetOrdinal("PAY_TenantId")),
            PAY_StudentId = r.GetGuid(r.GetOrdinal("PAY_StudentId")),
            PAY_PaymentNumber = r["PAY_PaymentNumber"] as string ?? string.Empty,
            PAY_PaymentDate = r.GetDateTime(r.GetOrdinal("PAY_PaymentDate")),
            PAY_Amount = r.GetDecimal(r.GetOrdinal("PAY_Amount")),
            PAY_PaymentMethodId = r.GetGuid(r.GetOrdinal("PAY_PaymentMethodId")),
            PAY_Status = r["PAY_Status"] as string ?? string.Empty,
            PAY_TransactionReference = r["PAY_TransactionReference"] as string,
            PAY_Notes = r["PAY_Notes"] as string,
            PAY_CreatedBy = r.GetGuid(r.GetOrdinal("PAY_CreatedBy")),
            PAY_CreatedAt = r.GetDateTime(r.GetOrdinal("PAY_CreatedAt")),
            PAY_UpdatedAt = r.GetDateTime(r.GetOrdinal("PAY_UpdatedAt")),
            StudentName = r["StudentName"] as string,
            StudentCode = r["StudentCode"] as string,
            PaymentMethodName = r["PaymentMethodName"] as string
        };

        private static string ConvertNumberToWords(decimal number)
        {
            long intPart = (long)Math.Floor(number);
            int decPart = (int)Math.Round((number - intPart) * 100);

            string words = ConvertWholeNumberToWords(intPart) + " Rupees";
            if (decPart > 0)
                words += " and " + ConvertWholeNumberToWords(decPart) + " Paise";

            return words + " Only.";
        }

        private static string ConvertWholeNumberToWords(long n)
        {
            if (n == 0) return "Zero";
            if (n < 0) return "Minus " + ConvertWholeNumberToWords(-n);

            string[] units = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            string words = "";

            if (n >= 10000000)
            {
                words += ConvertWholeNumberToWords(n / 10000000) + " Crore ";
                n %= 10000000;
            }
            if (n >= 100000)
            {
                words += ConvertWholeNumberToWords(n / 100000) + " Lakh ";
                n %= 100000;
            }
            if (n >= 1000)
            {
                words += ConvertWholeNumberToWords(n / 1000) + " Thousand ";
                n %= 1000;
            }
            if (n >= 100)
            {
                words += ConvertWholeNumberToWords(n / 100) + " Hundred ";
                n %= 100;
            }
            if (n > 0)
            {
                if (n < 20)
                    words += units[n];
                else
                {
                    words += tens[n / 10];
                    if ((n % 10) > 0) words += " " + units[n % 10];
                }
            }

            return words.Trim();
        }
    }
}
