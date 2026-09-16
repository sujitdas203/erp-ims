using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMS.Models.Notification;

namespace IMS.DAL.Interfaces
{
    public interface INotificationDAL
    {
        Task<Guid> CreateNotificationAsync(UserNotification notification);
        Task<List<UserNotification>> GetRecentNotificationsAsync(Guid tenantId, string? userId, string? role, int limit = 15);
        Task<int> GetUnreadCountAsync(Guid tenantId, string? userId, string? role);
        Task<bool> MarkAsReadAsync(Guid notificationId);
        Task<bool> MarkAllAsReadAsync(Guid tenantId, string? userId, string? role);
    }
}
