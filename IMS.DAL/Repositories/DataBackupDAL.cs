using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using IMS.DAL.Common;
using IMS.DAL.Interfaces;
using IMS.Models.Backup;

namespace IMS.DAL.Repositories
{
    public class DataBackupDAL : IDataBackupDAL
    {
        private readonly DBHelper _dbHelper;

        public DataBackupDAL(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<DataBackupModuleInfo> GetAvailableModules(Guid tenantId)
        {
            var counts = GetSystemSummaryCounts(tenantId);

            return new List<DataBackupModuleInfo>
            {
                new DataBackupModuleInfo
                {
                    Code = "Students",
                    DisplayName = "Students Directory",
                    Description = "Full student profiles, admission numbers, parents & contact data",
                    Icon = "fa-user-graduate",
                    Category = "Students & Admissions",
                    EstimatedCount = counts.GetValueOrDefault("Students", 0),
                    SupportedFilters = new() { "AcademicYear", "Branch", "Course", "Batch", "Status", "DateRange" }
                },
                new DataBackupModuleInfo
                {
                    Code = "AdmissionApplications",
                    DisplayName = "Admission Applications",
                    Description = "Inquiries, submitted & approved student applications",
                    Icon = "fa-address-card",
                    Category = "Students & Admissions",
                    EstimatedCount = counts.GetValueOrDefault("AdmissionApplications", 0),
                    SupportedFilters = new() { "AcademicYear", "Branch", "Course", "Status", "DateRange" }
                },
                new DataBackupModuleInfo
                {
                    Code = "Enrollments",
                    DisplayName = "Student Enrollments",
                    Description = "Course & batch enrollment mapping records",
                    Icon = "fa-user-plus",
                    Category = "Students & Admissions",
                    EstimatedCount = counts.GetValueOrDefault("Enrollments", 0),
                    SupportedFilters = new() { "AcademicYear", "Branch", "Course", "Batch", "Status", "DateRange" }
                },
                new DataBackupModuleInfo
                {
                    Code = "Teachers",
                    DisplayName = "Teachers & Faculty",
                    Description = "Teacher profiles, designations, department & contact details",
                    Icon = "fa-chalkboard-user",
                    Category = "Academic Delivery",
                    EstimatedCount = counts.GetValueOrDefault("Teachers", 0),
                    SupportedFilters = new() { "Branch", "Status" }
                },
                new DataBackupModuleInfo
                {
                    Code = "Staff",
                    DisplayName = "Staff & Administration",
                    Description = "Staff member records, roles, and branch assignments",
                    Icon = "fa-users-gear",
                    Category = "Academic Delivery",
                    EstimatedCount = counts.GetValueOrDefault("Staff", 0),
                    SupportedFilters = new() { "Branch", "Status", "DateRange" }
                },
                new DataBackupModuleInfo
                {
                    Code = "FeeInvoices",
                    DisplayName = "Fee Invoices & Payments",
                    Description = "Student invoices, dues, payments & transaction breakdown",
                    Icon = "fa-file-invoice-dollar",
                    Category = "Fee & Finance",
                    EstimatedCount = counts.GetValueOrDefault("FeeInvoices", 0),
                    SupportedFilters = new() { "AcademicYear", "Branch", "Course", "Batch", "Status", "DateRange" }
                },
                new DataBackupModuleInfo
                {
                    Code = "FeeStructures",
                    DisplayName = "Fee Structures",
                    Description = "Configured fee structures, breakdown & course allocations",
                    Icon = "fa-money-check-dollar",
                    Category = "Fee & Finance",
                    EstimatedCount = counts.GetValueOrDefault("FeeStructures", 0),
                    SupportedFilters = new() { "AcademicYear", "Course", "Batch", "DateRange" }
                },
                new DataBackupModuleInfo
                {
                    Code = "Attendance",
                    DisplayName = "Student Attendance",
                    Description = "Daily and session-wise student attendance records",
                    Icon = "fa-calendar-check",
                    Category = "Academic Delivery",
                    EstimatedCount = counts.GetValueOrDefault("Attendance", 0),
                    SupportedFilters = new() { "AcademicYear", "Branch", "Course", "Batch", "Status", "DateRange" }
                },
                new DataBackupModuleInfo
                {
                    Code = "ExamsAndMarks",
                    DisplayName = "Exams & Student Marks",
                    Description = "Examination schedules, subject marks & scores",
                    Icon = "fa-square-poll-vertical",
                    Category = "Academic Delivery",
                    EstimatedCount = counts.GetValueOrDefault("ExamsAndMarks", 0),
                    SupportedFilters = new() { "AcademicYear", "Course", "Batch" }
                },
                new DataBackupModuleInfo
                {
                    Code = "Timetable",
                    DisplayName = "Timetable Schedules",
                    Description = "Class schedules, subject teachers, and room allocations",
                    Icon = "fa-calendar-days",
                    Category = "Academic Delivery",
                    EstimatedCount = counts.GetValueOrDefault("Timetable", 0),
                    SupportedFilters = new() { "Branch", "Course", "Batch" }
                },
                new DataBackupModuleInfo
                {
                    Code = "Expenses",
                    DisplayName = "Expenses & Payments",
                    Description = "Institutional expenses, categories, vendors & payment modes",
                    Icon = "fa-receipt",
                    Category = "Fee & Finance",
                    EstimatedCount = counts.GetValueOrDefault("Expenses", 0),
                    SupportedFilters = new() { "Branch", "DateRange" }
                },
                new DataBackupModuleInfo
                {
                    Code = "MasterData",
                    DisplayName = "Master & Lookup Configurations",
                    Description = "Academic years, branches, courses, programs, subjects & classrooms",
                    Icon = "fa-database",
                    Category = "System & Master",
                    EstimatedCount = counts.GetValueOrDefault("MasterData", 0),
                    SupportedFilters = new() { "Branch" }
                }
            };
        }

        public Dictionary<string, int> GetSystemSummaryCounts(Guid tenantId)
        {
            var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            try
            {
                var sql = @"
                    SELECT 
                        (SELECT COUNT(1) FROM Students_S WHERE S_TenantId = @TenantId AND S_DeletedAt IS NULL) AS Students,
                        (SELECT COUNT(1) FROM AdmissionApplications_AA WHERE AA_TenantId = @TenantId) AS AdmissionApplications,
                        (SELECT COUNT(1) FROM Enrollments_E WHERE E_TenantId = @TenantId) AS Enrollments,
                        (SELECT COUNT(1) FROM Teachers_T WHERE T_TenantId = @TenantId AND T_IsActive = 1) AS Teachers,
                        (SELECT COUNT(1) FROM Staff_ST WHERE ST_TenantId = @TenantId AND ST_DeletedAt IS NULL) AS Staff,
                        (SELECT COUNT(1) FROM FeeInvoices_FI WHERE FI_TenantId = @TenantId) AS FeeInvoices,
                        (SELECT COUNT(1) FROM FeeStructures_FS WHERE FS_TenantId = @TenantId) AS FeeStructures,
                        (SELECT COUNT(1) FROM AttendanceRecords_AR ar INNER JOIN AttendanceSessions_AS asess ON ar.AR_AttendanceSessionId = asess.AS_Id WHERE asess.AS_TenantId = @TenantId) AS Attendance,
                        (SELECT COUNT(1) FROM Marks_M m INNER JOIN ExamSubjects_ES es ON m.M_ExamSubjectId = es.ES_Id INNER JOIN Exams_EX ex ON es.ES_ExamId = ex.EX_Id WHERE ex.EX_TenantId = @TenantId) AS ExamsAndMarks,
                        (SELECT COUNT(1) FROM Timetables_TT WHERE TT_TenantId = @TenantId) AS Timetable,
                        (SELECT COUNT(1) FROM Expenses_EXP WHERE EXP_TenantId = @TenantId) AS Expenses,
                        (SELECT (SELECT COUNT(1) FROM Courses_C WHERE C_TenantId = @TenantId AND C_DeletedAt IS NULL) + 
                                (SELECT COUNT(1) FROM Branches_B WHERE B_TenantId = @TenantId AND B_DeletedAt IS NULL) + 
                                (SELECT COUNT(1) FROM Subjects_SB WHERE SB_TenantId = @TenantId)) AS MasterData;
                ";

                var dt = _dbHelper.ExecuteDataTable(sql, new Dictionary<string, object> { { "@TenantId", tenantId } });
                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    foreach (DataColumn col in dt.Columns)
                    {
                        dict[col.ColumnName] = row[col] != DBNull.Value ? Convert.ToInt32(row[col]) : 0;
                    }
                }
            }
            catch
            {
                // Fallback gracefully if any table is not yet created in specific db snapshot
            }

            return dict;
        }

