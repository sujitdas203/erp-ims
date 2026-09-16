using System;
using System.Linq;
using System.Threading.Tasks;
using IMS.DAL.Interfaces;
using IMS.Models.Notification;
using IMS.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace IMS.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationConfigDAL _configDAL;
        private readonly INotificationDAL _notificationDAL;
        private readonly IEmailService _emailService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationConfigDAL configDAL,
            INotificationDAL notificationDAL,
            IEmailService emailService,
            ILogger<NotificationService> logger)
        {
            _configDAL = configDAL;
            _notificationDAL = notificationDAL;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<NotificationConfigModel> GetConfigAsync(Guid tenantId)
        {
            return await _configDAL.GetConfigAsync(tenantId);
        }

        public async Task<bool> SaveConfigAsync(Guid tenantId, NotificationConfigModel config)
        {
            return await _configDAL.SaveConfigAsync(tenantId, config);
        }

        public async Task<bool> RaiseNotificationAsync(
            Guid tenantId,
            string eventCode,
            string title,
            string message,
            string? linkUrl = null,
            string? targetRole = "TENANT_ADMIN",
            string? userId = null,
            string? recipientEmail = null)
        {
            try
            {
                var config = await _configDAL.GetConfigAsync(tenantId);
                var eventSetting = config.Events?.FirstOrDefault(e =>
                    string.Equals(e.EventCode, eventCode, StringComparison.OrdinalIgnoreCase));

                bool inAppAllowed = config.InAppEnabled && (eventSetting == null || eventSetting.InAppEnabled);
                bool emailAllowed = config.EmailEnabled && (eventSetting != null && eventSetting.EmailEnabled);

                // 1. Process In-App Notification
                if (inAppAllowed)
                {
                    var userNotification = new UserNotification
                    {
                        Id = Guid.NewGuid(),
                        TenantId = tenantId,
                        UserId = userId,
                        TargetRole = targetRole,
                        Title = title,
                        Message = message,
                        EventType = eventCode,
                        LinkUrl = linkUrl,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _notificationDAL.CreateNotificationAsync(userNotification);
                }

                // 2. Process Email Alert (if enabled and recipient email provided)
                if (emailAllowed && !string.IsNullOrWhiteSpace(recipientEmail))
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            var htmlBody = $@"
<div style='font-family:sans-serif; max-width:560px; margin:auto; padding:20px; border:1px solid #e2e8f0; border-radius:10px;'>
    <h3 style='color:#0f172a; margin-top:0;'>{title}</h3>
    <p style='color:#334155; line-height:1.6;'>{message}</p>
    {(string.IsNullOrEmpty(linkUrl) ? "" : $"<a href='{linkUrl}' style='display:inline-block; background:#2563eb; color:#ffffff; padding:8px 16px; border-radius:6px; text-decoration:none; font-weight:600;'>View in IMS ERP</a>")}
    <hr style='border:none; border-top:1px solid #f1f5f9; margin:20px 0;' />
    <p style='font-size:12px; color:#94a3b8;'>This is an automated operational notification from IMS ERP.</p>
</div>";
                            await _emailService.SendEmailAsync(recipientEmail, $"[IMS Alert] {title}", htmlBody, config);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed background notification email dispatch for {EventCode}", eventCode);
                        }
                    });
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to raise notification for {EventCode}", eventCode);
                return false;
            }
        }

        public async Task<HeaderNotificationResult> GetHeaderNotificationsAsync(Guid tenantId, string? userId, string? role)
        {
            var notifications = await _notificationDAL.GetRecentNotificationsAsync(tenantId, userId, role, 15);
            var unreadCount = await _notificationDAL.GetUnreadCountAsync(tenantId, userId, role);

            return new HeaderNotificationResult
            {
                UnreadCount = unreadCount,
                Notifications = notifications
            };
        }

        public async Task<bool> MarkAsReadAsync(Guid notificationId)
        {
            return await _notificationDAL.MarkAsReadAsync(notificationId);
        }

        public async Task<bool> MarkAllAsReadAsync(Guid tenantId, string? userId, string? role)
        {
            return await _notificationDAL.MarkAllAsReadAsync(tenantId, userId, role);
        }
    }
}
