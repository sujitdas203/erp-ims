using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using IMS.DAL.Common;
using IMS.DAL.Interfaces;
using IMS.Models.Common.Navigation;

namespace IMS.DAL
{
    public class SidebarMenuDAL : ISidebarMenuDAL
    {
        private readonly DBHelper _dbHelper;
        private static bool _tableChecked = false;
        private static readonly object _lock = new object();

        public SidebarMenuDAL(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task EnsureTableAndSeedAsync()
        {
            if (_tableChecked) return;

            lock (_lock)
            {
                if (_tableChecked) return;
            }

            const string ensureTableAndSeedSql = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SidebarMenuItems_SBM]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE [dbo].[SidebarMenuItems_SBM] (
                        [Id]                  INT IDENTITY(1,1) PRIMARY KEY,
                        [SectionTitle]        NVARCHAR(100) NOT NULL,
                        [SectionOrder]        INT NOT NULL DEFAULT(1),
                        [Title]               NVARCHAR(100) NOT NULL,
                        [Controller]          NVARCHAR(100) NULL,
                        [Action]              NVARCHAR(100) NULL DEFAULT('Index'),
                        [Area]                NVARCHAR(100) NULL DEFAULT(''),
                        [Icon]                NVARCHAR(100) NOT NULL DEFAULT('fa-circle'),
                        [PermissionCode]      NVARCHAR(150) NULL,
                        [MenuOrder]           INT NOT NULL DEFAULT(1),
                        [BadgeText]           NVARCHAR(50) NULL,
                        [BadgeClass]          NVARCHAR(50) NULL DEFAULT('IMS-soon-badge'),
                        [CustomUrl]           NVARCHAR(255) NULL,
                        [IsCollapsible]       BIT NOT NULL DEFAULT(0),
                        [CollapseTargetId]    NVARCHAR(100) NULL,
                        [IsMasterManagement]  BIT NOT NULL DEFAULT(0),
                        [IsActive]            BIT NOT NULL DEFAULT(1),
                        [CreatedAt]           DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
                        [UpdatedAt]           DATETIME2 NULL
                    );

                    CREATE INDEX IX_SidebarMenuItems_Order 
                    ON [dbo].[SidebarMenuItems_SBM] ([IsActive], [SectionOrder], [MenuOrder]);
                END;

                IF (SELECT COUNT(1) FROM [dbo].[SidebarMenuItems_SBM]) = 0
                BEGIN
                    INSERT INTO [dbo].[SidebarMenuItems_SBM]
                    ([SectionTitle], [SectionOrder], [Title], [Controller], [Action], [Area], [Icon], [PermissionCode], [MenuOrder], [BadgeText], [BadgeClass], [CustomUrl], [IsCollapsible], [CollapseTargetId], [IsMasterManagement], [IsActive])
                    VALUES
                    ('', 0, 'Dashboard', 'Home', 'Index', '', 'fa-gauge-high', 'ViewDashboard', 1, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Students & Admissions', 1, 'Admissions', 'AdmissionApplication', 'Index', '', 'fa-address-card', 'ViewAdmissionApplication', 1, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Students & Admissions', 1, 'Enrollments', 'Enrollment', 'Index', '', 'fa-user-plus', 'ViewEnrollment', 2, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Students & Admissions', 1, 'Students', 'Students', 'Index', '', 'fa-user-graduate', 'ViewStudent', 3, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Students & Admissions', 1, 'Student Leaves', 'StudentLeave', 'Index', '', 'fa-user-clock', 'ViewStudentLeave', 4, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Students & Admissions', 1, 'Transfer Certificates (TC)', 'TransferCertificate', 'Index', '', 'fa-file-shield', 'ViewTransferCertificate', 5, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Academic Delivery', 2, 'Batches', 'Batch', 'Index', '', 'fa-users-rectangle', 'ViewBatch', 1, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Academic Delivery', 2, 'Course Subjects', 'CourseSubject', 'Index', '', 'fa-book-bookmark', 'ViewCourseSubject', 2, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Academic Delivery', 2, 'Syllabus', 'Syllabus', 'Index', '', 'fa-book-open-reader', 'ViewSyllabus', 3, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Academic Delivery', 2, 'Timetable', 'Timetable', 'Index', '', 'fa-calendar-days', 'ViewTimetable', 4, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Academic Delivery', 2, 'Home Tasks', 'HomeTasks', 'Index', '', 'fa-list-check', 'ViewHomeTasks', 5, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Academic Delivery', 2, 'Mock Tests', 'MockTests', 'Index', '', 'fa-vial-circle-check', 'ViewMockTests', 6, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Attendance Centre', 3, 'Student Attendance', 'Attendance', 'Index', '', 'fa-calendar-check', 'ViewAttendance', 1, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Attendance Centre', 3, 'Teacher Attendance', 'TeacherAttendance', 'Index', '', 'fa-clipboard-user', 'ViewTeacherAttendance', 2, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('HR & Faculty', 4, 'Teachers', 'Teachers', 'Index', '', 'fa-chalkboard-user', 'ViewTeacher', 1, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('HR & Faculty', 4, 'Staff', 'Staff', 'Index', '', 'fa-user-tie', 'ViewStaff', 2, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('HR & Faculty', 4, 'Teacher Leaves', 'TeacherLeave', 'Index', '', 'fa-calendar-xmark', 'ViewTeacherLeave', 3, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Examinations', 5, 'Examinations', 'Exam', 'Index', '', 'fa-file-pen', 'ViewExam', 1, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Finance', 6, 'Fees & Payments', 'Fees', 'Index', '', 'fa-indian-rupee-sign', 'ViewFees', 1, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Finance', 6, 'Expenses', 'Expense', 'Index', '', 'fa-receipt', 'ViewExpense', 2, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Communication', 7, 'Announcements', 'Announcements', 'Index', '', 'fa-bullhorn', 'ViewAnnouncement', 1, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('Communication', 7, 'Notification Hub', 'NotificationConfig', 'Index', '', 'fa-sliders', 'ViewNotificationConfig', 2, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('System Setup', 8, 'Master Management', 'Master', 'Index', '', 'fa-database', 'ViewMaster', 1, NULL, NULL, NULL, 1, 'masterMenuCollapse', 1, 1),
                    ('System Setup', 8, 'Sidebar Management', 'SidebarMenu', 'Index', '', 'fa-bars-staggered', 'ViewSidebarMenu', 2, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('System Setup', 8, 'Data Backup & Export', 'DataBackup', 'Index', '', 'fa-cloud-arrow-down', 'ViewDataBackup', 3, NULL, NULL, NULL, 0, NULL, 0, 1),
                    ('System Setup', 8, 'Users & Roles', '', '', '', 'fa-user-shield', 'ViewUserRoles', 4, 'Soon', 'IMS-soon-badge', 'javascript:void(0);', 0, NULL, 0, 1),
                    ('Insights', 9, 'Reports', '', '', '', 'fa-chart-line', 'ViewReports', 1, 'Soon', 'IMS-soon-badge', 'javascript:void(0);', 0, NULL, 0, 1);
                END;";

            try
            {
                using var conn = _dbHelper.GetConnection();
                using var cmd = new SqlCommand(ensureTableAndSeedSql, conn);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                _tableChecked = true;
            }
            catch
            {
                // Suppress on concurrency; table check will retry if needed
            }
        }

        public async Task<List<SidebarMenuItemModel>> GetActiveMenuItemsAsync()
        {
            try
            {
                await EnsureTableAndSeedAsync();
                var list = new List<SidebarMenuItemModel>();

                const string sql = @"
                    SELECT [Id], [SectionTitle], [SectionOrder], [Title], [Controller], [Action], [Area], [Icon], 
                           [PermissionCode], [MenuOrder], [BadgeText], [BadgeClass], [CustomUrl], [IsCollapsible], 
                           [CollapseTargetId], [IsMasterManagement], [IsActive], [CreatedAt], [UpdatedAt]
                    FROM [dbo].[SidebarMenuItems_SBM]
                    WHERE [IsActive] = 1
                    ORDER BY [SectionOrder] ASC, [MenuOrder] ASC;";

                using var conn = _dbHelper.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(MapRow(reader));
                }

                if (list.Count > 0)
                {
                    return list;
                }
            }
            catch
            {
                // Fallback to built-in list if database is unreachable or table not populated
            }

            return GetDefaultMenuItems();
        }

        public async Task<List<SidebarMenuItemModel>> GetAllMenuItemsAsync()
        {
            try
            {
                await EnsureTableAndSeedAsync();
                var list = new List<SidebarMenuItemModel>();

                const string sql = @"
                    SELECT [Id], [SectionTitle], [SectionOrder], [Title], [Controller], [Action], [Area], [Icon], 
                           [PermissionCode], [MenuOrder], [BadgeText], [BadgeClass], [CustomUrl], [IsCollapsible], 
                           [CollapseTargetId], [IsMasterManagement], [IsActive], [CreatedAt], [UpdatedAt]
                    FROM [dbo].[SidebarMenuItems_SBM]
                    ORDER BY [SectionOrder] ASC, [MenuOrder] ASC;";

                using var conn = _dbHelper.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(MapRow(reader));
                }

                if (list.Count > 0)
                {
                    return list;
                }
            }
            catch
            {
                // Fallback to defaults
            }

            return GetDefaultMenuItems();
        }

