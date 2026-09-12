
namespace IMS.Models.Common.Dropdown
{

        /// <summary>
        /// Central registry for all generic dropdown configurations.
        /// Register every dropdown once here.
        /// </summary>
    public static class DropdownConfigRegistry
    {
        private static readonly List<DropdownConfig> _configs = new List<DropdownConfig>
    {
        #region Payment Mode

        new DropdownConfig
        {
            EntityType = "PaymentMode",
            TableName = "dbo.PaymentMode_PM",
            KeyColumn = "PM_Id",
            ValueColumn = "PM_Id",
            TextColumn = "PM_ModeName",
            CodeColumn = "PM_ModeCode",
            ActiveColumn = "PM_IsActive",
            OrderByColumn = "PM_ModeName"
        },

        #endregion


        #region Product Category

        new DropdownConfig
        {
            EntityType = "ProductCategory",
            TableName = "dbo.ProductCategory_PC",
            KeyColumn = "PC_Id",
            ValueColumn = "PC_Id",
            TextColumn = "PC_CategoryName",
            CodeColumn = "PC_CategoryCode",
            ActiveColumn = "PC_IsActive",
            OrderByColumn = "PC_CategoryName"
        },

        #endregion


        #region Product Brand

        new DropdownConfig
        {
            EntityType = "ProductBrand",
            TableName = "dbo.ProductBrand_PB",
            KeyColumn = "PB_Id",
            ValueColumn = "PB_Id",
            TextColumn = "PB_BrandName",
            CodeColumn = "PB_BrandCode",
            ActiveColumn = "PB_IsActive",
            OrderByColumn = "PB_BrandName"
        },

        #endregion


        #region Product Unit

        new DropdownConfig
        {
            EntityType = "ProductUnit",
            TableName = "dbo.ProductUnit_PU",
            KeyColumn = "PU_Id",
            ValueColumn = "PU_Id",
            TextColumn = "PU_UnitName",
            CodeColumn = "PU_UnitCode",
            ActiveColumn = "PU_IsActive",
            OrderByColumn = "PU_UnitName"
        },

        #endregion


        #region Vendor Category

        new DropdownConfig
        {
            EntityType = "VendorCategory",
            TableName = "dbo.VendorCategories_VC",
            KeyColumn = "VC_Id",
            ValueColumn = "VC_Id",
            TextColumn = "VC_CategoryName",
            CodeColumn = "VC_CategoryCode",
            ActiveColumn = "VC_IsActive",
            OrderByColumn = "VC_CategoryName"
        },

        #endregion


        #region Branch

        new DropdownConfig
        {
            EntityType = "Branch",
            TableName = "dbo.Branches_B",
            KeyColumn = "B_Id",
            ValueColumn = "B_Id",
            TextColumn = "B_Name",
            CodeColumn = "B_Code",
            ActiveColumn = "B_Status",
            OrderByColumn = "B_Name"
        },

        #endregion


        #region Course

        new DropdownConfig
        {
            EntityType = "Course",
            TableName = "dbo.Courses_C",
            KeyColumn = "C_Id",
            ValueColumn = "C_Id",
            TextColumn = "C_Name",
            CodeColumn = "C_Code",
            ActiveColumn = "C_Status",
            OrderByColumn = "C_Name"
        },

        #endregion


        #region Academic Year

        new DropdownConfig
        {
            EntityType = "AcademicYear",
            TableName = "dbo.AcademicYears_AY",
            KeyColumn = "AY_Id",
            ValueColumn = "AY_Id",
            TextColumn = "AY_Name",
            CodeColumn = "AY_Code",
            ActiveColumn = null,
            OrderByColumn = "AY_Name"
        },

        #endregion


        #region Batch

        new DropdownConfig
        {
            EntityType = "Batch",
            TableName = "dbo.Batches_BT",
            KeyColumn = "BT_Id",
            ValueColumn = "BT_Id",
            TextColumn = "BT_Name",
            CodeColumn = "BT_Code",
            ActiveColumn = "BT_Status",
            OrderByColumn = "BT_Name"
        },

        #endregion


        #region Subject

        new DropdownConfig
        {
            EntityType = "Subject",
            TableName = "dbo.Subjects_SB",
            KeyColumn = "SB_Id",
            ValueColumn = "SB_Id",
            TextColumn = "SB_Name",
            CodeColumn = "SB_Code",
            ActiveColumn = null,
            OrderByColumn = "SB_Name"
        },

        #endregion


        #region Staff

        new DropdownConfig
        {
            EntityType = "Staff",
            TableName = "dbo.Staff_ST",
            KeyColumn = "ST_Id",
            ValueColumn = "ST_Id",
            TextColumn = "ST_FirstName",
            CodeColumn = "ST_EmployeeCode",
            ActiveColumn = "ST_Status",
            OrderByColumn = "ST_FirstName"
        },

        #endregion


        #region Student

        new DropdownConfig
        {
            EntityType = "Student",
            TableName = "dbo.Students_S",
            KeyColumn = "S_Id",
            ValueColumn = "S_Id",
            TextColumn = "S_FirstName",
            CodeColumn = "S_StudentCode",
            ActiveColumn = "S_Status",
            OrderByColumn = "S_FirstName"
        },

        #endregion


        #region Department

        new DropdownConfig
        {
            EntityType = "Department",
            TableName = "dbo.Departments_D",
            KeyColumn = "D_Id",
            ValueColumn = "D_Id",
            TextColumn = "D_Name",
            CodeColumn = "D_Code",
            ActiveColumn = null,
            OrderByColumn = "D_Name"
        },

        #endregion


        #region Designation

        new DropdownConfig
        {
            EntityType = "Designation",
            TableName = "dbo.Designations_DS",
            KeyColumn = "DS_Id",
            ValueColumn = "DS_Id",
            TextColumn = "DS_Name",
            CodeColumn = "DS_Code",
            ActiveColumn = null,
            OrderByColumn = "DS_Name"
        },

        #endregion


        #region Exam Type

        new DropdownConfig
        {
            EntityType = "ExamType",
            TableName = "dbo.ExamTypes_ET",
            KeyColumn = "ET_Id",
            ValueColumn = "ET_Id",
            TextColumn = "ET_Name",
            CodeColumn = "ET_Code",
            ActiveColumn = null,
            OrderByColumn = "ET_Name"
        },

        #endregion


        #region Expense Category

        new DropdownConfig
        {
            EntityType = "ExpenseCategory",
            TableName = "dbo.ExpenseCategories_EC",
            KeyColumn = "EC_Id",
            ValueColumn = "EC_Id",
            TextColumn = "EC_Name",
            CodeColumn = "EC_Code",
            ActiveColumn = null,
            OrderByColumn = "EC_Name"
        },

        #endregion


        #region Fee Category

        new DropdownConfig
        {
            EntityType = "FeeCategory",
            TableName = "dbo.FeeCategories_FC",
            KeyColumn = "FC_Id",
            ValueColumn = "FC_Id",
            TextColumn = "FC_Name",
            CodeColumn = "FC_Code",
            ActiveColumn = null,
            OrderByColumn = "FC_Name"
        },

        #endregion


        #region Payment Method

        new DropdownConfig
        {
            EntityType = "PaymentMethod",
            TableName = "dbo.PaymentMethods_PM",
            KeyColumn = "PM_Id",
            ValueColumn = "PM_Id",
            TextColumn = "PM_Name",
            ActiveColumn = "PM_IsActive",
            OrderByColumn = "PM_Name"
        },

        #endregion


        #region Vendor

        new DropdownConfig
        {
            EntityType = "Vendor",
            TableName = "dbo.Vendors_V",
            KeyColumn = "V_Id",
            ValueColumn = "V_Id",
            TextColumn = "V_Name",
            CodeColumn = "V_Code",
            ActiveColumn = null,
            OrderByColumn = "V_Name"
        },

        #endregion


        #region Classroom

        new DropdownConfig
        {
            EntityType = "Classroom",
            TableName = "dbo.Classrooms_CR",
            KeyColumn = "CR_Id",
            ValueColumn = "CR_Id",
            TextColumn = "CR_Name",
            CodeColumn = "CR_Code",
            ActiveColumn = null,
            OrderByColumn = "CR_Name"
        },

        #endregion


        #region Program

        new DropdownConfig
        {
            EntityType = "Program",
            TableName = "dbo.Programs_P",
            KeyColumn = "P_Id",
            ValueColumn = "P_Id",
            TextColumn = "P_Name",
            CodeColumn = "P_Code",
            ActiveColumn = "P_Status",
            OrderByColumn = "P_Name"
        },

        #endregion


        #region Discount

        new DropdownConfig
        {
            EntityType = "Discount",
            TableName = "dbo.Discounts_DIS",
            KeyColumn = "DIS_Id",
            ValueColumn = "DIS_Id",
            TextColumn = "DIS_Name",
            CodeColumn = "DIS_Code",
            ActiveColumn = "DIS_IsActive",
            OrderByColumn = "DIS_Name"
        },

        #endregion


        #region Grade Scale

        new DropdownConfig
        {
            EntityType = "GradeScale",
            TableName = "dbo.GradeScales_GS",
            KeyColumn = "GS_Id",
            ValueColumn = "GS_Id",
            TextColumn = "GS_Name",
            CodeColumn = "GS_Code",
            ActiveColumn = null,
            OrderByColumn = "GS_Name"
        },

        #endregion


        #region Document Type

        new DropdownConfig
        {
            EntityType = "DocumentType",
            TableName = "dbo.DocumentTypes_DT",
            KeyColumn = "DT_Id",
            ValueColumn = "DT_Id",
            TextColumn = "DT_Name",
            CodeColumn = "DT_Code",
            ActiveColumn = null,
            OrderByColumn = "DT_Name"
        },

        #endregion


        #region Notification Template

        new DropdownConfig
        {
            EntityType = "NotificationTemplate",
            TableName = "dbo.NotificationTemplates_NT",
            KeyColumn = "NT_Id",
            ValueColumn = "NT_Id",
            TextColumn = "NT_Name",
            ActiveColumn = "NT_IsActive",
            OrderByColumn = "NT_Name"
        },

        #endregion


        #region Teacher

        new DropdownConfig
        {
            EntityType = "Teacher",
            TableName = "dbo.Teachers_T",
            KeyColumn = "T_Id",
            ValueColumn = "T_Id",
            TextColumn = "T_EmployeeCode",
            CodeColumn = "T_EmployeeCode",
            ActiveColumn = "T_IsActive",
            OrderByColumn = "T_EmployeeCode"
        },

        #endregion


        #region StudentGuardian

        new DropdownConfig
        {
            EntityType = "StudentGuardian",
            TableName = "dbo.Students_Guardians",
            KeyColumn = "SG_Id",
            ValueColumn = "SG_Id",
            TextColumn = "SG_Relation",
            ActiveColumn = null,
            OrderByColumn = "SG_Relation"
        },

        #endregion


        #region Class

        new DropdownConfig
        {
            EntityType = "Class",
            TableName = "dbo.Classes_CL",
            KeyColumn = "CL_Id",
            ValueColumn = "CL_Id",
            TextColumn = "CL_Name",
            CodeColumn = "CL_Code",
            ActiveColumn = "CL_IsActive",
            ParentColumn = "CL_BranchId",
            OrderByColumn = "CL_Name"
        },

        #endregion


        #region Section

        new DropdownConfig
        {
            EntityType = "Section",
            TableName = "dbo.Sections_S",
            KeyColumn = "S_Sect_Id",
            ValueColumn = "S_Sect_Id",
            TextColumn = "S_Sect_Name",
            CodeColumn = "S_Sect_Code",
            ActiveColumn = "S_Sect_IsActive",
            ParentColumn = "S_Sect_ClassId",
            OrderByColumn = "S_Sect_Name"
        },

        #endregion


        #region Bank

        new DropdownConfig
        {
            EntityType = "Bank",
            TableName = "dbo.BankMaster_BM",
            KeyColumn = "BM_Id",
            ValueColumn = "BM_Id",
            TextColumn = "BM_BankName",
            CodeColumn = "BM_IFSCCode",
            ActiveColumn = "BM_IsActive",
            OrderByColumn = "BM_BankName"
        },

        #endregion
    };

        /// <summary>
        /// Returns configuration by EntityType.
        /// </summary>
        public static DropdownConfig GetByEntityType(string entityType)
        {
            return _configs.FirstOrDefault(x =>
                x.EntityType.Equals(entityType, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns all registered dropdowns.
        /// </summary>
        public static List<DropdownConfig> GetAll()
        {
            return _configs;
        }
    }
}

