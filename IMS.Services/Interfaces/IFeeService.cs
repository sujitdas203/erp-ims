using System;
using System.Threading.Tasks;
using IMS.Models.ViewModels;

namespace IMS.Services.Interfaces
{
    public interface IFeeService
    {
        // Fee Structure
        Task<FeeStructureIndexViewModel> GetFeeStructureListAsync(Guid tenantId, string? searchTerm, Guid? academicYearId, int pageNumber, int pageSize);
        Task<FeeStructureFormViewModel?> GetFeeStructureForEditAsync(Guid id, Guid tenantId);
        Task<ServiceResult> CreateFeeStructureAsync(FeeStructureFormViewModel model, Guid tenantId);
        Task<ServiceResult> UpdateFeeStructureAsync(FeeStructureFormViewModel model, Guid tenantId);
        Task<ServiceResult> DeleteFeeStructureAsync(Guid id, Guid tenantId);
        void PopulateFeeStructureDropdowns(FeeStructureFormViewModel vm, Guid tenantId);

        // Invoices
        Task<FeeInvoiceIndexViewModel> GetFeeInvoiceListAsync(Guid tenantId, string? searchTerm, string? status, int pageNumber, int pageSize);
        Task<FeeInvoiceFormViewModel?> GetFeeInvoiceForEditAsync(Guid id, Guid tenantId);
        Task<ServiceResult> CreateFeeInvoiceAsync(FeeInvoiceFormViewModel model, Guid tenantId);
        Task<ServiceResult> DeleteFeeInvoiceAsync(Guid id, Guid tenantId);
        void PopulateFeeInvoiceDropdowns(FeeInvoiceFormViewModel vm, Guid tenantId);
        Task<decimal> GetTotalDueByStudentAsync(Guid studentId, Guid tenantId);

        // Payments (Standard List / Basic)
        Task<PaymentIndexViewModel> GetPaymentListAsync(Guid tenantId, string? searchTerm, string? status, int pageNumber, int pageSize);
        Task<PaymentFormViewModel?> GetPaymentForEditAsync(Guid id, Guid tenantId);
        Task<ServiceResult> CreatePaymentAsync(PaymentFormViewModel model, Guid tenantId, Guid createdBy);
        Task<ServiceResult> DeletePaymentAsync(Guid id, Guid tenantId);
        void PopulatePaymentDropdowns(PaymentFormViewModel vm, Guid tenantId);

        // Point-of-Sale Fee Collection Desk & Receipts
        Task<FeeCollectViewModel> GetFeeCollectDeskAsync(Guid? studentId, Guid tenantId);
        Task<ServiceResult> ProcessFeeCollectionAsync(FeeCollectSubmissionModel model, Guid tenantId, Guid createdBy);
        Task<FeeReceiptPrintViewModel?> GetReceiptForPrintAsync(Guid paymentId, Guid tenantId);
        Task<ServiceResult> CancelPaymentAsync(Guid paymentId, string reason, Guid tenantId, Guid cancelledBy);

        // Passbook Ledger & Day-End Cash Scroll
        Task<StudentFeeLedgerViewModel?> GetStudentFeeLedgerAsync(Guid studentId, Guid tenantId);
        Task<DailyCollectionReportViewModel> GetDailyCollectionReportAsync(Guid tenantId, DateTime fromDate, DateTime toDate, Guid? paymentMethodId, Guid? cashierId);

        // On-Demand & Scheduled Billing
        Task<ServiceResult> CreateAdHocChargeAsync(AdHocChargeFormViewModel model, Guid tenantId, Guid createdBy);
        Task<ServiceResult> GenerateBatchInvoicesAsync(BatchInvoiceGenerateViewModel model, Guid tenantId, Guid createdBy);
        void PopulateBatchInvoiceDropdowns(BatchInvoiceGenerateViewModel vm, Guid tenantId);
    }
}
