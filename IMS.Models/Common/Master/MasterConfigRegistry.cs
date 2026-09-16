using System;
using System.Collections.Generic;
using System.Linq;

namespace IMS.Models.Common.Master
{
    /// <summary>
    /// Single source of truth for all master entity configs.
    /// Arranged in logical School Management hierarchy and cascading dependency order:
    /// 1. Academic Structure (AcademicYear -> Branch -> Department -> Classroom -> Program -> Course -> Subject -> Batch)
    /// 2. Staff & Administration (Designation -> DocumentType)
    /// 3. Fee & Finance (FeeCategory -> Discount -> PaymentMethod -> ExpenseCategory)
    /// 4. Examinations & Grading (ExamType -> GradeScale)
    /// 5. Operations & System (Vendor -> NotificationTemplate)
    /// </summary>
    public static class MasterConfigRegistry
    {
        private static readonly List<MasterConfig> _configs = new List<MasterConfig>
        {
            // =========================================================================
            // GROUP 1: ACADEMIC STRUCTURE & HIERARCHY (CASCADING ORDER)
            // =========================================================================

            // 1. Academic Year Master (Top-level timeline/session)
            new MasterConfig
            {
                EntityType = "AcademicYear",
                SpName = "USP_AcademicYears_AY",
                TableName = "dbo.AcademicYears_AY",
                KeyColumn = "AY_Id",
                DisplayName = "Academic Year Master",
                SoftDelete = false,
                HasAuditColumns = true,
                MenuOrder = 1,
                GroupName = "Academic Structure",
                Icon = "fa-calendar-days",

                ViewPermission = "Master.AcademicYear.View",
                CreatePermission = "Master.AcademicYear.Create",
                EditPermission = "Master.AcademicYear.Edit",
                DeletePermission = "Master.AcademicYear.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig
                    {
                        ColumnName = "AY_Name",
                        PropertyName = "Name",
                        DisplayName = "Academic Year Name",
                        IsRequired = true,
                        IsUnique = true,
                        MaxLength = 100
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "AY_Code",
                        PropertyName = "Code",
                        DisplayName = "Code",
                        IsRequired = true,
                        IsUnique = true,
                        MaxLength = 50
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "AY_StartDate",
                        PropertyName = "StartDate",
                        DisplayName = "Start Date",
                        FieldType = MasterFieldType.Date,
                        IsRequired = true
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "AY_EndDate",
                        PropertyName = "EndDate",
                        DisplayName = "End Date",
                        FieldType = MasterFieldType.Date,
                        IsRequired = true,
                        DateRangeStartField = "AY_StartDate",
                        DateRangeEndField = "AY_EndDate"
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "AY_IsCurrent",
                        PropertyName = "IsCurrent",
                        DisplayName = "Is Current Year",
                        FieldType = MasterFieldType.Boolean
                    }
                }
            },

