using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Models.ViewModels
{
    public class FeeStructureListItemViewModel
    {
        public Guid FS_Id { get; set; }
        public string FS_Code { get; set; } = string.Empty;
        public string FS_Name { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string BatchName { get; set; } = string.Empty;
        public string AcademicYearName { get; set; } = string.Empty;
        public bool FS_IsActive { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class FeeStructureIndexViewModel
    {
        public List<FeeStructureListItemViewModel> FeeStructures { get; set; } = new();
        public string? SearchTerm { get; set; }
        public Guid? AcademicYearFilter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public List<SelectListItem> AcademicYearOptions { get; set; } = new();
    }

    public class FeeStructureFormViewModel
    {
        public Guid? FS_Id { get; set; }
        public string FS_Name { get; set; } = string.Empty;
        public string FS_Code { get; set; } = string.Empty;
        public Guid? FS_CourseId { get; set; }
        public Guid? FS_BatchId { get; set; }
        public Guid FS_AcademicYearId { get; set; }
        public string? FS_Description { get; set; }
        public bool FS_IsActive { get; set; } = true;

        public List<FeeStructureItemFormViewModel> Items { get; set; } = new();

        public List<SelectListItem> CourseOptions { get; set; } = new();
        public List<SelectListItem> BatchOptions { get; set; } = new();
        public List<SelectListItem> AcademicYearOptions { get; set; } = new();
        public List<SelectListItem> FeeCategoryOptions { get; set; } = new();
    }

    public class FeeStructureItemFormViewModel
    {
        public Guid? FSI_Id { get; set; }
        public Guid FSI_FeeCategoryId { get; set; }
        public decimal FSI_Amount { get; set; }
        public int? FSI_DueDays { get; set; }
        public bool FSI_IsMandatory { get; set; } = true;
    }

    public class FeeInvoiceListItemViewModel
    {
        public Guid FI_Id { get; set; }
        public Guid StudentId { get; set; }
        public string FI_InvoiceNumber { get; set; } = string.Empty;
        public string StudentCode { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public DateTime FI_InvoiceDate { get; set; }
        public DateTime FI_DueDate { get; set; }
        public decimal FI_TotalAmount { get; set; }
        public decimal FI_PaidAmount { get; set; }
        public decimal FI_BalanceAmount { get; set; }
        public string FI_Status { get; set; } = string.Empty;
        public string? FI_InvoiceType { get; set; }
        public string? FI_BillingPeriod { get; set; }
        public string? FeeHeadsSummary { get; set; }
    }

    public class FeeInvoiceIndexViewModel
    {
        public List<FeeInvoiceListItemViewModel> Invoices { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? StatusFilter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public List<SelectListItem> StatusOptions { get; set; } = new();
    }

    public class FeeInvoiceFormViewModel
    {
        public Guid? FI_Id { get; set; }
        public Guid FI_StudentId { get; set; }
        public DateTime FI_InvoiceDate { get; set; } = DateTime.Today;
        public DateTime FI_DueDate { get; set; } = DateTime.Today.AddDays(30);
        public decimal FI_DiscountAmount { get; set; }
        public decimal FI_TaxAmount { get; set; }
        public string? FI_Notes { get; set; }
        public string? FI_InvoiceType { get; set; } = "scheduled_session";
        public string? FI_BillingPeriod { get; set; }

        public List<FeeInvoiceItemFormViewModel> Items { get; set; } = new();

        public List<SelectListItem> StudentOptions { get; set; } = new();
        public List<SelectListItem> FeeCategoryOptions { get; set; } = new();
    }

    public class FeeInvoiceItemFormViewModel
    {
        public Guid? FII_Id { get; set; }
        public Guid FII_FeeCategoryId { get; set; }
        public string FII_Description { get; set; } = string.Empty;
        public decimal FII_Quantity { get; set; } = 1;
        public decimal FII_UnitAmount { get; set; }
        public decimal FII_DiscountAmount { get; set; }
        public decimal FII_TaxAmount { get; set; }
    }

    public class PaymentListItemViewModel
    {
        public Guid PAY_Id { get; set; }
        public Guid StudentId { get; set; }
        public string PAY_PaymentNumber { get; set; } = string.Empty;
        public string StudentCode { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public DateTime PAY_PaymentDate { get; set; }
        public decimal PAY_Amount { get; set; }
        public string PaymentMethodName { get; set; } = string.Empty;
        public string PAY_Status { get; set; } = string.Empty;
        public string? TransactionRef { get; set; }
        public string? CashierName { get; set; }
    }

    public class PaymentIndexViewModel
    {
        public List<PaymentListItemViewModel> Payments { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? StatusFilter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public List<SelectListItem> StatusOptions { get; set; } = new();
    }

    public class PaymentFormViewModel
    {
        public Guid? PAY_Id { get; set; }
        public Guid PAY_StudentId { get; set; }
        public DateTime PAY_PaymentDate { get; set; } = DateTime.Today;
        public decimal PAY_Amount { get; set; }
        public Guid PAY_PaymentMethodId { get; set; }
        public string? PAY_TransactionReference { get; set; }
        public string? PAY_ChequeNumber { get; set; }
        public string? PAY_BankName { get; set; }
        public DateTime? PAY_ChequeDate { get; set; }
        public string? PAY_Notes { get; set; }

        public List<SelectListItem> StudentOptions { get; set; } = new();
        public List<SelectListItem> PaymentMethodOptions { get; set; } = new();
    }

    // =========================================================================
    // POS Fee Collection Desk Models
    // =========================================================================

    public class FeeCollectViewModel
    {
        public Guid? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public string? AdmissionNumber { get; set; }
        public string? CourseName { get; set; }
        public string? BatchName { get; set; }
        public string? BranchName { get; set; }
        public string? GuardianName { get; set; }
        public string? Phone { get; set; }

        public decimal TotalBilled { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal OutstandingDue { get; set; }
        public decimal AdvanceWalletBalance { get; set; }

        public List<PendingInvoiceItemViewModel> PendingInvoices { get; set; } = new();

        public Guid PaymentMethodId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Today;
        public decimal AmountTendered { get; set; }
        public string? TransactionReference { get; set; }
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string? Notes { get; set; }

        public List<SelectListItem> PaymentMethodOptions { get; set; } = new();
        public List<SelectListItem> FeeCategoryOptions { get; set; } = new();
    }

    public class PendingInvoiceItemViewModel
    {
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string? InvoiceType { get; set; }
        public string? BillingPeriod { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? FeeHeadsSummary { get; set; }
        public bool IsSelected { get; set; } = true;
        public decimal PayAmount { get; set; }
    }

    public class FeeCollectSubmissionModel
    {
        public Guid StudentId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Today;
        public decimal Amount { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string? TransactionReference { get; set; }
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string? Notes { get; set; }
        public string? SelectedInvoiceIds { get; set; } // Comma-separated list of selected invoice GUIDs
    }

    // =========================================================================
    // Student Fee Ledger (Passbook / খতিয়ান) Models
    // =========================================================================

    public class StudentFeeLedgerViewModel
    {
        public Guid StudentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string AdmissionNumber { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? CourseName { get; set; }
        public string? BatchName { get; set; }
        public string? AcademicYearName { get; set; }
        public string? BranchName { get; set; }
        public string? GuardianName { get; set; }

        public decimal TotalBilled { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal OutstandingDue { get; set; }
        public decimal AdvanceWalletBalance { get; set; }

        public List<FeeInvoiceListItemViewModel> Invoices { get; set; } = new();
        public List<PaymentListItemViewModel> Payments { get; set; } = new();
        public List<StudentAdvanceItemViewModel> Advances { get; set; } = new();
    }

    public class StudentAdvanceItemViewModel
    {
        public Guid AdvanceId { get; set; }
        public Guid PaymentId { get; set; }
        public string OriginatingReceiptNumber { get; set; } = string.Empty;
        public decimal OriginalAmount { get; set; }
        public decimal AvailableBalance { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    // =========================================================================
    // Cashier Day-End Collection Scroll
    // =========================================================================

    public class DailyCollectionReportViewModel
    {
        public DateTime FromDate { get; set; } = DateTime.Today;
        public DateTime ToDate { get; set; } = DateTime.Today;
        public Guid? PaymentMethodId { get; set; }
        public Guid? CashierId { get; set; }

        public decimal CashTotal { get; set; }
        public decimal UpiTotal { get; set; }
        public decimal BankTotal { get; set; }
        public decimal ChequeTotal { get; set; }
        public decimal GrandTotal { get; set; }

        public List<DailyCollectionItemViewModel> Items { get; set; } = new();
        public List<SelectListItem> PaymentMethodOptions { get; set; } = new();
    }

    public class DailyCollectionItemViewModel
    {
        public Guid PaymentId { get; set; }
        public string ReceiptNumber { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? TransactionRef { get; set; }
        public string? ChequeNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StudentCode { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string? CourseName { get; set; }
        public string? BatchName { get; set; }
        public string? CashierName { get; set; }
    }

    // =========================================================================
    // Official Money Receipt (মানি রসিদ) Printable DTO
    // =========================================================================

    public class FeeReceiptPrintViewModel
    {
        public Guid PaymentId { get; set; }
        public string ReceiptNumber { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public decimal PaidAmount { get; set; }
        public string? TransactionRef { get; set; }
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string PaymentMode { get; set; } = string.Empty;

        // Institution Header
        public string OrganizationName { get; set; } = string.Empty;
        public string? OrganizationLogoUrl { get; set; }
        public string? BranchName { get; set; }
        public string? BranchAddress { get; set; }
        public string? BranchPhone { get; set; }
        public string? AffiliationInfo { get; set; } = "Affiliated to W.B.B.S.E. / W.B.C.H.S.E.";
        public string? UDISECode { get; set; }

        // Student Details
        public string StudentName { get; set; } = string.Empty;
        public string StudentCode { get; set; } = string.Empty;
        public string AdmissionNumber { get; set; } = string.Empty;
        public string? CourseName { get; set; }
        public string? BatchName { get; set; }
        public string? AcademicSession { get; set; }

        // Itemized Breakdowns
        public List<ReceiptAllocationItemDto> AllocatedInvoices { get; set; } = new();
        public List<ReceiptFeeHeadItemDto> FeeHeads { get; set; } = new();

        public decimal SubTotal { get; set; }
        public decimal TotalConcession { get; set; }
        public decimal AdvanceAdjusted { get; set; }
        public decimal OutstandingDue { get; set; }
        public string AmountInWords { get; set; } = string.Empty;
        public string? CashierName { get; set; }
    }

    public class ReceiptAllocationItemDto
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public string? BillingPeriod { get; set; }
    }

    public class ReceiptFeeHeadItemDto
    {
        public int SlNo { get; set; }
        public string FeeHeadName { get; set; } = string.Empty;
        public string? Period { get; set; }
        public decimal Amount { get; set; }
        public decimal Concession { get; set; }
        public decimal NetAmount { get; set; }
    }

    // =========================================================================
    // Quick Ad-Hoc Charge & Scheduled Bulk Billing DTOs
    // =========================================================================

    public class AdHocChargeFormViewModel
    {
        public Guid StudentId { get; set; }
        public Guid FeeCategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; } = DateTime.Today;
    }

    public class BatchInvoiceGenerateViewModel
    {
        public Guid AcademicYearId { get; set; }
        public Guid CourseId { get; set; }
        public Guid? BatchId { get; set; }
        public Guid FeeStructureId { get; set; }
        public string BillingPeriod { get; set; } = DateTime.Today.ToString("yyyy-MM");
        public DateTime InvoiceDate { get; set; } = DateTime.Today;
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(15);

        public List<SelectListItem> AcademicYearOptions { get; set; } = new();
        public List<SelectListItem> CourseOptions { get; set; } = new();
        public List<SelectListItem> BatchOptions { get; set; } = new();
        public List<SelectListItem> FeeStructureOptions { get; set; } = new();
    }

    public class CancelPaymentViewModel
    {
        public Guid PaymentId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