        public async Task<SidebarMenuItemModel?> GetByIdAsync(int id)
        {
            try
            {
                await EnsureTableAndSeedAsync();
                const string sql = @"
                    SELECT [Id], [SectionTitle], [SectionOrder], [Title], [Controller], [Action], [Area], [Icon], 
                           [PermissionCode], [MenuOrder], [BadgeText], [BadgeClass], [CustomUrl], [IsCollapsible], 
                           [CollapseTargetId], [IsMasterManagement], [IsActive], [CreatedAt], [UpdatedAt]
                    FROM [dbo].[SidebarMenuItems_SBM]
                    WHERE [Id] = @Id;";

                using var conn = _dbHelper.GetConnection();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return MapRow(reader);
                }
            }
            catch
            {
            }

            return null;
        }

        public async Task<int> CreateAsync(SidebarMenuItemModel item)
        {
            await EnsureTableAndSeedAsync();
            const string sql = @"
                INSERT INTO [dbo].[SidebarMenuItems_SBM]
                ([SectionTitle], [SectionOrder], [Title], [Controller], [Action], [Area], [Icon], [PermissionCode], [MenuOrder], [BadgeText], [BadgeClass], [CustomUrl], [IsCollapsible], [CollapseTargetId], [IsMasterManagement], [IsActive], [CreatedAt])
                VALUES
                (@SectionTitle, @SectionOrder, @Title, @Controller, @Action, @Area, @Icon, @PermissionCode, @MenuOrder, @BadgeText, @BadgeClass, @CustomUrl, @IsCollapsible, @CollapseTargetId, @IsMasterManagement, @IsActive, GETUTCDATE());
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var conn = _dbHelper.GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            AddParameters(cmd, item);

            await conn.OpenAsync();
            var res = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(res);
        }