        public int GetModuleCount(string module, DataBackupFilterViewModel filter, Guid tenantId)
        {
            var dt = GetModuleData(module, filter, tenantId);
            return dt?.Rows.Count ?? 0;
        }

        public DataTable GetModuleData(string module, DataBackupFilterViewModel filter, Guid tenantId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@TenantId", tenantId }
            };

            var sb = new StringBuilder();

            switch (module?.Trim())
            {
                case "Students":
                    sb.Append(@"
                        SELECT s.S_StudentCode AS [Student Code],
                               LTRIM(RTRIM(CONCAT(s.S_FirstName, ' ', ISNULL(s.S_MiddleName + ' ', ''), ISNULL(s.S_LastName, '')))) AS [Student Name],
                               s.S_AdmissionNumber AS [Admission Number],
                               COALESCE(e.E_RollNumber, '-') AS [Roll Number],
                               s.S_Gender AS [Gender],
                               CONVERT(VARCHAR(10), s.S_DateOfBirth, 120) AS [Date Of Birth],
                               s.S_Email AS [Email],
                               s.S_Phone AS [Phone],
                               b.B_Name AS [Branch],
                               p.P_Name AS [Program],
                               c.C_Name AS [Course],
                               bt.BT_Name AS [Batch],
                               ay.AY_Name AS [Academic Year],
                               s.S_Status AS [Status],
                               CONVERT(VARCHAR(10), s.S_AdmissionDate, 120) AS [Admission Date],
                               s.S_BloodGroup AS [Blood Group],
                               s.S_AddressLine1 AS [Address],
                               s.S_City AS [City],
                               s.S_State AS [State],
                               s.S_PostalCode AS [Postal Code],
                               s.S_Country AS [Country],
                               LTRIM(RTRIM(CONCAT(g.G_FirstName, ' ', ISNULL(g.G_LastName, '')))) AS [Guardian Name],
                               g.G_Phone AS [Guardian Phone],
                               g.G_Email AS [Guardian Email],
                               CONVERT(VARCHAR(19), s.S_CreatedAt, 120) AS [Registered Date]
                        FROM Students_S s
                        LEFT JOIN Branches_B b ON s.S_BranchId = b.B_Id
                        LEFT JOIN Batches_BT bt ON s.S_BatchId = bt.BT_Id
                        LEFT JOIN Courses_C c ON (s.S_ClassId = c.C_Id OR bt.BT_CourseId = c.C_Id)
                        LEFT JOIN Programs_P p ON c.C_ProgramId = p.P_Id
                        LEFT JOIN AcademicYears_AY ay ON bt.BT_AcademicYearId = ay.AY_Id
                        LEFT JOIN Enrollments_E e ON (e.E_StudentId = s.S_Id AND e.E_Status = 'Active')
                        LEFT JOIN Students_Guardians sg ON (sg.SG_StudentId = s.S_Id AND sg.SG_IsPrimary = 1)
                        LEFT JOIN Guardians_G g ON sg.SG_GuardianId = g.G_Id
                        WHERE s.S_TenantId = @TenantId AND s.S_DeletedAt IS NULL ");

                    ApplyStandardFilters(sb, parameters, filter,
                        academicYearCol: "bt.BT_AcademicYearId",
                        branchCol: "s.S_BranchId",
                        courseCol: "c.C_Id",
                        batchCol: "s.S_BatchId",
                        dateCol: "s.S_CreatedAt",
                        statusCol: "s.S_Status",
                        searchCols: new[] { "s.S_FirstName", "s.S_LastName", "s.S_StudentCode", "s.S_AdmissionNumber", "s.S_Email", "s.S_Phone" });

                    sb.Append(" ORDER BY s.S_CreatedAt DESC;");
                    break;

                case "AdmissionApplications":
                    sb.Append(@"
                        SELECT aa.AA_ApplicationNumber AS [Application Number],
                               LTRIM(RTRIM(CONCAT(aa.AA_FirstName, ' ', ISNULL(aa.AA_LastName, '')))) AS [Applicant Name],
                               COALESCE(c.C_Name, aa.AA_AppliedGrade, '-') AS [Applied Course / Grade],
                               aa.AA_Status AS [Status],
                               aa.AA_Gender AS [Gender],
                               CONVERT(VARCHAR(10), aa.AA_DateOfBirth, 120) AS [Date Of Birth],
                               aa.AA_Email AS [Email],
                               aa.AA_Phone AS [Phone],
                               COALESCE(b.B_Name, aa.AA_CampusName, '-') AS [Campus / Branch],
                               ay.AY_Name AS [Academic Year],
                               aa.AA_FatherName AS [Father Name],
                               aa.AA_FatherPhone AS [Father Phone],
                               aa.AA_MotherName AS [Mother Name],
                               aa.AA_GuardianName AS [Guardian Name],
                               aa.AA_Address AS [Address],
                               aa.AA_City AS [City],
                               aa.AA_State AS [State],
                               aa.AA_Pincode AS [Pincode],
                               aa.AA_ReviewRemarks AS [Review Remarks],
                               CONVERT(VARCHAR(19), aa.AA_CreatedAt, 120) AS [Submission Date]
                        FROM AdmissionApplications_AA aa
                        LEFT JOIN AcademicYears_AY ay ON aa.AA_AcademicYearId = ay.AY_Id
                        LEFT JOIN Branches_B b ON aa.AA_BranchId = b.B_Id
                        LEFT JOIN Courses_C c ON aa.AA_CourseId = c.C_Id
                        WHERE aa.AA_TenantId = @TenantId ");

                    ApplyStandardFilters(sb, parameters, filter,
                        academicYearCol: "aa.AA_AcademicYearId",
                        branchCol: "aa.AA_BranchId",
                        courseCol: "aa.AA_CourseId",
                        dateCol: "aa.AA_CreatedAt",
                        statusCol: "aa.AA_Status",
                        searchCols: new[] { "aa.AA_FirstName", "aa.AA_LastName", "aa.AA_ApplicationNumber", "aa.AA_Email", "aa.AA_Phone", "aa.AA_CampusName" });

                    sb.Append(" ORDER BY aa.AA_CreatedAt DESC;");
                    break;

                case "Enrollments":
                    sb.Append(@"
                        SELECT e.E_EnrollmentNumber AS [Enrollment Number],
                               s.S_StudentCode AS [Student Code],
                               LTRIM(RTRIM(CONCAT(s.S_FirstName, ' ', ISNULL(s.S_LastName, '')))) AS [Student Name],
                               e.E_RollNumber AS [Roll Number],
                               b.B_Name AS [Branch],
                               c.C_Name AS [Course],
                               bt.BT_Name AS [Batch],
                               ay.AY_Name AS [Academic Year],
                               e.E_Status AS [Status],
                               e.E_EnrollmentType AS [Enrollment Type],
                               CONVERT(VARCHAR(10), e.E_EnrollmentDate, 120) AS [Enrollment Date],
                               e.E_Remarks AS [Remarks],
                               CONVERT(VARCHAR(19), e.E_CreatedAt, 120) AS [Created Date]
                        FROM Enrollments_E e
                        LEFT JOIN Students_S s ON e.E_StudentId = s.S_Id
                        LEFT JOIN Branches_B b ON e.E_BranchId = b.B_Id
                        LEFT JOIN Courses_C c ON e.E_CourseId = c.C_Id
                        LEFT JOIN Batches_BT bt ON e.E_BatchId = bt.BT_Id
                        LEFT JOIN AcademicYears_AY ay ON e.E_AcademicYearId = ay.AY_Id
                        WHERE e.E_TenantId = @TenantId ");

                    ApplyStandardFilters(sb, parameters, filter,
                        academicYearCol: "e.E_AcademicYearId",
                        branchCol: "e.E_BranchId",
                        courseCol: "e.E_CourseId",
                        batchCol: "e.E_BatchId",
                        dateCol: "e.E_CreatedAt",
                        statusCol: "e.E_Status",
                        searchCols: new[] { "e.E_EnrollmentNumber", "s.S_StudentCode", "s.S_FirstName", "s.S_LastName", "e.E_RollNumber" });

                    sb.Append(" ORDER BY e.E_CreatedAt DESC;");
                    break;

                case "Teachers":
                    sb.Append(@"
                        SELECT t.T_EmployeeCode AS [Employee Code],
                               COALESCE(LTRIM(RTRIM(CONCAT(st.ST_FirstName, ' ', ISNULL(st.ST_LastName, '')))), t.T_EmployeeCode) AS [Teacher Name],
                               st.ST_Email AS [Email],
                               st.ST_Phone AS [Phone],
                               st.ST_Gender AS [Gender],
                               COALESCE(t.T_Qualification, st.ST_Qualification) AS [Qualification],
                               COALESCE(t.T_ExperienceYears, st.ST_ExperienceYears) AS [Experience Years],
                               COALESCE(ds.DS_Name, t.T_Designation, st_ds.DS_Name, '-') AS [Designation],
                               COALESCE(dp.D_Name, t.T_Department, st_dp.D_Name, '-') AS [Department],
                               b.B_Name AS [Branch],
                               t.T_BloodGroup AS [Blood Group],
                               t.T_Status AS [Status],
                               CONVERT(VARCHAR(10), COALESCE(t.T_JoiningDate, st.ST_JoiningDate), 120) AS [Date Of Joining],
                               CONVERT(VARCHAR(19), t.T_CreatedAt, 120) AS [Created Date]
                        FROM Teachers_T t
                        LEFT JOIN Branches_B b ON t.T_BranchId = b.B_Id
                        LEFT JOIN Staff_ST st ON (t.T_EmployeeCode = st.ST_EmployeeCode OR t.T_Id = st.ST_Id)
                        LEFT JOIN Designations_DS ds ON TRY_CAST(t.T_Designation AS UNIQUEIDENTIFIER) = ds.DS_Id
                        LEFT JOIN Departments_D dp ON TRY_CAST(t.T_Department AS UNIQUEIDENTIFIER) = dp.D_Id
                        LEFT JOIN Designations_DS st_ds ON st.ST_DesignationId = st_ds.DS_Id
                        LEFT JOIN Departments_D st_dp ON st.ST_DepartmentId = st_dp.D_Id
                        WHERE t.T_TenantId = @TenantId AND t.T_IsActive = 1 ");

                    ApplyStandardFilters(sb, parameters, filter,
                        branchCol: "t.T_BranchId",
                        statusCol: "t.T_Status",
                        searchCols: new[] { "st.ST_FirstName", "st.ST_LastName", "t.T_EmployeeCode", "st.ST_Email", "st.ST_Phone" });

                    sb.Append(" ORDER BY t.T_CreatedAt DESC;");
                    break;

                case "Staff":
                    sb.Append(@"
                        SELECT st.ST_EmployeeCode AS [Employee Code],
                               LTRIM(RTRIM(CONCAT(st.ST_FirstName, ' ', ISNULL(st.ST_LastName, '')))) AS [Staff Name],
                               st.ST_Email AS [Email],
                               st.ST_Phone AS [Phone],
                               st.ST_Gender AS [Gender],
                               CONVERT(VARCHAR(10), st.ST_DateOfBirth, 120) AS [Date Of Birth],
                               b.B_Name AS [Branch],
                               d.D_Name AS [Department],
                               ds.DS_Name AS [Designation],
                               st.ST_Qualification AS [Qualification],
                               st.ST_ExperienceYears AS [Experience Years],
                               st.ST_BloodGroup AS [Blood Group],
                               st.ST_BasicSalary AS [Basic Salary],
                               st.ST_Address AS [Address],
                               st.ST_EmergencyContact AS [Emergency Contact],
                               st.ST_Status AS [Status],
                               CONVERT(VARCHAR(10), st.ST_JoiningDate, 120) AS [Date Of Joining],
                               CONVERT(VARCHAR(19), st.ST_CreatedAt, 120) AS [Created Date]
                        FROM Staff_ST st
                        LEFT JOIN Branches_B b ON st.ST_BranchId = b.B_Id
                        LEFT JOIN Departments_D d ON st.ST_DepartmentId = d.D_Id
                        LEFT JOIN Designations_DS ds ON st.ST_DesignationId = ds.DS_Id
                        WHERE st.ST_TenantId = @TenantId AND st.ST_DeletedAt IS NULL ");

                    ApplyStandardFilters(sb, parameters, filter,
                        branchCol: "st.ST_BranchId",
                        statusCol: "st.ST_Status",
                        dateCol: "st.ST_CreatedAt",
                        searchCols: new[] { "st.ST_FirstName", "st.ST_LastName", "st.ST_EmployeeCode", "st.ST_Email", "st.ST_Phone" });

                    sb.Append(" ORDER BY st.ST_CreatedAt DESC;");
                    break;

                case "FeeInvoices":
                    sb.Append(@"
                        SELECT fi.FI_InvoiceNumber AS [Invoice Number],
                               s.S_StudentCode AS [Student Code],
                               LTRIM(RTRIM(CONCAT(s.S_FirstName, ' ', ISNULL(s.S_LastName, '')))) AS [Student Name],
                               COALESCE(e.E_RollNumber, '-') AS [Roll Number],
                               b.B_Name AS [Branch],
                               c.C_Name AS [Course],
                               bt.BT_Name AS [Batch],
                               ay.AY_Name AS [Academic Year],
                               fi.FI_Subtotal AS [Subtotal],
                               fi.FI_DiscountAmount AS [Discount Amount],
                               fi.FI_TaxAmount AS [Tax Amount],
                               fi.FI_TotalAmount AS [Total Amount],
                               fi.FI_PaidAmount AS [Paid Amount],
                               fi.FI_BalanceAmount AS [Balance Due],
                               fi.FI_Status AS [Payment Status],
                               CONVERT(VARCHAR(10), fi.FI_InvoiceDate, 120) AS [Invoice Date],
                               CONVERT(VARCHAR(10), fi.FI_DueDate, 120) AS [Due Date],
                               fi.FI_Notes AS [Notes],
                               CONVERT(VARCHAR(19), fi.FI_CreatedAt, 120) AS [Created Date]
                        FROM FeeInvoices_FI fi
                        LEFT JOIN Students_S s ON fi.FI_StudentId = s.S_Id
                        LEFT JOIN Branches_B b ON s.S_BranchId = b.B_Id
                        LEFT JOIN Batches_BT bt ON s.S_BatchId = bt.BT_Id
                        LEFT JOIN Courses_C c ON (s.S_ClassId = c.C_Id OR bt.BT_CourseId = c.C_Id)
                        LEFT JOIN AcademicYears_AY ay ON bt.BT_AcademicYearId = ay.AY_Id
                        LEFT JOIN Enrollments_E e ON (e.E_StudentId = s.S_Id AND e.E_Status = 'Active')
                        WHERE fi.FI_TenantId = @TenantId ");

                    ApplyStandardFilters(sb, parameters, filter,
                        academicYearCol: "bt.BT_AcademicYearId",
                        branchCol: "s.S_BranchId",
                        courseCol: "c.C_Id",
                        batchCol: "s.S_BatchId",
                        dateCol: "fi.FI_CreatedAt",
                        statusCol: "fi.FI_Status",
                        searchCols: new[] { "fi.FI_InvoiceNumber", "s.S_StudentCode", "s.S_FirstName", "s.S_LastName", "fi.FI_Notes" });

                    sb.Append(" ORDER BY fi.FI_CreatedAt DESC;");
                    break;

                case "FeeStructures":
                    sb.Append(@"
                        SELECT fs.FS_Name AS [Fee Structure Name],
                               fs.FS_Code AS [Code],
                               ay.AY_Name AS [Academic Year],
                               c.C_Name AS [Course],
                               bt.BT_Name AS [Batch],
                               fs.FS_Description AS [Description],
                               CASE WHEN fs.FS_IsActive = 1 THEN 'Active' ELSE 'Inactive' END AS [Status],
                               CONVERT(VARCHAR(19), fs.FS_CreatedAt, 120) AS [Created Date]
                        FROM FeeStructures_FS fs
                        LEFT JOIN AcademicYears_AY ay ON fs.FS_AcademicYearId = ay.AY_Id
                        LEFT JOIN Courses_C c ON fs.FS_CourseId = c.C_Id
                        LEFT JOIN Batches_BT bt ON fs.FS_BatchId = bt.BT_Id
                        WHERE fs.FS_TenantId = @TenantId ");

                    ApplyStandardFilters(sb, parameters, filter,
                        academicYearCol: "fs.FS_AcademicYearId",
                        courseCol: "fs.FS_CourseId",
                        batchCol: "fs.FS_BatchId",
                        dateCol: "fs.FS_CreatedAt",
                        searchCols: new[] { "fs.FS_Name", "fs.FS_Code", "fs.FS_Description" });

                    sb.Append(" ORDER BY fs.FS_Name ASC;");
                    break;

                case "Attendance":
                    sb.Append(@"
                        SELECT CONVERT(VARCHAR(10), asess.AS_AttendanceDate, 120) AS [Attendance Date],
                               s.S_StudentCode AS [Student Code],
                               LTRIM(RTRIM(CONCAT(s.S_FirstName, ' ', ISNULL(s.S_LastName, '')))) AS [Student Name],
                               COALESCE(e.E_RollNumber, '-') AS [Roll Number],
                               b.B_Name AS [Branch],
                               c.C_Name AS [Course],
                               bt.BT_Name AS [Batch],
                               sb.SB_Name AS [Subject],
                               ay.AY_Name AS [Academic Year],
                               ar.AR_Status AS [Attendance Status],
                               ar.AR_Remarks AS [Remarks]
                        FROM AttendanceRecords_AR ar
                        INNER JOIN AttendanceSessions_AS asess ON ar.AR_AttendanceSessionId = asess.AS_Id
                        LEFT JOIN Students_S s ON ar.AR_StudentId = s.S_Id
                        LEFT JOIN Batches_BT bt ON asess.AS_BatchId = bt.BT_Id
                        LEFT JOIN Branches_B b ON asess.AS_BranchId = b.B_Id
                        LEFT JOIN Courses_C c ON bt.BT_CourseId = c.C_Id
                        LEFT JOIN AcademicYears_AY ay ON bt.BT_AcademicYearId = ay.AY_Id
                        LEFT JOIN Subjects_SB sb ON asess.AS_SubjectId = sb.SB_Id
                        LEFT JOIN Enrollments_E e ON (e.E_StudentId = s.S_Id AND e.E_Status = 'Active')
                        WHERE asess.AS_TenantId = @TenantId ");

                    ApplyStandardFilters(sb, parameters, filter,
                        academicYearCol: "bt.BT_AcademicYearId",
                        branchCol: "asess.AS_BranchId",
                        courseCol: "bt.BT_CourseId",
                        batchCol: "asess.AS_BatchId",
                        dateCol: "asess.AS_AttendanceDate",
                        statusCol: "ar.AR_Status",
                        searchCols: new[] { "s.S_StudentCode", "s.S_FirstName", "s.S_LastName", "ar.AR_Remarks" });

                    sb.Append(" ORDER BY asess.AS_AttendanceDate DESC;");
                    break;

                case "ExamsAndMarks":
                    sb.Append(@"
                        SELECT ex.EX_Name AS [Exam Name],
                               ex.EX_Code AS [Exam Code],
                               sb.SB_Name AS [Subject],
                               s.S_StudentCode AS [Student Code],
                               LTRIM(RTRIM(CONCAT(s.S_FirstName, ' ', ISNULL(s.S_LastName, '')))) AS [Student Name],
                               COALESCE(e.E_RollNumber, '-') AS [Roll Number],
                               c.C_Name AS [Course],
                               bt.BT_Name AS [Batch],
                               ay.AY_Name AS [Academic Year],
                               m.M_MarksObtained AS [Marks Obtained],
                               es.ES_MaxMarks AS [Max Marks],
                               es.ES_PassMarks AS [Pass Marks],
                               m.M_Percentage AS [Percentage],
                               m.M_Remarks AS [Remarks]
                        FROM Marks_M m
                        INNER JOIN ExamSubjects_ES es ON m.M_ExamSubjectId = es.ES_Id
                        INNER JOIN Exams_EX ex ON es.ES_ExamId = ex.EX_Id
                        LEFT JOIN Subjects_SB sb ON es.ES_SubjectId = sb.SB_Id
                        LEFT JOIN Students_S s ON m.M_StudentId = s.S_Id
                        LEFT JOIN Courses_C c ON ex.EX_CourseId = c.C_Id
                        LEFT JOIN Batches_BT bt ON ex.EX_BatchId = bt.BT_Id
                        LEFT JOIN AcademicYears_AY ay ON ex.EX_AcademicYearId = ay.AY_Id
                        LEFT JOIN Enrollments_E e ON (e.E_StudentId = s.S_Id AND e.E_Status = 'Active')
                        WHERE ex.EX_TenantId = @TenantId ");

                    ApplyStandardFilters(sb, parameters, filter,
                        academicYearCol: "ex.EX_AcademicYearId",
                        courseCol: "ex.EX_CourseId",
                        batchCol: "ex.EX_BatchId",
                        searchCols: new[] { "ex.EX_Name", "ex.EX_Code", "sb.SB_Name", "s.S_StudentCode", "s.S_FirstName", "s.S_LastName" });

                    sb.Append(" ORDER BY ex.EX_Name, s.S_FirstName;");
                    break;

                case "Timetable":
                    sb.Append(@"
                        SELECT CASE tt.TT_DayOfWeek 
                                   WHEN 1 THEN 'Monday'
                                   WHEN 2 THEN 'Tuesday'
                                   WHEN 3 THEN 'Wednesday'
                                   WHEN 4 THEN 'Thursday'
                                   WHEN 5 THEN 'Friday'
                                   WHEN 6 THEN 'Saturday'
                                   WHEN 7 THEN 'Sunday'
                                   WHEN 0 THEN 'Sunday'
                                   ELSE CAST(tt.TT_DayOfWeek AS VARCHAR(10)) 
                               END AS [Day],
                               b.B_Name AS [Branch],
                               c.C_Name AS [Course],
                               bt.BT_Name AS [Batch],
                               sb.SB_Name AS [Subject],
                               LTRIM(RTRIM(CONCAT(st.ST_FirstName, ' ', ISNULL(st.ST_LastName, '')))) AS [Teacher / Staff],
                               cr.CR_Name AS [Classroom],
                               CONVERT(VARCHAR(5), tt.TT_StartTime, 108) AS [Start Time],
                               CONVERT(VARCHAR(5), tt.TT_EndTime, 108) AS [End Time],
                               CONVERT(VARCHAR(10), tt.TT_EffectiveFrom, 120) AS [Effective From],
                               CONVERT(VARCHAR(10), tt.TT_EffectiveTo, 120) AS [Effective To]
                        FROM Timetables_TT tt
                        LEFT JOIN Branches_B b ON tt.TT_BranchId = b.B_Id
                        LEFT JOIN Batches_BT bt ON tt.TT_BatchId = bt.BT_Id
                        LEFT JOIN Courses_C c ON bt.BT_CourseId = c.C_Id
                        LEFT JOIN Subjects_SB sb ON tt.TT_SubjectId = sb.SB_Id
                        LEFT JOIN Staff_ST st ON tt.TT_StaffId = st.ST_Id
                        LEFT JOIN Classrooms_CR cr ON tt.TT_ClassroomId = cr.CR_Id
                        WHERE tt.TT_TenantId = @TenantId ");

                    ApplyStandardFilters(sb, parameters, filter,
                        branchCol: "tt.TT_BranchId",
                        courseCol: "bt.BT_CourseId",
                        batchCol: "tt.TT_BatchId",
                        searchCols: new[] { "sb.SB_Name", "cr.CR_Name", "st.ST_FirstName", "st.ST_LastName", "st.ST_EmployeeCode" });

                    sb.Append(" ORDER BY tt.TT_DayOfWeek, tt.TT_StartTime;");
                    break;

                case "Expenses":
                    sb.Append(@"
                        SELECT exp.EXP_ExpenseNumber AS [Expense Number],
                               ec.EC_Name AS [Category],
                               b.B_Name AS [Branch],
                               exp.EXP_Amount AS [Amount],
                               CONVERT(VARCHAR(10), exp.EXP_ExpenseDate, 120) AS [Expense Date],
                               v.V_Name AS [Vendor],
                               pm.PM_Name AS [Payment Method],
                               exp.EXP_Description AS [Description],
                               CONVERT(VARCHAR(19), exp.EXP_CreatedAt, 120) AS [Created Date]
                        FROM Expenses_EXP exp
                        LEFT JOIN ExpenseCategories_EC ec ON exp.EXP_ExpenseCategoryId = ec.EC_Id
                        LEFT JOIN Branches_B b ON exp.EXP_BranchId = b.B_Id
                        LEFT JOIN Vendors_V v ON exp.EXP_VendorId = v.V_Id
                        LEFT JOIN PaymentMethods_PM pm ON exp.EXP_PaymentMethodId = pm.PM_Id
                        WHERE exp.EXP_TenantId = @TenantId ");

                    ApplyStandardFilters(sb, parameters, filter,
                        branchCol: "exp.EXP_BranchId",
                        dateCol: "exp.EXP_ExpenseDate",
                        searchCols: new[] { "exp.EXP_ExpenseNumber", "ec.EC_Name", "v.V_Name", "exp.EXP_Description" });

                    sb.Append(" ORDER BY exp.EXP_ExpenseDate DESC;");
                    break;

                case "MasterData":
                default:
                    sb.Append(@"
                        SELECT 'Course' AS [Entity Type], c.C_Code AS [Code], c.C_Name AS [Name], p.P_Name AS [Parent Group / Program], CASE WHEN c.C_Status = 'active' THEN 'Active' ELSE 'Inactive' END AS [Status]
                        FROM Courses_C c 
                        LEFT JOIN Programs_P p ON c.C_ProgramId = p.P_Id 
                        WHERE c.C_TenantId = @TenantId AND c.C_DeletedAt IS NULL
                        UNION ALL
                        SELECT 'Branch', b.B_Code, b.B_Name, b.B_City, CASE WHEN b.B_Status = 'active' THEN 'Active' ELSE 'Inactive' END
                        FROM Branches_B b 
                        WHERE b.B_TenantId = @TenantId AND b.B_DeletedAt IS NULL
                        UNION ALL
                        SELECT 'Batch', bt.BT_Code, bt.BT_Name, c.C_Name, CASE WHEN bt.BT_Status = 'active' THEN 'Active' ELSE 'Inactive' END
                        FROM Batches_BT bt
                        LEFT JOIN Courses_C c ON bt.BT_CourseId = c.C_Id
                        WHERE bt.BT_TenantId = @TenantId
                        UNION ALL
                        SELECT 'Subject', sb.SB_Code, sb.SB_Name, CONCAT('Credits: ', ISNULL(CAST(sb.SB_Credits AS VARCHAR(10)), '0'), ', Max Marks: ', ISNULL(CAST(sb.SB_MaxMarks AS VARCHAR(10)), '100')), 'Active'
                        FROM Subjects_SB sb 
                        WHERE sb.SB_TenantId = @TenantId
                        UNION ALL
                        SELECT 'Department', d.D_Code, d.D_Name, b.B_Name, 'Active'
                        FROM Departments_D d 
                        LEFT JOIN Branches_B b ON d.D_BranchId = b.B_Id 
                        WHERE d.D_TenantId = @TenantId
                        UNION ALL
                        SELECT 'Classroom', cr.CR_Code, cr.CR_Name, CONCAT(b.B_Name, ' (Cap: ', cr.CR_Capacity, ')'), 'Active'
                        FROM Classrooms_CR cr
                        LEFT JOIN Branches_B b ON cr.CR_BranchId = b.B_Id
                        WHERE cr.CR_TenantId = @TenantId
                        UNION ALL
                        SELECT 'Academic Year', ay.AY_Code, ay.AY_Name, CONCAT(CONVERT(VARCHAR(10), ay.AY_StartDate, 120), ' to ', CONVERT(VARCHAR(10), ay.AY_EndDate, 120)), CASE WHEN ay.AY_IsCurrent = 1 THEN 'Current' ELSE 'Standard' END
                        FROM AcademicYears_AY ay 
                        WHERE ay.AY_TenantId = @TenantId
                        ORDER BY [Entity Type], [Name];");
                    break;
            }

            return _dbHelper.ExecuteDataTable(sb.ToString(), parameters);
        }

        private void ApplyStandardFilters(StringBuilder sb, Dictionary<string, object> parameters, DataBackupFilterViewModel filter,
            string academicYearCol = null, string branchCol = null, string programCol = null, string courseCol = null,
            string batchCol = null, string dateCol = null, string statusCol = null, string[] searchCols = null)
        {
            if (filter == null) return;

            if (academicYearCol != null && filter.AcademicYearId.HasValue && filter.AcademicYearId.Value != Guid.Empty)
            {
                sb.Append($" AND {academicYearCol} = @F_AcademicYearId ");
                parameters["@F_AcademicYearId"] = filter.AcademicYearId.Value;
            }

            if (branchCol != null && filter.BranchId.HasValue && filter.BranchId.Value != Guid.Empty)
            {
                sb.Append($" AND {branchCol} = @F_BranchId ");
                parameters["@F_BranchId"] = filter.BranchId.Value;
            }

            if (programCol != null && filter.ProgramId.HasValue && filter.ProgramId.Value != Guid.Empty)
            {
                sb.Append($" AND {programCol} = @F_ProgramId ");
                parameters["@F_ProgramId"] = filter.ProgramId.Value;
            }

            if (courseCol != null && filter.CourseId.HasValue && filter.CourseId.Value != Guid.Empty)
            {
                sb.Append($" AND {courseCol} = @F_CourseId ");
                parameters["@F_CourseId"] = filter.CourseId.Value;
            }

            if (batchCol != null && filter.BatchId.HasValue && filter.BatchId.Value != Guid.Empty)
            {
                sb.Append($" AND {batchCol} = @F_BatchId ");
                parameters["@F_BatchId"] = filter.BatchId.Value;
            }

            if (dateCol != null)
            {
                if (filter.StartDate.HasValue)
                {
                    sb.Append($" AND {dateCol} >= @F_StartDate ");
                    parameters["@F_StartDate"] = filter.StartDate.Value.Date;
                }
                if (filter.EndDate.HasValue)
                {
                    sb.Append($" AND {dateCol} <= @F_EndDate ");
                    parameters["@F_EndDate"] = filter.EndDate.Value.Date.AddDays(1).AddTicks(-1);
                }
            }

            if (statusCol != null && !string.IsNullOrWhiteSpace(filter.Status) && !string.Equals(filter.Status, "All", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append($" AND {statusCol} = @F_Status ");
                parameters["@F_Status"] = filter.Status.Trim();
            }

            if (searchCols != null && searchCols.Length > 0 && !string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var likeClauses = new List<string>();
                for (int i = 0; i < searchCols.Length; i++)
                {
                    likeClauses.Add($"{searchCols[i]} LIKE @F_SearchTerm");
                }
                sb.Append($" AND ({string.Join(" OR ", likeClauses)}) ");
                parameters["@F_SearchTerm"] = $"%{filter.SearchTerm.Trim()}%";
            }
        }
    }
}
