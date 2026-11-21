using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using IMS.DAL.Common;
using IMS.DAL.Interfaces;
using IMS.Models.Notification;

namespace IMS.DAL
{
    public class NotificationDAL : INotificationDAL
    {
        private readonly DBHelper _dbHelper;
        private static bool _tableEnsured = false;
        private static readonly object _lock = new object();

        public NotificationDAL(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
            EnsureTableCreated();
        }

        private void EnsureTableCreated()
        {
            if (_tableEnsured) return;
            lock (_lock)
            {
                if (_tableEnsured) return;
                try
                {
                    using var conn = _dbHelper.GetConnection();
                    conn.Open();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = @"
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserNotifications')
BEGIN
    CREATE TABLE UserNotifications (
        UN_Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        UN_TenantId UNIQUEIDENTIFIER NOT NULL,
        UN_UserId NVARCHAR(100) NULL,
        UN_TargetRole NVARCHAR(100) NULL DEFAULT 'TENANT_ADMIN',
        UN_Title NVARCHAR(250) NOT NULL,
        UN_Message NVARCHAR(1000) NOT NULL,
        UN_EventType NVARCHAR(100) NOT NULL,
        UN_LinkUrl NVARCHAR(500) NULL,
        UN_IsRead BIT NOT NULL DEFAULT 0,
        UN_CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE INDEX IX_UserNotifications_Tenant ON UserNotifications(UN_TenantId, UN_IsRead, UN_CreatedAt DESC);
END";
                    cmd.ExecuteNonQuery();
                    _tableEnsured = true;
                }
                catch
                {
                    _tableEnsured = true;
                }
            }
        }

        public async Task<Guid> CreateNotificationAsync(UserNotification notification)
        {
            try
            {
                if (notification.Id == Guid.Empty)
                    notification.Id = Guid.NewGuid();

                using var conn = _dbHelper.GetConnection();
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
INSERT INTO UserNotifications (
    UN_Id, UN_TenantId, UN_UserId, UN_TargetRole, UN_Title, 
    UN_Message, UN_EventType, UN_LinkUrl, UN_IsRead, UN_CreatedAt
) VALUES (
    @Id, @TenantId, @UserId, @TargetRole, @Title, 
    @Message, @EventType, @LinkUrl, @IsRead, SYSUTCDATETIME()
)";

                cmd.Parameters.AddWithValue("@Id", notification.Id);
                cmd.Parameters.AddWithValue("@TenantId", notification.TenantId);
                cmd.Parameters.AddWithValue("@UserId", (object?)notification.UserId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TargetRole", (object?)notification.TargetRole ?? "TENANT_ADMIN");
                cmd.Parameters.AddWithValue("@Title", notification.Title);
                cmd.Parameters.AddWithValue("@Message", notification.Message);
                cmd.Parameters.AddWithValue("@EventType", notification.EventType);
                cmd.Parameters.AddWithValue("@LinkUrl", (object?)notification.LinkUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsRead", notification.IsRead);

                await cmd.ExecuteNonQueryAsync();
                return notification.Id;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public async Task<List<UserNotification>> GetRecentNotificationsAsync(Guid tenantId, string? userId, string? role, int limit = 15)
        {
            var list = new List<UserNotification>();
            try
            {
                using var conn = _dbHelper.GetConnection();
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = $@"
SELECT TOP ({limit}) UN_Id, UN_TenantId, UN_UserId, UN_TargetRole, UN_Title, 
                     UN_Message, UN_EventType, UN_LinkUrl, UN_IsRead, UN_CreatedAt
FROM UserNotifications
WHERE UN_TenantId = @TenantId
  AND (UN_UserId = @UserId OR UN_UserId IS NULL OR @UserId IS NULL)
  AND (UN_TargetRole = @Role OR UN_TargetRole = 'ALL' OR UN_TargetRole IS NULL OR @Role IS NULL)
ORDER BY UN_CreatedAt DESC";

                cmd.Parameters.AddWithValue("@TenantId", tenantId);
                cmd.Parameters.AddWithValue("@UserId", (object?)userId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Role", (object?)role ?? DBNull.Value);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var createdAt = reader.GetDateTime(reader.GetOrdinal("UN_CreatedAt"));
                    var eventType = reader["UN_EventType"] as string ?? "GENERAL";

                    var item = new UserNotification
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("UN_Id")),
                        TenantId = reader.GetGuid(reader.GetOrdinal("UN_TenantId")),
                        UserId = reader["UN_UserId"] as string,
                        TargetRole = reader["UN_TargetRole"] as string,
                        Title = reader["UN_Title"] as string ?? "",
                        Message = reader["UN_Message"] as string ?? "",
                        EventType = eventType,
                        LinkUrl = reader["UN_LinkUrl"] as string,
                        IsRead = reader.GetBoolean(reader.GetOrdinal("UN_IsRead")),
                        CreatedAt = createdAt,
                        TimeAgo = FormatTimeAgo(createdAt),
                        IconClass = GetIconForEvent(eventType),
                        BadgeColor = GetColorForEvent(eventType)
                    };
                    list.Add(item);
                }
            }
            catch
            {
                // Return empty list on error
            }

            return list;
        }

        public async Task<int> GetUnreadCountAsync(Guid tenantId, string? userId, string? role)
        {
            try
            {
                using var conn = _dbHelper.GetConnection();
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
SELECT COUNT(1)
FROM UserNotifications
WHERE UN_TenantId = @TenantId
  AND UN_IsRead = 0
  AND (UN_UserId = @UserId OR UN_UserId IS NULL OR @UserId IS NULL)
  AND (UN_TargetRole = @Role OR UN_TargetRole = 'ALL' OR UN_TargetRole IS NULL OR @Role IS NULL)";

                cmd.Parameters.AddWithValue("@TenantId", tenantId);
                cmd.Parameters.AddWithValue("@UserId", (object?)userId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Role", (object?)role ?? DBNull.Value);

                var scalar = await cmd.ExecuteScalarAsync();
                return scalar != null && scalar != DBNull.Value ? Convert.ToInt32(scalar) : 0;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> MarkAsReadAsync(Guid notificationId)
        {
            try
            {
                using var conn = _dbHelper.GetConnection();
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE UserNotifications SET UN_IsRead = 1 WHERE UN_Id = @Id";
                cmd.Parameters.AddWithValue("@Id", notificationId);

                var rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> MarkAllAsReadAsync(Guid tenantId, string? userId, string? role)
        {
            try
            {
                using var conn = _dbHelper.GetConnection();
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
UPDATE UserNotifications 
SET UN_IsRead = 1 
WHERE UN_TenantId = @TenantId
  AND UN_IsRead = 0
  AND (UN_UserId = @UserId OR UN_UserId IS NULL OR @UserId IS NULL)
  AND (UN_TargetRole = @Role OR UN_TargetRole = 'ALL' OR UN_TargetRole IS NULL OR @Role IS NULL)";

                cmd.Parameters.AddWithValue("@TenantId", tenantId);
                cmd.Parameters.AddWithValue("@UserId", (object?)userId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Role", (object?)role ?? DBNull.Value);

                var rows = await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string FormatTimeAgo(DateTime dt)
        {
            var span = DateTime.UtcNow - dt;
            if (span.TotalMinutes < 1) return "Just now";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes}m ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours}h ago";
            if (span.TotalDays < 7) return $"{(int)span.TotalDays}d ago";
            return dt.ToString("dd MMM");
        }

        private static string GetIconForEvent(string eventType)
        {
            return eventType.ToUpperInvariant() switch
            {
                "ADMISSION_SUBMITTED" => "fa-solid fa-address-book",
                "FEE_RECEIVED" => "fa-solid fa-indian-rupee-sign",
                "FEE_OVERDUE" => "fa-solid fa-file-invoice-dollar",
                "TEACHER_LEAVE_APPLIED" => "fa-solid fa-calendar-xmark",
                "STUDENT_LEAVE_APPLIED" => "fa-solid fa-user-clock",
                "MOCK_TEST_PUBLISHED" => "fa-solid fa-file-circle-check",
                "ANNOUNCEMENT_PUBLISHED" => "fa-solid fa-bullhorn",
                "TC_GENERATED" => "fa-solid fa-file-shield",
                "LOW_ATTENDANCE" => "fa-solid fa-triangle-exclamation",
                _ => "fa-solid fa-bell"
            };
        }

        private static string GetColorForEvent(string eventType)
        {
            return eventType.ToUpperInvariant() switch
            {
                "ADMISSION_SUBMITTED" => "primary",
                "FEE_RECEIVED" => "success",
                "FEE_OVERDUE" => "danger",
                "TEACHER_LEAVE_APPLIED" => "warning",
                "STUDENT_LEAVE_APPLIED" => "info",
                "MOCK_TEST_PUBLISHED" => "purple",
                "ANNOUNCEMENT_PUBLISHED" => "amber",
                "TC_GENERATED" => "rose",
                "LOW_ATTENDANCE" => "danger",
                _ => "secondary"
            };
        }
    }
}
