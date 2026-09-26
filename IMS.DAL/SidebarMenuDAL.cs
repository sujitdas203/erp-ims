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

            const string checkSql = @"
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
                END";

            try
            {
                using var conn = _dbHelper.GetConnection();
                using var cmd = new SqlCommand(checkSql, conn);
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
            await EnsureTableAndSeedAsync();
            var list = new List<SidebarMenuItemModel>();

            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SidebarMenuItems_GetActive", conn);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapRow(reader));
            }
            return list;
        }

        public async Task<List<SidebarMenuItemModel>> GetAllMenuItemsAsync()
        {
            await EnsureTableAndSeedAsync();
            var list = new List<SidebarMenuItemModel>();

            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SidebarMenuItems_GetAll", conn);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapRow(reader));
            }
            return list;
        }

        public async Task<SidebarMenuItemModel?> GetByIdAsync(int id)
        {
            await EnsureTableAndSeedAsync();
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SidebarMenuItems_GetById", conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapRow(reader);
            }
            return null;
        }

        public async Task<int> CreateAsync(SidebarMenuItemModel item)
        {
            await EnsureTableAndSeedAsync();
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SidebarMenuItems_Create", conn);
            AddParameters(cmd, item);

            await conn.OpenAsync();
            var res = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(res);
        }

        public async Task<bool> UpdateAsync(SidebarMenuItemModel item)
        {
            await EnsureTableAndSeedAsync();
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SidebarMenuItems_Update", conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = item.Id;
            AddParameters(cmd, item);

            await conn.OpenAsync();
            var rows = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await EnsureTableAndSeedAsync();
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SidebarMenuItems_Delete", conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await conn.OpenAsync();
            var rows = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return rows > 0;
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            await EnsureTableAndSeedAsync();
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SidebarMenuItems_ToggleStatus", conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await conn.OpenAsync();
            var rows = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return rows > 0;
        }

        public async Task<bool> UpdateOrderAsync(int id, int sectionOrder, int menuOrder)
        {
            await EnsureTableAndSeedAsync();
            using var conn = _dbHelper.GetConnection();
            using var cmd = _dbHelper.CreateCommand("SP_SidebarMenuItems_UpdateOrder", conn);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@SectionOrder", SqlDbType.Int).Value = sectionOrder;
            cmd.Parameters.Add("@MenuOrder", SqlDbType.Int).Value = menuOrder;

            await conn.OpenAsync();
            var rows = Convert.ToInt32(await cmd.ExecuteScalarAsync());
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
    }
}