            // 2. Branch Master (Institute Campus / Location)
            new MasterConfig
            {
                EntityType = "Branch",
                SpName = "USP_Branches_B",
                TableName = "dbo.Branches_B",
                KeyColumn = "B_Id",
                DisplayName = "Branch Master",
                SoftDelete = true,
                HasAuditColumns = true,
                MenuOrder = 2,
                GroupName = "Academic Structure",
                Icon = "fa-building-columns",

                ViewPermission = "Master.Branch.View",
                CreatePermission = "Master.Branch.Create",
                EditPermission = "Master.Branch.Edit",
                DeletePermission = "Master.Branch.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig { ColumnName = "B_Name", PropertyName = "Name", DisplayName = "Branch Name", IsRequired = true, IsUnique = true, MaxLength = 200 },
                    new MasterFieldConfig { ColumnName = "B_Code", PropertyName = "Code", DisplayName = "Branch Code", IsRequired = true, IsUnique = true, MaxLength = 50 },
                    new MasterFieldConfig { ColumnName = "B_Email", PropertyName = "Email", DisplayName = "Email", MaxLength = 255 },
                    new MasterFieldConfig { ColumnName = "B_Phone", PropertyName = "Phone", DisplayName = "Phone", MaxLength = 30 },
                    new MasterFieldConfig { ColumnName = "B_AddressLine1", PropertyName = "AddressLine1", DisplayName = "Address Line 1", MaxLength = 255 },
                    new MasterFieldConfig { ColumnName = "B_AddressLine2", PropertyName = "AddressLine2", DisplayName = "Address Line 2", MaxLength = 255 },
                    new MasterFieldConfig { ColumnName = "B_City", PropertyName = "City", DisplayName = "City", MaxLength = 100 },
                    new MasterFieldConfig { ColumnName = "B_State", PropertyName = "State", DisplayName = "State", MaxLength = 100 },
                    new MasterFieldConfig { ColumnName = "B_PostalCode", PropertyName = "PostalCode", DisplayName = "Postal Code", MaxLength = 20 },
                    new MasterFieldConfig { ColumnName = "B_CountryCode", PropertyName = "CountryCode", DisplayName = "Country Code", MaxLength = 2 }
                }
            },