        public async Task<bool> UpdateAsync(SidebarMenuItemModel item)
        {
            await EnsureTableAndSeedAsync();
            const string sql = @"
                UPDATE [dbo].[SidebarMenuItems_SBM]
                SET [SectionTitle] = @SectionTitle,
                    [SectionOrder] = @SectionOrder,
                    [Title] = @Title,
                    [Controller] = @Controller,
                    [Action] = @Action,
                    [Area] = @Area,
                    [Icon] = @Icon,
                    [PermissionCode] = @PermissionCode,
                    [MenuOrder] = @MenuOrder,
                    [BadgeText] = @BadgeText,
                    [BadgeClass] = @BadgeClass,
                    [CustomUrl] = @CustomUrl,
                    [IsCollapsible] = @IsCollapsible,
                    [CollapseTargetId] = @CollapseTargetId,
                    [IsMasterManagement] = @IsMasterManagement,
                    [IsActive] = @IsActive,
                    [UpdatedAt] = GETUTCDATE()
                WHERE [Id] = @Id;";

            using var conn = _dbHelper.GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = item.Id;
            AddParameters(cmd, item);

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await EnsureTableAndSeedAsync();
            const string sql = "DELETE FROM [dbo].[SidebarMenuItems_SBM] WHERE [Id] = @Id;";

            using var conn = _dbHelper.GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            await EnsureTableAndSeedAsync();
            const string sql = @"
                UPDATE [dbo].[SidebarMenuItems_SBM]
                SET [IsActive] = CASE WHEN [IsActive] = 1 THEN 0 ELSE 1 END,
                    [UpdatedAt] = GETUTCDATE()
                WHERE [Id] = @Id;";

            using var conn = _dbHelper.GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> UpdateOrderAsync(int id, int sectionOrder, int menuOrder)
        {
            await EnsureTableAndSeedAsync();
            const string sql = @"
                UPDATE [dbo].[SidebarMenuItems_SBM]
                SET [SectionOrder] = @SectionOrder,
                    [MenuOrder] = @MenuOrder,
                    [UpdatedAt] = GETUTCDATE()
                WHERE [Id] = @Id;";

            using var conn = _dbHelper.GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@SectionOrder", SqlDbType.Int).Value = sectionOrder;
            cmd.Parameters.Add("@MenuOrder", SqlDbType.Int).Value = menuOrder;

            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        private static void AddParameters(SqlCommand cmd, SidebarMenuItemModel item)
        {
            cmd.Parameters.Add("@SectionTitle", SqlDbType.NVarChar, 100).Value = item.SectionTitle ?? "";
            cmd.Parameters.Add("@SectionOrder", SqlDbType.Int).Value = item.SectionOrder;
            cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 100).Value = item.Title ?? "";
            cmd.Parameters.Add("@Controller", SqlDbType.NVarChar, 100).Value = (object?)item.Controller ?? DBNull.Value;
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar, 100).Value = (object?)item.Action ?? DBNull.Value;
            cmd.Parameters.Add("@Area", SqlDbType.NVarChar, 100).Value = (object?)item.Area ?? DBNull.Value;
            cmd.Parameters.Add("@Icon", SqlDbType.NVarChar, 100).Value = item.Icon ?? "fa-circle";
            cmd.Parameters.Add("@PermissionCode", SqlDbType.NVarChar, 150).Value = (object?)item.PermissionCode ?? DBNull.Value;
            cmd.Parameters.Add("@MenuOrder", SqlDbType.Int).Value = item.MenuOrder;
            cmd.Parameters.Add("@BadgeText", SqlDbType.NVarChar, 50).Value = (object?)item.BadgeText ?? DBNull.Value;
            cmd.Parameters.Add("@BadgeClass", SqlDbType.NVarChar, 50).Value = (object?)item.BadgeClass ?? DBNull.Value;
            cmd.Parameters.Add("@CustomUrl", SqlDbType.NVarChar, 255).Value = (object?)item.CustomUrl ?? DBNull.Value;
            cmd.Parameters.Add("@IsCollapsible", SqlDbType.Bit).Value = item.IsCollapsible;
            cmd.Parameters.Add("@CollapseTargetId", SqlDbType.NVarChar, 100).Value = (object?)item.CollapseTargetId ?? DBNull.Value;
            cmd.Parameters.Add("@IsMasterManagement", SqlDbType.Bit).Value = item.IsMasterManagement;
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = item.IsActive;
        }

