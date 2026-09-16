using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;
using IMS.DAL.Common;
using IMS.DAL.Interfaces;
using IMS.Models.Notification;

namespace IMS.DAL
{
    public class NotificationConfigDAL : INotificationConfigDAL
    {
        private readonly DBHelper _dbHelper;
        private static bool _tableEnsured = false;
        private static readonly object _lock = new object();

        public NotificationConfigDAL(DBHelper dbHelper)
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
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NotificationSettings')
BEGIN
    CREATE TABLE NotificationSettings (
        NS_TenantId UNIQUEIDENTIFIER PRIMARY KEY,
        NS_InAppEnabled BIT NOT NULL DEFAULT 1,
        NS_EmailEnabled BIT NOT NULL DEFAULT 0,
        NS_SmtpHost NVARCHAR(200) NULL,
        NS_SmtpPort INT NOT NULL DEFAULT 587,
        NS_SmtpUsername NVARCHAR(200) NULL,
        NS_SmtpPassword NVARCHAR(500) NULL,
        NS_FromEmail NVARCHAR(200) NULL,
        NS_FromName NVARCHAR(200) NULL,
        NS_EnableSsl BIT NOT NULL DEFAULT 1,
        NS_EventConfigJson NVARCHAR(MAX) NULL,
        NS_UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END";
                    cmd.ExecuteNonQuery();
                    _tableEnsured = true;
                }
                catch
                {
                    // Fallback if table already exists or permissions issue
                    _tableEnsured = true;
                }
            }
        }

        public async Task<NotificationConfigModel> GetConfigAsync(Guid tenantId)
        {
            try
            {
                using var conn = _dbHelper.GetConnection();
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT NS_InAppEnabled, NS_EmailEnabled, NS_SmtpHost, NS_SmtpPort, 
                                           NS_SmtpUsername, NS_SmtpPassword, NS_FromEmail, NS_FromName, 
                                           NS_EnableSsl, NS_EventConfigJson 
                                    FROM NotificationSettings 
                                    WHERE NS_TenantId = @TenantId";
                cmd.Parameters.AddWithValue("@TenantId", tenantId);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var config = new NotificationConfigModel
                    {
                        TenantId = tenantId,
                        InAppEnabled = reader.GetBoolean(reader.GetOrdinal("NS_InAppEnabled")),
                        EmailEnabled = reader.GetBoolean(reader.GetOrdinal("NS_EmailEnabled")),
                        SmtpHost = reader["NS_SmtpHost"] as string ?? "smtp.gmail.com",
                        SmtpPort = reader.GetInt32(reader.GetOrdinal("NS_SmtpPort")),
                        SmtpUsername = reader["NS_SmtpUsername"] as string ?? "",
                        SmtpPassword = reader["NS_SmtpPassword"] as string ?? "",
                        FromEmail = reader["NS_FromEmail"] as string ?? "noreply@ims-erp.com",
                        FromName = reader["NS_FromName"] as string ?? "IMS ERP System",
                        EnableSsl = reader.GetBoolean(reader.GetOrdinal("NS_EnableSsl"))
                    };

                    var json = reader["NS_EventConfigJson"] as string;
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        try
                        {
                            var customEvents = JsonSerializer.Deserialize<List<NotificationEventSetting>>(json);
                            if (customEvents != null && customEvents.Count > 0)
                            {
                                config.Events = MergeWithDefaultEvents(customEvents);
                            }
                            else
                            {
                                config.Events = NotificationDefaultEvents.GetDefaultEvents();
                            }
                        }
                        catch
                        {
                            config.Events = NotificationDefaultEvents.GetDefaultEvents();
                        }
                    }
                    else
                    {
                        config.Events = NotificationDefaultEvents.GetDefaultEvents();
                    }

                    return config;
                }
            }
            catch
            {
                // Fallback on error to default model
            }

            return new NotificationConfigModel
            {
                TenantId = tenantId,
                InAppEnabled = true,
                EmailEnabled = false,
                Events = NotificationDefaultEvents.GetDefaultEvents()
            };
        }

        private List<NotificationEventSetting> MergeWithDefaultEvents(List<NotificationEventSetting> savedEvents)
        {
            var defaults = NotificationDefaultEvents.GetDefaultEvents();
            var result = new List<NotificationEventSetting>();

            foreach (var def in defaults)
            {
                var saved = savedEvents.Find(e => string.Equals(e.EventCode, def.EventCode, StringComparison.OrdinalIgnoreCase));
                if (saved != null)
                {
                    saved.EventName = def.EventName;
                    saved.Category = def.Category;
                    saved.Description = def.Description;
                    saved.IconClass = def.IconClass;
                    saved.ColorClass = def.ColorClass;
                    result.Add(saved);
                }
                else
                {
                    result.Add(def);
                }
            }

            return result;
        }

        public async Task<bool> SaveConfigAsync(Guid tenantId, NotificationConfigModel config)
        {
            try
            {
                var eventJson = JsonSerializer.Serialize(config.Events ?? NotificationDefaultEvents.GetDefaultEvents());

                using var conn = _dbHelper.GetConnection();
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
IF EXISTS (SELECT 1 FROM NotificationSettings WHERE NS_TenantId = @TenantId)
BEGIN
    UPDATE NotificationSettings 
    SET NS_InAppEnabled = @InAppEnabled,
        NS_EmailEnabled = @EmailEnabled,
        NS_SmtpHost = @SmtpHost,
        NS_SmtpPort = @SmtpPort,
        NS_SmtpUsername = @SmtpUsername,
        NS_SmtpPassword = @SmtpPassword,
        NS_FromEmail = @FromEmail,
        NS_FromName = @FromName,
        NS_EnableSsl = @EnableSsl,
        NS_EventConfigJson = @EventConfigJson,
        NS_UpdatedAt = SYSUTCDATETIME()
    WHERE NS_TenantId = @TenantId;
END
ELSE
BEGIN
    INSERT INTO NotificationSettings (
        NS_TenantId, NS_InAppEnabled, NS_EmailEnabled, NS_SmtpHost, 
        NS_SmtpPort, NS_SmtpUsername, NS_SmtpPassword, NS_FromEmail, 
        NS_FromName, NS_EnableSsl, NS_EventConfigJson, NS_UpdatedAt
    ) VALUES (
        @TenantId, @InAppEnabled, @EmailEnabled, @SmtpHost, 
        @SmtpPort, @SmtpUsername, @SmtpPassword, @FromEmail, 
        @FromName, @EnableSsl, @EventConfigJson, SYSUTCDATETIME()
    );
END";

                cmd.Parameters.AddWithValue("@TenantId", tenantId);
                cmd.Parameters.AddWithValue("@InAppEnabled", config.InAppEnabled);
                cmd.Parameters.AddWithValue("@EmailEnabled", config.EmailEnabled);
                cmd.Parameters.AddWithValue("@SmtpHost", (object?)config.SmtpHost ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SmtpPort", config.SmtpPort);
                cmd.Parameters.AddWithValue("@SmtpUsername", (object?)config.SmtpUsername ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SmtpPassword", (object?)config.SmtpPassword ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FromEmail", (object?)config.FromEmail ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FromName", (object?)config.FromName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EnableSsl", config.EnableSsl);
                cmd.Parameters.AddWithValue("@EventConfigJson", eventJson);

                var rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
