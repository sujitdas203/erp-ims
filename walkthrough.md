# Walkthrough - Fix SqlException on /Expense

## Overview
Resolved the SqlException `@EC_TenantId is not a parameter for procedure USP_ExpenseCategories_EC` that occurred when accessing `/Expense`.

---

## Root Cause
Stored procedure `USP_ExpenseCategories_EC` originally declared `@TenantId UNIQUEIDENTIFIER = NULL`, whereas `ExpenseCategoryDAL.cs` was passing `@EC_TenantId`. When `GetAllAsync` was invoked during `/Expense` listing, SQL Server threw:
`@EC_TenantId is not a parameter for procedure USP_ExpenseCategories_EC`.

---

## Key Changes Made

1. **Stored Procedure Compatibility (`USP_ExpenseCategories_EC`)**:
   - Updated stored procedure `USP_ExpenseCategories_EC` ([migration_fix_expense_categories_sp.sql](file:///d:/1Common/ExtraPush/IMS/erp-ims/database/migration_fix_expense_categories_sp.sql)) to accept both `@TenantId` and `@EC_TenantId` parameters:
     ```sql
     IF @TenantId IS NULL AND @EC_TenantId IS NOT NULL
         SET @TenantId = @EC_TenantId;
     ```
   - Executed and verified the migration against LocalDB.

2. **DAL Parameter Binding**:
   - Updated [ExpenseCategoryDAL.cs](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.DAL/ExpenseCategoryDAL.cs) to pass `@TenantId` with null checking.

3. **Tenant Context Fallback**:
   - Updated [ExpenseController.cs](file:///d:/1Common/ExtraPush/IMS/erp-ims/IMS.Web/Controllers/ExpenseController.cs) so `CurrentTenantId` falls back to `HardcodedMasterData.CurrentTenantId`.

---

## Verification
- Executed `USP_ExpenseCategories_EC` using both `@TenantId` and `@EC_TenantId` parameters and verified records were returned correctly.
- Rebuilt `IMS.DAL` and `IMS.Services` with 0 compilation errors.