        private static SidebarMenuItemModel MapRow(SqlDataReader reader)
        {
            return new SidebarMenuItemModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                SectionTitle = reader.IsDBNull(reader.GetOrdinal("SectionTitle")) ? "" : reader.GetString(reader.GetOrdinal("SectionTitle")),
                SectionOrder = reader.GetInt32(reader.GetOrdinal("SectionOrder")),
                Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? "" : reader.GetString(reader.GetOrdinal("Title")),
                Controller = reader.IsDBNull(reader.GetOrdinal("Controller")) ? null : reader.GetString(reader.GetOrdinal("Controller")),
                Action = reader.IsDBNull(reader.GetOrdinal("Action")) ? null : reader.GetString(reader.GetOrdinal("Action")),
                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? null : reader.GetString(reader.GetOrdinal("Area")),
                Icon = reader.IsDBNull(reader.GetOrdinal("Icon")) ? "fa-circle" : reader.GetString(reader.GetOrdinal("Icon")),
                PermissionCode = reader.IsDBNull(reader.GetOrdinal("PermissionCode")) ? null : reader.GetString(reader.GetOrdinal("PermissionCode")),
                MenuOrder = reader.GetInt32(reader.GetOrdinal("MenuOrder")),
                BadgeText = reader.IsDBNull(reader.GetOrdinal("BadgeText")) ? null : reader.GetString(reader.GetOrdinal("BadgeText")),
                BadgeClass = reader.IsDBNull(reader.GetOrdinal("BadgeClass")) ? null : reader.GetString(reader.GetOrdinal("BadgeClass")),
                CustomUrl = reader.IsDBNull(reader.GetOrdinal("CustomUrl")) ? null : reader.GetString(reader.GetOrdinal("CustomUrl")),
                IsCollapsible = reader.GetBoolean(reader.GetOrdinal("IsCollapsible")),
                CollapseTargetId = reader.IsDBNull(reader.GetOrdinal("CollapseTargetId")) ? null : reader.GetString(reader.GetOrdinal("CollapseTargetId")),
                IsMasterManagement = reader.GetBoolean(reader.GetOrdinal("IsMasterManagement")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }

        public static List<SidebarMenuItemModel> GetDefaultMenuItems()
        {
            return new List<SidebarMenuItemModel>
            {
                new() { Id = 1, SectionTitle = "", SectionOrder = 0, Title = "Dashboard", Controller = "Home", Action = "Index", Area = "", Icon = "fa-gauge-high", PermissionCode = "ViewDashboard", MenuOrder = 1, IsActive = true },
                
                // Students & Admissions
                new() { Id = 2, SectionTitle = "Students & Admissions", SectionOrder = 1, Title = "Admissions", Controller = "AdmissionApplication", Action = "Index", Area = "", Icon = "fa-address-card", PermissionCode = "ViewAdmissionApplication", MenuOrder = 1, IsActive = true },
                new() { Id = 3, SectionTitle = "Students & Admissions", SectionOrder = 1, Title = "Enrollments", Controller = "Enrollment", Action = "Index", Area = "", Icon = "fa-user-plus", PermissionCode = "ViewEnrollment", MenuOrder = 2, IsActive = true },
                new() { Id = 4, SectionTitle = "Students & Admissions", SectionOrder = 1, Title = "Students", Controller = "Students", Action = "Index", Area = "", Icon = "fa-user-graduate", PermissionCode = "ViewStudent", MenuOrder = 3, IsActive = true },
                new() { Id = 5, SectionTitle = "Students & Admissions", SectionOrder = 1, Title = "Student Leaves", Controller = "StudentLeave", Action = "Index", Area = "", Icon = "fa-user-clock", PermissionCode = "ViewStudentLeave", MenuOrder = 4, IsActive = true },
                new() { Id = 6, SectionTitle = "Students & Admissions", SectionOrder = 1, Title = "Transfer Certificates (TC)", Controller = "TransferCertificate", Action = "Index", Area = "", Icon = "fa-file-shield", PermissionCode = "ViewTransferCertificate", MenuOrder = 5, IsActive = true },

                // Academic Delivery
                new() { Id = 7, SectionTitle = "Academic Delivery", SectionOrder = 2, Title = "Batches", Controller = "Batch", Action = "Index", Area = "", Icon = "fa-users-rectangle", PermissionCode = "ViewBatch", MenuOrder = 1, IsActive = true },
                new() { Id = 8, SectionTitle = "Academic Delivery", SectionOrder = 2, Title = "Course Subjects", Controller = "CourseSubject", Action = "Index", Area = "", Icon = "fa-book-bookmark", PermissionCode = "ViewCourseSubject", MenuOrder = 2, IsActive = true },
                new() { Id = 9, SectionTitle = "Academic Delivery", SectionOrder = 2, Title = "Syllabus", Controller = "Syllabus", Action = "Index", Area = "", Icon = "fa-book-open-reader", PermissionCode = "ViewSyllabus", MenuOrder = 3, IsActive = true },
                new() { Id = 10, SectionTitle = "Academic Delivery", SectionOrder = 2, Title = "Timetable", Controller = "Timetable", Action = "Index", Area = "", Icon = "fa-calendar-days", PermissionCode = "ViewTimetable", MenuOrder = 4, IsActive = true },
                new() { Id = 11, SectionTitle = "Academic Delivery", SectionOrder = 2, Title = "Home Tasks", Controller = "HomeTasks", Action = "Index", Area = "", Icon = "fa-list-check", PermissionCode = "ViewHomeTasks", MenuOrder = 5, IsActive = true },
                new() { Id = 12, SectionTitle = "Academic Delivery", SectionOrder = 2, Title = "Mock Tests", Controller = "MockTests", Action = "Index", Area = "", Icon = "fa-vial-circle-check", PermissionCode = "ViewMockTests", MenuOrder = 6, IsActive = true },

                // Attendance Centre
                new() { Id = 13, SectionTitle = "Attendance Centre", SectionOrder = 3, Title = "Student Attendance", Controller = "Attendance", Action = "Index", Area = "", Icon = "fa-calendar-check", PermissionCode = "ViewAttendance", MenuOrder = 1, IsActive = true },
                new() { Id = 14, SectionTitle = "Attendance Centre", SectionOrder = 3, Title = "Teacher Attendance", Controller = "TeacherAttendance", Action = "Index", Area = "", Icon = "fa-clipboard-user", PermissionCode = "ViewTeacherAttendance", MenuOrder = 2, IsActive = true },

                // HR & Faculty
                new() { Id = 15, SectionTitle = "HR & Faculty", SectionOrder = 4, Title = "Teachers", Controller = "Teachers", Action = "Index", Area = "", Icon = "fa-chalkboard-user", PermissionCode = "ViewTeacher", MenuOrder = 1, IsActive = true },
                new() { Id = 16, SectionTitle = "HR & Faculty", SectionOrder = 4, Title = "Staff", Controller = "Staff", Action = "Index", Area = "", Icon = "fa-user-tie", PermissionCode = "ViewStaff", MenuOrder = 2, IsActive = true },
                new() { Id = 17, SectionTitle = "HR & Faculty", SectionOrder = 4, Title = "Teacher Leaves", Controller = "TeacherLeave", Action = "Index", Area = "", Icon = "fa-calendar-xmark", PermissionCode = "ViewTeacherLeave", MenuOrder = 3, IsActive = true },

                // Examinations
                new() { Id = 18, SectionTitle = "Examinations", SectionOrder = 5, Title = "Examinations", Controller = "Exam", Action = "Index", Area = "", Icon = "fa-file-pen", PermissionCode = "ViewExam", MenuOrder = 1, IsActive = true },

                // Finance
                new() { Id = 19, SectionTitle = "Finance", SectionOrder = 6, Title = "Fees & Payments", Controller = "Fees", Action = "Index", Area = "", Icon = "fa-indian-rupee-sign", PermissionCode = "ViewFees", MenuOrder = 1, IsActive = true },
                new() { Id = 20, SectionTitle = "Finance", SectionOrder = 6, Title = "Expenses", Controller = "Expense", Action = "Index", Area = "", Icon = "fa-receipt", PermissionCode = "ViewExpense", MenuOrder = 2, IsActive = true },

                // Communication
                new() { Id = 21, SectionTitle = "Communication", SectionOrder = 7, Title = "Announcements", Controller = "Announcements", Action = "Index", Area = "", Icon = "fa-bullhorn", PermissionCode = "ViewAnnouncement", MenuOrder = 1, IsActive = true },
                new() { Id = 22, SectionTitle = "Communication", SectionOrder = 7, Title = "Notification Hub", Controller = "NotificationConfig", Action = "Index", Area = "", Icon = "fa-sliders", PermissionCode = "ViewNotificationConfig", MenuOrder = 2, IsActive = true },

                // System Setup
                new() { Id = 23, SectionTitle = "System Setup", SectionOrder = 8, Title = "Master Management", Controller = "Master", Action = "Index", Area = "", Icon = "fa-database", PermissionCode = "ViewMaster", MenuOrder = 1, IsCollapsible = true, CollapseTargetId = "masterMenuCollapse", IsMasterManagement = true, IsActive = true },
                new() { Id = 24, SectionTitle = "System Setup", SectionOrder = 8, Title = "Sidebar Management", Controller = "SidebarMenu", Action = "Index", Area = "", Icon = "fa-bars-staggered", PermissionCode = "ViewSidebarMenu", MenuOrder = 2, IsActive = true },
                new() { Id = 25, SectionTitle = "System Setup", SectionOrder = 8, Title = "Data Backup & Export", Controller = "DataBackup", Action = "Index", Area = "", Icon = "fa-cloud-arrow-down", PermissionCode = "ViewDataBackup", MenuOrder = 3, IsActive = true },
                new() { Id = 26, SectionTitle = "System Setup", SectionOrder = 8, Title = "Users & Roles", Controller = "", Action = "", Area = "", Icon = "fa-user-shield", PermissionCode = "ViewUserRoles", MenuOrder = 4, BadgeText = "Soon", BadgeClass = "IMS-soon-badge", CustomUrl = "javascript:void(0);", IsActive = true },

                // Insights
                new() { Id = 27, SectionTitle = "Insights", SectionOrder = 9, Title = "Reports", Controller = "", Action = "", Area = "", Icon = "fa-chart-line", PermissionCode = "ViewReports", MenuOrder = 1, BadgeText = "Soon", BadgeClass = "IMS-soon-badge", CustomUrl = "javascript:void(0);", IsActive = true }
            };
        }
    }
}