            // 3. Department Master (Belongs to Branch)
            new MasterConfig
            {
                EntityType = "Department",
                SpName = "USP_Departments_D",
                TableName = "dbo.Departments_D",
                KeyColumn = "D_Id",
                DisplayName = "Department Master",
                SoftDelete = false,
                HasAuditColumns = true,
                MenuOrder = 3,
                GroupName = "Academic Structure",
                Icon = "fa-sitemap",

                ViewPermission = "Master.Department.View",
                CreatePermission = "Master.Department.Create",
                EditPermission = "Master.Department.Edit",
                DeletePermission = "Master.Department.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig { ColumnName = "D_Name", PropertyName = "Name", DisplayName = "Department Name", IsRequired = true, IsUnique = true, MaxLength = 150 },
                    new MasterFieldConfig { ColumnName = "D_Code", PropertyName = "Code", DisplayName = "Department Code", IsRequired = true, IsUnique = true, MaxLength = 50 },
                    new MasterFieldConfig { ColumnName = "D_BranchId", GridColumnName = "B_Name", PropertyName = "BranchId", DisplayName = "Branch", FieldType = MasterFieldType.Dropdown, LookupEntityType = "Branch", LookupValueField = "B_Id", LookupTextField = "B_Name" },
                    new MasterFieldConfig { ColumnName = "D_Description", PropertyName = "Description", DisplayName = "Description", FieldType = MasterFieldType.TextArea }
                }
            },

            // 4. Classroom Master (Rooms / Labs belonging to Branch)
            new MasterConfig
            {
                EntityType = "Classroom",
                SpName = "USP_Classrooms_CR",
                TableName = "dbo.Classrooms_CR",
                KeyColumn = "CR_Id",
                DisplayName = "Classroom Master",
                SoftDelete = false,
                HasAuditColumns = true,
                MenuOrder = 4,
                GroupName = "Academic Structure",
                Icon = "fa-chalkboard-user",

                ViewPermission = "Master.Classroom.View",
                CreatePermission = "Master.Classroom.Create",
                EditPermission = "Master.Classroom.Edit",
                DeletePermission = "Master.Classroom.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig
                    {
                        ColumnName = "CR_Name",
                        PropertyName = "Name",
                        DisplayName = "Room Name",
                        IsRequired = true,
                        MaxLength = 100
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "CR_Code",
                        PropertyName = "Code",
                        DisplayName = "Room Code",
                        IsRequired = true,
                        IsUnique = true,
                        MaxLength = 50
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "CR_BranchId",
                        GridColumnName = "B_Name",
                        PropertyName = "BranchId",
                        DisplayName = "Branch",
                        FieldType = MasterFieldType.Dropdown,
                        LookupEntityType = "Branch",
                        LookupValueField = "B_Id",
                        LookupTextField = "B_Name"
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "CR_Capacity",
                        PropertyName = "Capacity",
                        DisplayName = "Seating Capacity",
                        FieldType = MasterFieldType.Number,
                        IsRequired = true
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "CR_Location",
                        PropertyName = "Location",
                        DisplayName = "Location",
                        MaxLength = 255
                    }
                }
            },

            // 5. Program Master (Degrees / Streams / Standards)
            new MasterConfig
            {
                EntityType = "Program",
                SpName = "USP_Programs_P",
                TableName = "dbo.Programs_P",
                KeyColumn = "P_Id",
                DisplayName = "Program Master",
                SoftDelete = true,
                HasAuditColumns = true,
                MenuOrder = 5,
                GroupName = "Academic Structure",
                Icon = "fa-graduation-cap",

                ViewPermission = "Master.Program.View",
                CreatePermission = "Master.Program.Create",
                EditPermission = "Master.Program.Edit",
                DeletePermission = "Master.Program.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig { ColumnName = "P_Name", PropertyName = "Name", DisplayName = "Program Name", IsRequired = true, IsUnique = true, MaxLength = 200 },
                    new MasterFieldConfig { ColumnName = "P_Code", PropertyName = "Code", DisplayName = "Program Code", IsRequired = true, IsUnique = true, MaxLength = 50 },
                    new MasterFieldConfig { ColumnName = "P_DurationValue", PropertyName = "DurationValue", DisplayName = "Duration Value", FieldType = MasterFieldType.Number },
                    new MasterFieldConfig { ColumnName = "P_DurationUnit", PropertyName = "DurationUnit", DisplayName = "Duration Unit (e.g. Years, Months)", MaxLength = 20 },
                    new MasterFieldConfig { ColumnName = "P_Description", PropertyName = "Description", DisplayName = "Description", FieldType = MasterFieldType.TextArea }
                }
            },

            // 6. Course Master (Belongs to Program)
            new MasterConfig
            {
                EntityType = "Course",
                SpName = "USP_Courses_C",
                TableName = "dbo.Courses_C",
                KeyColumn = "C_Id",
                DisplayName = "Course Master",
                SoftDelete = true,
                HasAuditColumns = true,
                MenuOrder = 6,
                GroupName = "Academic Structure",
                Icon = "fa-book-open",

                ViewPermission = "Master.Course.View",
                CreatePermission = "Master.Course.Create",
                EditPermission = "Master.Course.Edit",
                DeletePermission = "Master.Course.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig { ColumnName = "C_Name", PropertyName = "Name", DisplayName = "Course Name", IsRequired = true, IsUnique = true, MaxLength = 200 },
                    new MasterFieldConfig { ColumnName = "C_Code", PropertyName = "Code", DisplayName = "Course Code", IsRequired = true, IsUnique = true, MaxLength = 50 },
                    new MasterFieldConfig { ColumnName = "C_ProgramId", GridColumnName = "P_Name", PropertyName = "ProgramId", DisplayName = "Program", FieldType = MasterFieldType.Dropdown, LookupEntityType = "Program", LookupValueField = "P_Id", LookupTextField = "P_Name" },
                    new MasterFieldConfig { ColumnName = "C_Description", PropertyName = "Description", DisplayName = "Description", FieldType = MasterFieldType.TextArea }
                }
            },

            // 7. Subject Master (Curriculum subjects)
            new MasterConfig
            {
                EntityType = "Subject",
                SpName = "USP_Subjects_SB",
                TableName = "dbo.Subjects_SB",
                KeyColumn = "SB_Id",
                DisplayName = "Subject Master",
                SoftDelete = false,
                HasAuditColumns = true,
                MenuOrder = 7,
                GroupName = "Academic Structure",
                Icon = "fa-book-bookmark",

                ViewPermission = "Master.Subject.View",
                CreatePermission = "Master.Subject.Create",
                EditPermission = "Master.Subject.Edit",
                DeletePermission = "Master.Subject.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig { ColumnName = "SB_Name", PropertyName = "Name", DisplayName = "Subject Name", IsRequired = true, IsUnique = true, MaxLength = 200 },
                    new MasterFieldConfig { ColumnName = "SB_Code", PropertyName = "Code", DisplayName = "Subject Code", IsRequired = true, IsUnique = true, MaxLength = 50 },
                    new MasterFieldConfig { ColumnName = "SB_Credits", PropertyName = "Credits", DisplayName = "Credits", FieldType = MasterFieldType.Number },
                    new MasterFieldConfig { ColumnName = "SB_MaxMarks", PropertyName = "MaxMarks", DisplayName = "Maximum Marks", FieldType = MasterFieldType.Number },
                    new MasterFieldConfig { ColumnName = "SB_PassMarks", PropertyName = "PassMarks", DisplayName = "Pass Marks", FieldType = MasterFieldType.Number },
                    new MasterFieldConfig { ColumnName = "SB_Description", PropertyName = "Description", DisplayName = "Description", FieldType = MasterFieldType.TextArea }
                }
            },

            // 8. Batch Master (Cohorts linking Academic Year + Branch + Course)
            new MasterConfig
            {
                EntityType = "Batch",
                SpName = "USP_Batches_BT",
                TableName = "dbo.Batches_BT",
                KeyColumn = "BT_Id",
                DisplayName = "Batch Master",
                SoftDelete = false,
                HasAuditColumns = true,
                MenuOrder = 8,
                GroupName = "Academic Structure",
                Icon = "fa-users-rectangle",

                ViewPermission = "Batch.View",
                CreatePermission = "Batch.Create",
                EditPermission = "Batch.Edit",
                DeletePermission = "Batch.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig { ColumnName = "BT_Name", PropertyName = "Name", DisplayName = "Batch Name", IsRequired = true, IsUnique = true, MaxLength = 150 },
                    new MasterFieldConfig { ColumnName = "BT_Code", PropertyName = "Code", DisplayName = "Batch Code", IsRequired = true, IsUnique = true, MaxLength = 50 },
                    new MasterFieldConfig { ColumnName = "BT_BranchId", GridColumnName = "BranchName", PropertyName = "BranchId", DisplayName = "Branch", FieldType = MasterFieldType.Dropdown, LookupEntityType = "Branch", LookupValueField = "B_Id", LookupTextField = "B_Name", IsRequired = true },
                    new MasterFieldConfig { ColumnName = "BT_CourseId", GridColumnName = "CourseName", PropertyName = "CourseId", DisplayName = "Course", FieldType = MasterFieldType.Dropdown, LookupEntityType = "Course", LookupValueField = "C_Id", LookupTextField = "C_Name", IsRequired = true },
                    new MasterFieldConfig { ColumnName = "BT_AcademicYearId", GridColumnName = "AcademicYearName", PropertyName = "AcademicYearId", DisplayName = "Academic Year", FieldType = MasterFieldType.Dropdown, LookupEntityType = "AcademicYear", LookupValueField = "AY_Id", LookupTextField = "AY_Name", IsRequired = true },
                    new MasterFieldConfig { ColumnName = "BT_StartDate", PropertyName = "StartDate", DisplayName = "Start Date", FieldType = MasterFieldType.Date, IsRequired = true },
                    new MasterFieldConfig { ColumnName = "BT_EndDate", PropertyName = "EndDate", DisplayName = "End Date", FieldType = MasterFieldType.Date },
                    new MasterFieldConfig { ColumnName = "BT_Capacity", PropertyName = "Capacity", DisplayName = "Capacity", FieldType = MasterFieldType.Number }
                }
            },

            // =========================================================================
            // GROUP 2: STAFF & INSTITUTIONAL ADMINISTRATION
            // =========================================================================

            // 9. Designation Master
            new MasterConfig
            {
                EntityType = "Designation",
                SpName = "USP_Designations_DS",
                TableName = "dbo.Designations_DS",
                KeyColumn = "DS_Id",
                DisplayName = "Designation Master",
                SoftDelete = false,
                HasAuditColumns = true,
                MenuOrder = 9,
                GroupName = "Staff & Administration",
                Icon = "fa-id-badge",

                ViewPermission = "Master.Designation.View",
                CreatePermission = "Master.Designation.Create",
                EditPermission = "Master.Designation.Edit",
                DeletePermission = "Master.Designation.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig { ColumnName = "DS_Name", PropertyName = "Name", DisplayName = "Designation Name", IsRequired = true, IsUnique = true, MaxLength = 100 },
                    new MasterFieldConfig { ColumnName = "DS_Code", PropertyName = "Code", DisplayName = "Designation Code", IsRequired = true, IsUnique = true, MaxLength = 50 }
                }
            },

            // 10. Document Type Master
            new MasterConfig
            {
                EntityType = "DocumentType",
                SpName = "USP_DocumentTypes_DT",
                TableName = "dbo.DocumentTypes_DT",
                KeyColumn = "DT_Id",
                DisplayName = "Document Type Master",
                SoftDelete = false,
                HasAuditColumns = true,
                MenuOrder = 10,
                GroupName = "Staff & Administration",
                Icon = "fa-file-circle-check",

                ViewPermission = "Master.DocumentType.View",
                CreatePermission = "Master.DocumentType.Create",
                EditPermission = "Master.DocumentType.Edit",
                DeletePermission = "Master.DocumentType.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig
                    {
                        ColumnName = "DT_Name",
                        PropertyName = "Name",
                        DisplayName = "Document Type Name",
                        IsRequired = true,
                        IsUnique = true,
                        MaxLength = 100
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "DT_Code",
                        PropertyName = "Code",
                        DisplayName = "Code",
                        IsRequired = true,
                        IsUnique = true,
                        MaxLength = 50
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "DT_EntityType",
                        PropertyName = "EntityType",
                        DisplayName = "Entity Type",
                        FieldType = MasterFieldType.Dropdown,
                        IsRequired = true,
                        MaxLength = 30,
                        DropdownOptions = new Dictionary<string, string>
                        {
                            { "Student", "Student" },
                            { "Staff", "Staff" },
                            { "Teacher", "Teacher" },
                            { "Admission", "Admission Application" },
                            { "General", "General" }
                        }
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "DT_IsRequired",
                        PropertyName = "IsRequired",
                        DisplayName = "Mandatory Upload",
                        FieldType = MasterFieldType.Boolean
                    }
                }
            },

            // =========================================================================
            // GROUP 3: FINANCE, FEE & CONCESSIONS
            // =========================================================================

            // 11. Fee Category Master
            new MasterConfig
            {
                EntityType = "FeeCategory",
                SpName = "USP_FeeCategories_FC",
                TableName = "dbo.FeeCategories_FC",
                KeyColumn = "FC_Id",
                DisplayName = "Fee Category Master",
                SoftDelete = false,
                HasAuditColumns = true,
                MenuOrder = 11,
                GroupName = "Fee & Finance",
                Icon = "fa-file-invoice-dollar",

                ViewPermission = "Master.FeeCategory.View",
                CreatePermission = "Master.FeeCategory.Create",
                EditPermission = "Master.FeeCategory.Edit",
                DeletePermission = "Master.FeeCategory.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig { ColumnName = "FC_Name", PropertyName = "Name", DisplayName = "Category Name", IsRequired = true, IsUnique = true, MaxLength = 100 },
                    new MasterFieldConfig { ColumnName = "FC_Code", PropertyName = "Code", DisplayName = "Category Code", IsRequired = true, IsUnique = true, MaxLength = 50 },
                    new MasterFieldConfig { ColumnName = "FC_Description", PropertyName = "Description", DisplayName = "Description", FieldType = MasterFieldType.TextArea },
                    new MasterFieldConfig { ColumnName = "FC_IsRefundable", PropertyName = "IsRefundable", DisplayName = "Refundable", FieldType = MasterFieldType.Boolean }
                }
            },

            // 12. Discount Master
            new MasterConfig
            {
                EntityType = "Discount",
                SpName = "USP_Discounts_DIS",
                TableName = "dbo.Discounts_DIS",
                KeyColumn = "DIS_Id",
                DisplayName = "Discount Master",
                SoftDelete = false,
                HasAuditColumns = true,
                MenuOrder = 12,
                GroupName = "Fee & Finance",
                Icon = "fa-percent",

                ViewPermission = "Master.Discount.View",
                CreatePermission = "Master.Discount.Create",
                EditPermission = "Master.Discount.Edit",
                DeletePermission = "Master.Discount.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig
                    {
                        ColumnName = "DIS_Name",
                        PropertyName = "Name",
                        DisplayName = "Discount Policy Name",
                        IsRequired = true,
                        IsUnique = true,
                        MaxLength = 100
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "DIS_Code",
                        PropertyName = "Code",
                        DisplayName = "Code",
                        IsRequired = true,
                        IsUnique = true,
                        MaxLength = 50
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "DIS_DiscountType",
                        PropertyName = "DiscountType",
                        DisplayName = "Type",
                        FieldType = MasterFieldType.Dropdown,
                        IsRequired = true,
                        MaxLength = 20,
                        DropdownOptions = new Dictionary<string, string>
                        {
                            { "percentage", "Percentage" },
                            { "fixed", "Fixed" }
                        }
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "DIS_Value",
                        PropertyName = "Value",
                        DisplayName = "Discount Value",
                        FieldType = MasterFieldType.Number,
                        IsRequired = true
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "DIS_Description",
                        PropertyName = "Description",
                        DisplayName = "Description",
                        FieldType = MasterFieldType.TextArea
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "DIS_IsActive",
                        PropertyName = "IsActive",
                        DisplayName = "Is Active",
                        FieldType = MasterFieldType.Boolean,
                        ShowInGrid = false
                    }
                }
            },

            // 13. Payment Method Master
            new MasterConfig
            {
                EntityType = "PaymentMethod",
                SpName = "USP_PaymentMethods_PM",
                TableName = "dbo.PaymentMethods_PM",
                KeyColumn = "PM_Id",
                DisplayName = "Payment Method Master",
                SoftDelete = true,
                HasAuditColumns = true,
                MenuOrder = 13,
                GroupName = "Fee & Finance",
                Icon = "fa-credit-card",

                ViewPermission = "Master.PaymentMethod.View",
                CreatePermission = "Master.PaymentMethod.Create",
                EditPermission = "Master.PaymentMethod.Edit",
                DeletePermission = "Master.PaymentMethod.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig
                    {
                        ColumnName = "PM_Name",
                        PropertyName = "Name",
                        DisplayName = "Method Name",
                        IsRequired = true,
                        IsUnique = true,
                        MaxLength = 100
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "PM_Type",
                        PropertyName = "Type",
                        DisplayName = "Method Type",
                        FieldType = MasterFieldType.Dropdown,
                        IsRequired = true,
                        MaxLength = 30,
                        DropdownOptions = new Dictionary<string, string>
                        {
                            { "Cash", "Cash" },
                            { "Card", "Card" },
                            { "Bank Transfer", "Bank Transfer" },
                            { "Cheque", "Cheque" },
                            { "Online", "Online / UPI" },
                            { "Other", "Other" }
                        }
                    }
                }
            },

            // 14. Expense Category Master
            new MasterConfig
            {
                EntityType = "ExpenseCategory",
                SpName = "USP_ExpenseCategories_EC",
                TableName = "dbo.ExpenseCategories_EC",
                KeyColumn = "EC_Id",
                DisplayName = "Expense Category Master",
                SoftDelete = false,
                HasAuditColumns = true,
                MenuOrder = 14,
                GroupName = "Fee & Finance",
                Icon = "fa-receipt",

                ViewPermission = "Master.ExpenseCategory.View",
                CreatePermission = "Master.ExpenseCategory.Create",
                EditPermission = "Master.ExpenseCategory.Edit",
                DeletePermission = "Master.ExpenseCategory.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig { ColumnName = "EC_Name", PropertyName = "Name", DisplayName = "Category Name", IsRequired = true, IsUnique = true, MaxLength = 100 },
                    new MasterFieldConfig { ColumnName = "EC_Code", PropertyName = "Code", DisplayName = "Category Code", IsRequired = true, IsUnique = true, MaxLength = 50 },
                    new MasterFieldConfig { ColumnName = "EC_Description", PropertyName = "Description", DisplayName = "Description", FieldType = MasterFieldType.TextArea }
                }
            },

            // =========================================================================
            // GROUP 4: EXAMINATIONS & GRADING
            // =========================================================================

            // 15. Exam Type Master
            new MasterConfig
            {
                EntityType = "ExamType",
                SpName = "USP_ExamTypes_ET",
                TableName = "dbo.ExamTypes_ET",
                KeyColumn = "ET_Id",
                DisplayName = "Exam Type Master",
                SoftDelete = false,
                HasAuditColumns = true,
                MenuOrder = 15,
                GroupName = "Examinations & Grading",
                Icon = "fa-pen-to-square",

                ViewPermission = "Master.ExamType.View",
                CreatePermission = "Master.ExamType.Create",
                EditPermission = "Master.ExamType.Edit",
                DeletePermission = "Master.ExamType.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig
                    {
                        ColumnName = "ET_Name",
                        PropertyName = "Name",
                        DisplayName = "Exam Type Name",
                        IsRequired = true,
                        IsUnique = true,
                        MaxLength = 100
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "ET_Code",
                        PropertyName = "Code",
                        DisplayName = "Exam Code",
                        IsRequired = true,
                        IsUnique = true,
                        MaxLength = 50
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "ET_WeightagePercentage",
                        PropertyName = "WeightagePercentage",
                        DisplayName = "Weightage %",
                        FieldType = MasterFieldType.Number,
                        IsRequired = false
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "ET_Description",
                        PropertyName = "Description",
                        DisplayName = "Description",
                        FieldType = MasterFieldType.TextArea
                    }
                }
            },

            // 16. Grade Scale Master
            new MasterConfig
            {
                EntityType = "GradeScale",
                SpName = "USP_GradeScales_GS",
                TableName = "dbo.GradeScales_GS",
                KeyColumn = "GS_Id",
                DisplayName = "Grade Scale Master",
                SoftDelete = true,
                HasAuditColumns = true,
                MenuOrder = 16,
                GroupName = "Examinations & Grading",
                Icon = "fa-award",

                ViewPermission = "Master.GradeScale.View",
                CreatePermission = "Master.GradeScale.Create",
                EditPermission = "Master.GradeScale.Edit",
                DeletePermission = "Master.GradeScale.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig { ColumnName = "GS_Name", PropertyName = "Name", DisplayName = "Grade Scale Name", IsRequired = true, IsUnique = true, MaxLength = 100 },
                    new MasterFieldConfig { ColumnName = "GS_Code", PropertyName = "Code", DisplayName = "Grade Code", IsRequired = true, IsUnique = true, MaxLength = 50 },
                    new MasterFieldConfig { ColumnName = "GS_Description", PropertyName = "Description", DisplayName = "Description", FieldType = MasterFieldType.TextArea },
                    new MasterFieldConfig { ColumnName = "GS_IsDefault", PropertyName = "IsDefault", DisplayName = "Default Scale", FieldType = MasterFieldType.Boolean }
                }
            },

            // =========================================================================
            // GROUP 5: OPERATIONS & SYSTEM
            // =========================================================================

            // 17. Vendor Master
            new MasterConfig
            {
                EntityType = "Vendor",
                SpName = "USP_Vendors_V",
                TableName = "dbo.Vendors_V",
                KeyColumn = "V_Id",
                DisplayName = "Vendor Master",
                SoftDelete = false,
                HasAuditColumns = true,
                MenuOrder = 17,
                GroupName = "Operations & System",
                Icon = "fa-truck-field",

                ViewPermission = "Master.Vendor.View",
                CreatePermission = "Master.Vendor.Create",
                EditPermission = "Master.Vendor.Edit",
                DeletePermission = "Master.Vendor.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig { ColumnName = "V_Name", PropertyName = "Name", DisplayName = "Vendor Name", IsRequired = true, IsUnique = true, MaxLength = 200 },
                    new MasterFieldConfig { ColumnName = "V_Code", PropertyName = "Code", DisplayName = "Vendor Code", IsRequired = true, IsUnique = true, MaxLength = 50 },
                    new MasterFieldConfig { ColumnName = "V_Email", PropertyName = "Email", DisplayName = "Email", MaxLength = 255 },
                    new MasterFieldConfig { ColumnName = "V_Phone", PropertyName = "Phone", DisplayName = "Phone", MaxLength = 30 },
                    new MasterFieldConfig { ColumnName = "V_TaxNumber", PropertyName = "TaxNumber", DisplayName = "GSTIN / Tax Number", MaxLength = 100 },
                    new MasterFieldConfig { ColumnName = "V_Address", PropertyName = "Address", DisplayName = "Address", FieldType = MasterFieldType.TextArea }
                }
            },

            // 18. Notification Template Master
            new MasterConfig
            {
                EntityType = "NotificationTemplate",
                SpName = "USP_NotificationTemplates_NT",
                TableName = "dbo.NotificationTemplates_NT",
                KeyColumn = "NT_Id",
                DisplayName = "Notification Template Master",
                SoftDelete = true,
                HasAuditColumns = true,
                MenuOrder = 18,
                GroupName = "Operations & System",
                Icon = "fa-bell",

                ViewPermission = "Master.NotificationTemplate.View",
                CreatePermission = "Master.NotificationTemplate.Create",
                EditPermission = "Master.NotificationTemplate.Edit",
                DeletePermission = "Master.NotificationTemplate.Delete",

                Fields = new List<MasterFieldConfig>
                {
                    new MasterFieldConfig
                    {
                        ColumnName = "NT_Name",
                        PropertyName = "Name",
                        DisplayName = "Template Name",
                        IsRequired = true,
                        IsUnique = true,
                        MaxLength = 150
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "NT_EventKey",
                        PropertyName = "EventKey",
                        DisplayName = "Event Key",
                        IsRequired = true,
                        IsUnique = true,
                        MaxLength = 100
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "NT_Channel",
                        PropertyName = "Channel",
                        DisplayName = "Channel",
                        FieldType = MasterFieldType.Dropdown,
                        IsRequired = true,
                        MaxLength = 20,
                        DropdownOptions = new Dictionary<string, string>
                        {
                            { "Email", "Email" },
                            { "SMS", "SMS" },
                            { "WhatsApp", "WhatsApp" },
                            { "InApp", "In-App Notification" }
                        }
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "NT_Subject",
                        PropertyName = "Subject",
                        DisplayName = "Email Subject Line",
                        MaxLength = 255
                    },
                    new MasterFieldConfig
                    {
                        ColumnName = "NT_BodyTemplate",
                        PropertyName = "BodyTemplate",
                        DisplayName = "Template Content",
                        FieldType = MasterFieldType.TextArea,
                        IsRequired = true
                    }
                }
            }
        };

        public static List<MasterConfig> GetAll() => _configs.OrderBy(c => c.MenuOrder).ToList();

        public static MasterConfig GetByEntityType(string entityType)
        {
            return _configs.FirstOrDefault(c => c.EntityType.Equals(entityType, StringComparison.OrdinalIgnoreCase));
        }
    }
}
