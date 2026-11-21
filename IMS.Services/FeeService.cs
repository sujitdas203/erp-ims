using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.DAL.Interfaces;
using IMS.Helpers.Constants;
using IMS.Models.Entities;
using IMS.Models.ViewModels;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Services
{
    public class FeeService : IFeeService
    {
        private readonly IFeeStructureDAL _fsDAL;
        private readonly IFeeStructureItemDAL _fsiDAL;
        private readonly IFeeInvoiceDAL _fiDAL;
        private readonly IPaymentDAL _payDAL;
        private readonly IMasterService _masterService;

        public FeeService(
            IFeeStructureDAL fsDAL, 
            IFeeStructureItemDAL fsiDAL,
            IFeeInvoiceDAL fiDAL, 
            IPaymentDAL payDAL, 
            IMasterService masterService)
        {
            _fsDAL = fsDAL;
            _fsiDAL = fsiDAL;
            _fiDAL = fiDAL;
            _payDAL = payDAL;
            _masterService = masterService;
        }

        // ==================== Fee Structures ====================
        public async Task<FeeStructureIndexViewModel> GetFeeStructureListAsync(Guid tenantId, string? searchTerm, Guid? academicYearId, int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            var (items, totalCount) = await _fsDAL.GetPagedAsync(tenantId, searchTerm, academicYearId, pageNumber, pageSize);
            return new FeeStructureIndexViewModel
            {
                FeeStructures = items.Select(f => new FeeStructureListItemViewModel
                {
                    FS_Id = f.FS_Id, FS_Code = f.FS_Code, FS_Name = f.FS_Name,
                    CourseName = f.CourseName ?? "-", BatchName = f.BatchName ?? "-",
                    AcademicYearName = f.AcademicYearName ?? "-", FS_IsActive = f.FS_IsActive
                }).ToList(),
                SearchTerm = searchTerm, AcademicYearFilter = academicYearId,
                PageNumber = pageNumber, PageSize = pageSize, TotalCount = totalCount,
                AcademicYearOptions = GetMasterSelectList("AcademicYear", academicYearId?.ToString())
            };
        }

        public async Task<FeeStructureFormViewModel?> GetFeeStructureForEditAsync(Guid id, Guid tenantId)
        {
            var fs = await _fsDAL.GetByIdAsync(id, tenantId);
            if (fs == null) return null;
            var items = await _fsiDAL.GetByFeeStructureIdAsync(id);
            var vm = new FeeStructureFormViewModel
            {
                FS_Id = fs.FS_Id, FS_Name = fs.FS_Name, FS_Code = fs.FS_Code,
                FS_CourseId = fs.FS_CourseId, FS_BatchId = fs.FS_BatchId,
                FS_AcademicYearId = fs.FS_AcademicYearId, FS_Description = fs.FS_Description,
                FS_IsActive = fs.FS_IsActive,
                Items = items.Select(i => new FeeStructureItemFormViewModel
                {
                    FSI_Id = i.FSI_Id, FSI_FeeCategoryId = i.FSI_FeeCategoryId,
                    FSI_Amount = i.FSI_Amount, FSI_DueDays = i.FSI_DueDays, FSI_IsMandatory = i.FSI_IsMandatory
                }).ToList()
            };
            PopulateFeeStructureDropdowns(vm, tenantId);
            return vm;
        }

        public async Task<ServiceResult> CreateFeeStructureAsync(FeeStructureFormViewModel model, Guid tenantId)
        {
            if (!string.IsNullOrWhiteSpace(model.FS_Code) && await _fsDAL.IsCodeTakenAsync(tenantId, model.FS_Code, null))
                return ServiceResult.Fail("This fee structure code is already in use.");
            var entity = new FeeStructure 
            { 
                FS_Id = Guid.NewGuid(), 
                FS_TenantId = tenantId, 
                FS_Name = model.FS_Name, 
                FS_Code = model.FS_Code, 
                FS_CourseId = model.FS_CourseId, 
                FS_BatchId = model.FS_BatchId, 
                FS_AcademicYearId = model.FS_AcademicYearId, 
                FS_Description = model.FS_Description, 
                FS_IsActive = model.FS_IsActive 
            };
            var id = await _fsDAL.CreateAsync(entity);
            var fsiItems = model.Items.Select(i => new FeeStructureItem 
            { 
                FSI_Id = Guid.NewGuid(), 
                FSI_FeeStructureId = id, 
                FSI_FeeCategoryId = i.FSI_FeeCategoryId, 
                FSI_Amount = i.FSI_Amount, 
                FSI_DueDays = i.FSI_DueDays, 
                FSI_IsMandatory = i.FSI_IsMandatory 
            }).ToList();
            await _fsiDAL.SaveItemsAsync(id, fsiItems);
            return ServiceResult.Ok("Fee structure created successfully.", id);
        }

        public async Task<ServiceResult> UpdateFeeStructureAsync(FeeStructureFormViewModel model, Guid tenantId)
        {
            if (!model.FS_Id.HasValue) return ServiceResult.Fail("Fee Structure Id is required.");
            if (await _fsDAL.IsCodeTakenAsync(tenantId, model.FS_Code, model.FS_Id)) return ServiceResult.Fail("This code is already in use.");
            var entity = new FeeStructure 
            { 
                FS_Id = model.FS_Id.Value, 
                FS_TenantId = tenantId, 
                FS_Name = model.FS_Name, 
                FS_Code = model.FS_Code, 
                FS_CourseId = model.FS_CourseId, 
                FS_BatchId = model.FS_BatchId, 
                FS_AcademicYearId = model.FS_AcademicYearId, 
                FS_Description = model.FS_Description, 
                FS_IsActive = model.FS_IsActive 
            };
            var success = await _fsDAL.UpdateAsync(entity);
            var fsiItems = model.Items.Select(i => new FeeStructureItem 
            { 
                FSI_Id = i.FSI_Id ?? Guid.NewGuid(), 
                FSI_FeeStructureId = model.FS_Id.Value, 
                FSI_FeeCategoryId = i.FSI_FeeCategoryId, 
                FSI_Amount = i.FSI_Amount, 
                FSI_DueDays = i.FSI_DueDays, 
                FSI_IsMandatory = i.FSI_IsMandatory 
            }).ToList();
            await _fsiDAL.SaveItemsAsync(model.FS_Id.Value, fsiItems);
            return success ? ServiceResult.Ok("Fee structure updated successfully.") : ServiceResult.Fail("Fee structure not found.");
        }

        public async Task<ServiceResult> DeleteFeeStructureAsync(Guid id, Guid tenantId)
        {
            return await _fsDAL.DeleteAsync(id, tenantId) ? ServiceResult.Ok("Fee structure deleted successfully.") : ServiceResult.Fail("Unable to delete fee structure.");
        }

        public void PopulateFeeStructureDropdowns(FeeStructureFormViewModel vm, Guid tenantId)
        {
            vm.CourseOptions = GetMasterSelectList("Course", vm.FS_CourseId?.ToString());
            vm.BatchOptions = GetMasterSelectList("Batch", vm.FS_BatchId?.ToString());
            vm.AcademicYearOptions = GetMasterSelectList("AcademicYear", vm.FS_AcademicYearId.ToString());
            vm.FeeCategoryOptions = GetMasterSelectList("FeeCategory", null);
        }

        // ==================== Fee Invoices ====================
        public async Task<FeeInvoiceIndexViewModel> GetFeeInvoiceListAsync(Guid tenantId, string? searchTerm, string? status, int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            var (items, totalCount) = await _fiDAL.GetPagedAsync(tenantId, searchTerm, status, pageNumber, pageSize);
            return new FeeInvoiceIndexViewModel
            {
                Invoices = items.Select(i => new FeeInvoiceListItemViewModel
                {
                    FI_Id = i.FI_Id, StudentId = i.FI_StudentId, FI_InvoiceNumber = i.FI_InvoiceNumber,
                    StudentCode = i.StudentCode ?? "-", StudentName = i.StudentName ?? "-",
                    FI_InvoiceDate = i.FI_InvoiceDate, FI_DueDate = i.FI_DueDate,
                    FI_TotalAmount = i.FI_TotalAmount, FI_PaidAmount = i.FI_PaidAmount,
                    FI_BalanceAmount = i.FI_BalanceAmount, FI_Status = i.FI_Status,
                    FI_InvoiceType = i.FI_InvoiceType, FI_BillingPeriod = i.FI_BillingPeriod
                }).ToList(),
                SearchTerm = searchTerm, StatusFilter = status,
                PageNumber = pageNumber, PageSize = pageSize, TotalCount = totalCount,
                StatusOptions = GetInvoiceStatusSelectList(status)
            };
        }

        public async Task<FeeInvoiceFormViewModel?> GetFeeInvoiceForEditAsync(Guid id, Guid tenantId)
        {
            var fi = await _fiDAL.GetByIdAsync(id, tenantId);
            if (fi == null) return null;
            var items = await _fiDAL.GetItemsByInvoiceIdAsync(id);
            var vm = new FeeInvoiceFormViewModel
            {
                FI_Id = fi.FI_Id, FI_StudentId = fi.FI_StudentId,
                FI_InvoiceDate = fi.FI_InvoiceDate, FI_DueDate = fi.FI_DueDate,
                FI_DiscountAmount = fi.FI_DiscountAmount, FI_TaxAmount = fi.FI_TaxAmount,
                FI_Notes = fi.FI_Notes, FI_InvoiceType = fi.FI_InvoiceType,
                FI_BillingPeriod = fi.FI_BillingPeriod,
                Items = items.Select(x => new FeeInvoiceItemFormViewModel
                {
                    FII_Id = x.FII_Id, FII_FeeCategoryId = x.FII_FeeCategoryId,
                    FII_Description = x.FII_Description, FII_Quantity = x.FII_Quantity,
                    FII_UnitAmount = x.FII_UnitAmount, FII_DiscountAmount = x.FII_DiscountAmount,
                    FII_TaxAmount = x.FII_TaxAmount
                }).ToList()
            };
            PopulateFeeInvoiceDropdowns(vm, tenantId);
            return vm;
        }

        public async Task<ServiceResult> CreateFeeInvoiceAsync(FeeInvoiceFormViewModel model, Guid tenantId)
        {
            var invoiceNumber = await _fiDAL.GetNextInvoiceNumberAsync(tenantId);
            var subtotal = model.Items.Sum(i => i.FII_Quantity * i.FII_UnitAmount);
            var totalDiscount = model.Items.Sum(i => i.FII_DiscountAmount) + model.FI_DiscountAmount;
            var totalTax = model.Items.Sum(i => i.FII_TaxAmount) + model.FI_TaxAmount;
            var total = subtotal - totalDiscount + totalTax;

            var entity = new FeeInvoice
            {
                FI_Id = Guid.NewGuid(), FI_TenantId = tenantId, FI_StudentId = model.FI_StudentId,
                FI_InvoiceNumber = invoiceNumber, FI_InvoiceDate = model.FI_InvoiceDate,
                FI_DueDate = model.FI_DueDate, FI_Subtotal = subtotal, FI_DiscountAmount = totalDiscount,
                FI_TaxAmount = totalTax, FI_TotalAmount = total, FI_PaidAmount = 0,
                FI_BalanceAmount = total, FI_Status = "issued", FI_Notes = model.FI_Notes,
                FI_InvoiceType = model.FI_InvoiceType ?? "scheduled_session",
                FI_BillingPeriod = model.FI_BillingPeriod
            };
            var id = await _fiDAL.CreateAsync(entity);

            if (model.Items.Count > 0)
            {
                var fiiItems = model.Items.Select(x => new FeeInvoiceItem
                {
                    FII_Id = Guid.NewGuid(),
                    FII_InvoiceId = id,
                    FII_FeeCategoryId = x.FII_FeeCategoryId,
                    FII_Description = x.FII_Description ?? "Fee Charge",
                    FII_Quantity = x.FII_Quantity,
                    FII_UnitAmount = x.FII_UnitAmount,
                    FII_DiscountAmount = x.FII_DiscountAmount,
                    FII_TaxAmount = x.FII_TaxAmount,
                    FII_TotalAmount = (x.FII_Quantity * x.FII_UnitAmount) - x.FII_DiscountAmount + x.FII_TaxAmount
                }).ToList();
                await _fiDAL.SaveInvoiceItemsAsync(id, fiiItems);
            }

            return ServiceResult.Ok("Invoice created successfully.", id);
        }

        public async Task<ServiceResult> DeleteFeeInvoiceAsync(Guid id, Guid tenantId)
        {
            return await _fiDAL.DeleteAsync(id, tenantId) ? ServiceResult.Ok("Invoice deleted successfully.") : ServiceResult.Fail("Unable to delete invoice.");
        }

        public void PopulateFeeInvoiceDropdowns(FeeInvoiceFormViewModel vm, Guid tenantId)
        {
            vm.StudentOptions = GetMasterSelectList("Student", vm.FI_StudentId.ToString());
            vm.FeeCategoryOptions = GetMasterSelectList("FeeCategory", null);
        }

        public async Task<decimal> GetTotalDueByStudentAsync(Guid studentId, Guid tenantId)
        {
            return await _fiDAL.GetTotalDueByStudentAsync(studentId, tenantId);
        }

        // ==================== Payments ====================
        public async Task<PaymentIndexViewModel> GetPaymentListAsync(Guid tenantId, string? searchTerm, string? status, int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            var (items, totalCount) = await _payDAL.GetPagedAsync(tenantId, searchTerm, status, pageNumber, pageSize);
            return new PaymentIndexViewModel
            {
                Payments = items.Select(p => new PaymentListItemViewModel
                {
                    PAY_Id = p.PAY_Id, StudentId = p.PAY_StudentId, PAY_PaymentNumber = p.PAY_PaymentNumber,
                    StudentCode = p.StudentCode ?? "-", StudentName = p.StudentName ?? "-",
                    PAY_PaymentDate = p.PAY_PaymentDate, PAY_Amount = p.PAY_Amount,
                    PaymentMethodName = p.PaymentMethodName ?? "-", PAY_Status = p.PAY_Status,
                    TransactionRef = p.PAY_TransactionReference, CashierName = p.CashierName
                }).ToList(),
                SearchTerm = searchTerm, StatusFilter = status,
                PageNumber = pageNumber, PageSize = pageSize, TotalCount = totalCount,
                StatusOptions = GetPaymentStatusSelectList(status)
            };
        }

        public async Task<PaymentFormViewModel?> GetPaymentForEditAsync(Guid id, Guid tenantId)
        {
            var p = await _payDAL.GetByIdAsync(id, tenantId);
            if (p == null) return null;
            var vm = new PaymentFormViewModel
            {
                PAY_Id = p.PAY_Id, PAY_StudentId = p.PAY_StudentId,
                PAY_PaymentDate = p.PAY_PaymentDate, PAY_Amount = p.PAY_Amount,
                PAY_PaymentMethodId = p.PAY_PaymentMethodId,
                PAY_TransactionReference = p.PAY_TransactionReference,
                PAY_ChequeNumber = p.PAY_ChequeNumber,
                PAY_BankName = p.PAY_BankName,
                PAY_ChequeDate = p.PAY_ChequeDate,
                PAY_Notes = p.PAY_Notes
            };
            PopulatePaymentDropdowns(vm, tenantId);
            return vm;
        }

        public async Task<ServiceResult> CreatePaymentAsync(PaymentFormViewModel model, Guid tenantId, Guid createdBy)
        {
            var result = await _payDAL.ProcessPaymentAsync(
                tenantId, model.PAY_StudentId, model.PAY_PaymentDate, model.PAY_Amount,
                model.PAY_PaymentMethodId, model.PAY_TransactionReference,
                model.PAY_ChequeNumber, model.PAY_BankName, model.PAY_ChequeDate,
                model.PAY_Notes, createdBy, null);

            return ServiceResult.Ok($"Payment {result.ReceiptNumber} recorded successfully.", result.PaymentId);
        }

        public async Task<ServiceResult> DeletePaymentAsync(Guid id, Guid tenantId)
        {
            var res = await _payDAL.CancelPaymentAsync(tenantId, id, "Deleted via admin panel", Guid.Empty);
            return res.Success ? ServiceResult.Ok(res.Message) : ServiceResult.Fail(res.Message);
        }

        public void PopulatePaymentDropdowns(PaymentFormViewModel vm, Guid tenantId)
        {
            vm.StudentOptions = GetMasterSelectList("Student", vm.PAY_StudentId.ToString());
            vm.PaymentMethodOptions = GetMasterSelectList("PaymentMethod", vm.PAY_PaymentMethodId.ToString());
        }

        // ==================== Point-of-Sale Fee Collection Desk ====================
        public async Task<FeeCollectViewModel> GetFeeCollectDeskAsync(Guid? studentId, Guid tenantId)
        {
            var vm = new FeeCollectViewModel();
            vm.PaymentMethodOptions = GetMasterSelectList("PaymentMethod", null);
            vm.FeeCategoryOptions = GetMasterSelectList("FeeCategory", null);

            if (studentId.HasValue && studentId.Value != Guid.Empty)
            {
                var ledger = await _payDAL.GetStudentFeeLedgerAsync(studentId.Value, tenantId);
                if (ledger != null)
                {
                    vm.StudentId = ledger.StudentId;
                    vm.StudentName = ledger.StudentName;
                    vm.StudentCode = ledger.StudentCode;
                    vm.AdmissionNumber = ledger.AdmissionNumber;
                    vm.CourseName = ledger.CourseName;
                    vm.BatchName = ledger.BatchName;
                    vm.BranchName = ledger.BranchName;
                    vm.GuardianName = ledger.GuardianName;
                    vm.Phone = ledger.Phone;
                    vm.TotalBilled = ledger.TotalBilled;
                    vm.TotalPaid = ledger.TotalPaid;
                    vm.OutstandingDue = ledger.OutstandingDue;
                    vm.AdvanceWalletBalance = ledger.AdvanceWalletBalance;

                    var pending = await _fiDAL.GetPendingByStudentAsync(studentId.Value, tenantId);
                    vm.PendingInvoices = pending.Select(p => new PendingInvoiceItemViewModel
                    {
                        InvoiceId = p.FI_Id,
                        InvoiceNumber = p.FI_InvoiceNumber,
                        InvoiceDate = p.FI_InvoiceDate,
                        DueDate = p.FI_DueDate,
                        InvoiceType = p.FI_InvoiceType,
                        BillingPeriod = p.FI_BillingPeriod,
                        TotalAmount = p.FI_TotalAmount,
                        PaidAmount = p.FI_PaidAmount,
                        BalanceAmount = p.FI_BalanceAmount,
                        Status = p.FI_Status,
                        FeeHeadsSummary = p.StudentName, // populated with FeeHeadsSummary in DAL
                        IsSelected = true,
                        PayAmount = p.FI_BalanceAmount
                    }).ToList();

                    vm.AmountTendered = vm.PendingInvoices.Sum(x => x.BalanceAmount);
                }
            }

            return vm;
        }

        public async Task<ServiceResult> ProcessFeeCollectionAsync(FeeCollectSubmissionModel model, Guid tenantId, Guid createdBy)
        {
            if (model.StudentId == Guid.Empty) return ServiceResult.Fail("Student is required.");
            if (model.Amount <= 0) return ServiceResult.Fail("Payment amount must be greater than zero.");
            if (model.PaymentMethodId == Guid.Empty) return ServiceResult.Fail("Payment method is required.");

            var res = await _payDAL.ProcessPaymentAsync(
                tenantId, model.StudentId, model.PaymentDate, model.Amount,
                model.PaymentMethodId, model.TransactionReference,
                model.ChequeNumber, model.BankName, model.ChequeDate,
                model.Notes, createdBy, model.SelectedInvoiceIds);

            if (res.PaymentId == Guid.Empty)
                return ServiceResult.Fail("Failed to process payment.");

            string msg = $"Payment {res.ReceiptNumber} processed successfully! Allocated: ₹{res.TotalAllocated:N2}";
            if (res.AdvanceCredited > 0)
                msg += $" (Advance credited to wallet: ₹{res.AdvanceCredited:N2})";

            return ServiceResult.Ok(msg, res.PaymentId);
        }

        public async Task<FeeReceiptPrintViewModel?> GetReceiptForPrintAsync(Guid paymentId, Guid tenantId)
        {
            return await _payDAL.GetReceiptForPrintAsync(paymentId, tenantId);
        }

        public async Task<ServiceResult> CancelPaymentAsync(Guid paymentId, string reason, Guid tenantId, Guid cancelledBy)
        {
            if (paymentId == Guid.Empty) return ServiceResult.Fail("Payment ID is required.");
            if (string.IsNullOrWhiteSpace(reason)) return ServiceResult.Fail("Cancellation reason is required.");

            var res = await _payDAL.CancelPaymentAsync(tenantId, paymentId, reason, cancelledBy);
            return res.Success ? ServiceResult.Ok(res.Message) : ServiceResult.Fail(res.Message);
        }

        // ==================== Passbook Ledger & Day-End Cash Scroll ====================
        public async Task<StudentFeeLedgerViewModel?> GetStudentFeeLedgerAsync(Guid studentId, Guid tenantId)
        {
            return await _payDAL.GetStudentFeeLedgerAsync(studentId, tenantId);
        }

        public async Task<DailyCollectionReportViewModel> GetDailyCollectionReportAsync(
            Guid tenantId, DateTime fromDate, DateTime toDate, Guid? paymentMethodId, Guid? cashierId)
        {
            var vm = await _payDAL.GetDailyCollectionReportAsync(tenantId, fromDate, toDate, paymentMethodId, cashierId);
            vm.PaymentMethodOptions = GetMasterSelectList("PaymentMethod", paymentMethodId?.ToString());
            return vm;
        }

        // ==================== On-Demand & Scheduled Billing ====================
        public async Task<ServiceResult> CreateAdHocChargeAsync(AdHocChargeFormViewModel model, Guid tenantId, Guid createdBy)
        {
            if (model.StudentId == Guid.Empty) return ServiceResult.Fail("Student is required.");
            if (model.FeeCategoryId == Guid.Empty) return ServiceResult.Fail("Fee Category is required.");
            if (model.Amount <= 0) return ServiceResult.Fail("Charge amount must be greater than zero.");
            if (string.IsNullOrWhiteSpace(model.Description)) return ServiceResult.Fail("Description is required.");

            var (invoiceId, invoiceNumber) = await _fiDAL.CreateAdHocChargeAsync(
                tenantId, model.StudentId, model.FeeCategoryId, model.Amount,
                model.Description, model.DueDate, createdBy);

            if (invoiceId == Guid.Empty) return ServiceResult.Fail("Failed to create on-demand charge.");

            return ServiceResult.Ok($"Charge created successfully. Invoice #{invoiceNumber}", invoiceId);
        }

        public async Task<ServiceResult> GenerateBatchInvoicesAsync(BatchInvoiceGenerateViewModel model, Guid tenantId, Guid createdBy)
        {
            if (model.AcademicYearId == Guid.Empty) return ServiceResult.Fail("Academic Year is required.");
            if (model.CourseId == Guid.Empty) return ServiceResult.Fail("Class / Course is required.");
            if (model.FeeStructureId == Guid.Empty) return ServiceResult.Fail("Fee Structure is required.");
            if (string.IsNullOrWhiteSpace(model.BillingPeriod)) return ServiceResult.Fail("Billing period is required.");

            var (count, total) = await _fiDAL.GenerateBatchInvoicesAsync(
                tenantId, model.AcademicYearId, model.CourseId, model.BatchId,
                model.FeeStructureId, model.BillingPeriod, model.InvoiceDate, model.DueDate, createdBy);

            return ServiceResult.Ok($"Successfully generated {count} student invoices totaling ₹{total:N2} for period {model.BillingPeriod}.");
        }

        public void PopulateBatchInvoiceDropdowns(BatchInvoiceGenerateViewModel vm, Guid tenantId)
        {
            vm.AcademicYearOptions = GetMasterSelectList("AcademicYear", vm.AcademicYearId.ToString());
            vm.CourseOptions = GetMasterSelectList("Course", vm.CourseId.ToString());
            vm.BatchOptions = GetMasterSelectList("Batch", vm.BatchId?.ToString());
            vm.FeeStructureOptions = GetMasterSelectList("FeeCategory", null); // master helper
        }

        // ==================== Helpers ====================
        private List<SelectListItem> GetMasterSelectList(string entityType, string? selectedValue = null)
        {
            var items = _masterService.GetAll(entityType);
            var list = new List<SelectListItem>();
            if (items == null) return list;
            foreach (var item in items)
            {
                var keyEntry = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_Id"));
                var id = keyEntry.Value?.ToString() ?? "";
                string? displayName = null;
                var nameEntry = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_Name"));
                if (nameEntry.Value != null) displayName = nameEntry.Value.ToString();
                if (string.IsNullOrEmpty(displayName))
                {
                    var firstName = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_FirstName")).Value?.ToString() ?? "";
                    var lastName = item.FirstOrDefault(kvp => kvp.Key.EndsWith("_LastName")).Value?.ToString() ?? "";
                    displayName = $"{firstName} {lastName}".Trim();
                }
                if (string.IsNullOrEmpty(displayName)) displayName = item.Values.ElementAtOrDefault(1)?.ToString() ?? id;
                list.Add(new SelectListItem { Value = id, Text = displayName, Selected = id == selectedValue });
            }
            return list;
        }

        private static List<SelectListItem> GetInvoiceStatusSelectList(string? selected = null)
        {
            var statuses = new[] { "issued", "partially_paid", "paid", "overdue", "cancelled" };
            return new List<SelectListItem>(Array.ConvertAll(statuses, s => new SelectListItem { Value = s, Text = s, Selected = s == selected }));
        }

        private static List<SelectListItem> GetPaymentStatusSelectList(string? selected = null)
        {
            var statuses = new[] { "completed", "pending", "failed", "cancelled", "refunded" };
            return new List<SelectListItem>(Array.ConvertAll(statuses, s => new SelectListItem { Value = s, Text = s, Selected = s == selected }));
        }
    }
}
