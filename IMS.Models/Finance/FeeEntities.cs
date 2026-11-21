using System;

namespace IMS.Models.Entities
{
    public class FeeStructure
    {
        public Guid FS_Id { get; set; }
        public Guid FS_TenantId { get; set; }
        public string FS_Name { get; set; } = string.Empty;
        public string FS_Code { get; set; } = string.Empty;
        public Guid? FS_CourseId { get; set; }
        public Guid? FS_BatchId { get; set; }
        public Guid FS_AcademicYearId { get; set; }
        public string? FS_Description { get; set; }
        public bool FS_IsActive { get; set; }
        public DateTime FS_CreatedAt { get; set; }
        public DateTime FS_UpdatedAt { get; set; }

        public string? CourseName { get; set; }
        public string? BatchName { get; set; }
        public string? AcademicYearName { get; set; }
    }

    public class FeeStructureItem
    {
        public Guid FSI_Id { get; set; }
        public Guid FSI_FeeStructureId { get; set; }
        public Guid FSI_FeeCategoryId { get; set; }
        public decimal FSI_Amount { get; set; }
        public int? FSI_DueDays { get; set; }
        public bool FSI_IsMandatory { get; set; }

        public string? FeeCategoryName { get; set; }
    }

    public class FeeInvoice
    {
        public Guid FI_Id { get; set; }
        public Guid FI_TenantId { get; set; }
        public Guid FI_StudentId { get; set; }
        public string FI_InvoiceNumber { get; set; } = string.Empty;
        public DateTime FI_InvoiceDate { get; set; }
        public DateTime FI_DueDate { get; set; }
        public decimal FI_Subtotal { get; set; }
        public decimal FI_DiscountAmount { get; set; }
        public decimal FI_TaxAmount { get; set; }
        public decimal FI_TotalAmount { get; set; }
        public decimal FI_PaidAmount { get; set; }
        public decimal FI_BalanceAmount { get; set; }
        public string FI_Status { get; set; } = string.Empty;
        public string? FI_InvoiceType { get; set; }
        public string? FI_BillingPeriod { get; set; }
        public string? FI_Notes { get; set; }
        public DateTime FI_CreatedAt { get; set; }
        public DateTime FI_UpdatedAt { get; set; }

        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public string? CourseName { get; set; }
        public string? BatchName { get; set; }
    }

    public class FeeInvoiceItem
    {
        public Guid FII_Id { get; set; }
        public Guid FII_InvoiceId { get; set; }
        public Guid FII_FeeCategoryId { get; set; }
        public string FII_Description { get; set; } = string.Empty;
        public decimal FII_Quantity { get; set; } = 1;
        public decimal FII_UnitAmount { get; set; }
        public decimal FII_DiscountAmount { get; set; }
        public decimal FII_TaxAmount { get; set; }
        public decimal FII_TotalAmount { get; set; }

        public string? FeeCategoryName { get; set; }
        public string? FeeCategoryCode { get; set; }
    }

    public class Payment
    {
        public Guid PAY_Id { get; set; }
        public Guid PAY_TenantId { get; set; }
        public Guid PAY_StudentId { get; set; }
        public string PAY_PaymentNumber { get; set; } = string.Empty;
        public DateTime PAY_PaymentDate { get; set; }
        public decimal PAY_Amount { get; set; }
        public Guid PAY_PaymentMethodId { get; set; }
        public string PAY_Status { get; set; } = string.Empty;
        public string? PAY_TransactionReference { get; set; }
        public string? PAY_ChequeNumber { get; set; }
        public string? PAY_BankName { get; set; }
        public DateTime? PAY_ChequeDate { get; set; }
        public string? PAY_ChequeClearingStatus { get; set; }
        public string? PAY_Notes { get; set; }
        public Guid PAY_CreatedBy { get; set; }
        public DateTime PAY_CreatedAt { get; set; }
        public DateTime PAY_UpdatedAt { get; set; }

        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public string? PaymentMethodName { get; set; }
        public string? CashierName { get; set; }
    }

    public class PaymentAllocation
    {
        public Guid PA_Id { get; set; }
        public Guid PA_PaymentId { get; set; }
        public Guid PA_InvoiceId { get; set; }
        public decimal PA_AllocatedAmount { get; set; }
        public DateTime PA_CreatedAt { get; set; }
    }

    public class StudentAdvance
    {
        public Guid SA_Id { get; set; }
        public Guid SA_TenantId { get; set; }
        public Guid SA_StudentId { get; set; }
        public Guid SA_PaymentId { get; set; }
        public decimal SA_OriginalAmount { get; set; }
        public decimal SA_AvailableBalance { get; set; }
        public string SA_Status { get; set; } = "active";
        public DateTime SA_CreatedAt { get; set; }
        public DateTime SA_UpdatedAt { get; set; }
    }

    public class FeeCategory
    {
        public Guid FC_Id { get; set; }
        public Guid FC_TenantId { get; set; }
        public string FC_Name { get; set; } = string.Empty;
        public string FC_Code { get; set; } = string.Empty;
        public string? FC_Description { get; set; }
        public bool FC_IsRefundable { get; set; }
        public string? FC_BillingFrequency { get; set; }
        public string? FC_ChargeType { get; set; }
        public bool FC_IsConcessionEligible { get; set; } = true;
        public string? FC_AccountHeadGroup { get; set; }
        public DateTime FC_CreatedAt { get; set; }
        public DateTime FC_UpdatedAt { get; set; }
    }
}
