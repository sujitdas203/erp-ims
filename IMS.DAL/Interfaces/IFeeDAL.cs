using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMS.Models.Entities;
using IMS.Models.ViewModels;

namespace IMS.DAL.Interfaces
{
    public interface IFeeStructureDAL
    {
        Task<FeeStructure?> GetByIdAsync(Guid id, Guid tenantId);
        Task<(List<FeeStructure> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, string? searchTerm, Guid? academicYearId,
            int pageNumber, int pageSize);
        Task<bool> IsCodeTakenAsync(Guid tenantId, string code, Guid? excludeId);
        Task<Guid> CreateAsync(FeeStructure feeStructure);
        Task<bool> UpdateAsync(FeeStructure feeStructure);
        Task<bool> DeleteAsync(Guid id, Guid tenantId);
    }

    public interface IFeeStructureItemDAL
    {
        Task<List<FeeStructureItem>> GetByFeeStructureIdAsync(Guid feeStructureId);
        Task<bool> SaveItemsAsync(Guid feeStructureId, List<FeeStructureItem> items);
    }

    public interface IFeeInvoiceDAL
    {
        Task<FeeInvoice?> GetByIdAsync(Guid id, Guid tenantId);
        Task<(List<FeeInvoice> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, string? searchTerm, string? status,
            int pageNumber, int pageSize);
        Task<List<FeeInvoice>> GetPendingByStudentAsync(Guid studentId, Guid tenantId);
        Task<string> GetNextInvoiceNumberAsync(Guid tenantId);
        Task<Guid> CreateAsync(FeeInvoice invoice);
        Task<bool> UpdateAsync(FeeInvoice invoice);
        Task<bool> UpdatePaidAmountAsync(Guid invoiceId, decimal paidAmount);
        Task<bool> DeleteAsync(Guid id, Guid tenantId);
        Task<decimal> GetTotalDueByStudentAsync(Guid studentId, Guid tenantId);
        Task<(Guid InvoiceId, string InvoiceNumber)> CreateAdHocChargeAsync(
            Guid tenantId, Guid studentId, Guid feeCategoryId, decimal amount, 
            string description, DateTime? dueDate, Guid createdBy);
        Task<(int GeneratedCount, decimal TotalAmount)> GenerateBatchInvoicesAsync(
            Guid tenantId, Guid academicYearId, Guid courseId, Guid? batchId, 
            Guid feeStructureId, string billingPeriod, DateTime invoiceDate, DateTime dueDate, Guid createdBy);
        Task<List<FeeInvoiceItem>> GetItemsByInvoiceIdAsync(Guid invoiceId);
        Task<bool> SaveInvoiceItemsAsync(Guid invoiceId, List<FeeInvoiceItem> items);
    }

    public interface IPaymentDAL
    {
        Task<Payment?> GetByIdAsync(Guid id, Guid tenantId);
        Task<(List<Payment> Items, int TotalCount)> GetPagedAsync(
            Guid tenantId, string? searchTerm, string? status,
            int pageNumber, int pageSize);
        Task<string> GetNextPaymentNumberAsync(Guid tenantId);
        Task<Guid> CreateAsync(Payment payment);
        Task<(Guid PaymentId, string ReceiptNumber, decimal TotalAllocated, decimal AdvanceCredited)> ProcessPaymentAsync(
            Guid tenantId, Guid studentId, DateTime paymentDate, decimal amount, 
            Guid paymentMethodId, string? transactionRef, string? chequeNo, string? bankName, 
            DateTime? chequeDate, string? notes, Guid createdBy, string? selectedInvoiceIds);
        Task<(bool Success, string Message)> CancelPaymentAsync(Guid tenantId, Guid paymentId, string reason, Guid cancelledBy);
        Task<StudentFeeLedgerViewModel?> GetStudentFeeLedgerAsync(Guid studentId, Guid tenantId);
        Task<DailyCollectionReportViewModel> GetDailyCollectionReportAsync(Guid tenantId, DateTime fromDate, DateTime toDate, Guid? paymentMethodId, Guid? cashierId);
        Task<FeeReceiptPrintViewModel?> GetReceiptForPrintAsync(Guid paymentId, Guid tenantId);
        Task<bool> UpdateAsync(Payment payment);
        Task<bool> DeleteAsync(Guid id, Guid tenantId);
    }
}
