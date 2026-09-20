using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMS.Models.Notification;

namespace IMS.Services.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationConfigModel> GetConfigAsync(Guid tenantId);
        Task<bool> SaveConfigAsync(Guid tenantId, NotificationConfigModel config);

        Task<bool> RaiseNotificationAsync(
            Guid tenantId,
            string eventCode,
            string title,
            string message,
            string? linkUrl = null,
            string? targetRole = "TENANT_ADMIN",
            string? userId = null,
            string? recipientEmail = null);

        Task<HeaderNotificationResult> GetHeaderNotificationsAsync(Guid tenantId, string? userId, string? role);
        Task<bool> MarkAsReadAsync(Guid notificationId);
        Task<bool> MarkAllAsReadAsync(Guid tenantId, string? userId, string? role);
    }
}
